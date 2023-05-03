using System;
using game.core;
using game.core.Common;
using game.core.storage;
using game.Gameplay;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace game.Source.Gameplay.Weapon
{
    public class ProjectileView : MonoBehaviour
    {
        protected Whistle<Healthable, ProjectileView> _onHitHealthable = new ();

        public IWhistle<Healthable, ProjectileView> onHitHealthable => _onHitHealthable;


        private Vector3 _lastPosition;
        private RaycastHit[] _raycastHits = new RaycastHit[10];
        private bool _isStopped;

        public bool isStopped => _isStopped;
        
        private void Start() {
            _lastPosition = transform.position;
        }
        
        private void Update() {
            if (_lastPosition == Vector3.zero || _isStopped) {
                return;
            }
            
            Physics.RaycastNonAlloc(_lastPosition, transform.position,  _raycastHits, Vector3.Distance(transform.position, _lastPosition), (int) GameLayers.HEALTHABLE_OBJECTS);
            Debug.DrawLine(_lastPosition, transform.position);
            if (_raycastHits.Length == 0) {
                return;
            }

            var hit = _raycastHits[0];

            AppCore.Get<LevelManager>().SpawnDebugObject(hit.point, .1f);

            if (hit.transform == null) {
                return;
            }

            var healthable = hit.transform.gameObject.GetComponent<Healthable>();

            if (healthable == null) {
                return;
            }
            
            _onHitHealthable.Dispatch(healthable, this);
        }

        public void Stop() {
            _isStopped = true;
        }
    }
}