using System.Collections.Generic;
using game.core;
using game.core.Common;
using game.core.storage;
using game.Gameplay;
using UnityEngine;

namespace game.Gameplay.Weapon
{
    public class ProjectileView : MonoBehaviour
    {
        protected Whistle<Healthable, ProjectileView> _onHitHealthable = new ();

        public IWhistle<Healthable, ProjectileView> onHitHealthable => _onHitHealthable;

        private RaycastHit[] _raycastHitsRaw = new RaycastHit[10];
        private List<RaycastHit> _raycastHits = new List<RaycastHit>(10);
        private bool _isStopped;
        private bool _isInited;
        private Vector3 _targetPosition;
        
        public bool isStopped => _isStopped;
        
        
        public void Move(Vector3 position) {
            var currentPosition = transform.position;
            _targetPosition = transform.TransformDirection(position);

            Physics.RaycastNonAlloc(currentPosition, _targetPosition,  _raycastHitsRaw, Vector3.Distance(currentPosition, _targetPosition), (int) GameLayers.HEALTHABLE_OBJECTS);

            _raycastHits.Clear();
            
            foreach (var raycastHit in _raycastHitsRaw) {
                if (raycastHit.transform != null) {
                    _raycastHits.Add(raycastHit);
                }
            }

            if (_raycastHits.Count == 0) {
                transform.position += _targetPosition;
                return;
            }
            
            _raycastHits.Sort((x, y) => Vector3.Distance(currentPosition, x.point).CompareTo(Vector3.Distance(currentPosition, y.point)));
            
            foreach (var raycastHit in _raycastHits) {
                if (_isStopped) {
                    break;
                }
                
                handleHit(raycastHit);
                
                Debug.DrawLine(transform.position, raycastHit.point);
                transform.position = raycastHit.point;
            }
        }

        public void Stop() {
            _isStopped = true;
        }

        private void handleHit(RaycastHit hit) {
            var healthable = hit.transform.gameObject.GetComponent<Healthable>();

            if (healthable == null) {
                return;
            }
            
            
            AppCore.Get<LevelManager>().SpawnDebugObject(hit.point, .1f);

            _onHitHealthable.Dispatch(healthable, this);
        }

        public void Dispose() {
            gameObject.SetActive(false);
            
            _onHitHealthable.Clear();
            _raycastHitsRaw = null;
            _raycastHits.Clear();
        }
    }
}