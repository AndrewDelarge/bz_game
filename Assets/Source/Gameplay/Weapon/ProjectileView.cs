using System;
using game.core.Common;
using game.core.storage;
using game.Gameplay;
using UnityEngine;

namespace game.Source.Gameplay.Weapon
{
    public class ProjectileView : MonoBehaviour
    {
        protected Whistle<Healthable, ProjectileView> _onHitHealthable = new ();
        // protected Whistle<Rigidbody> _onHit;

        public IWhistle<Healthable, ProjectileView> onHitHealthable => _onHitHealthable;


        private Vector3 _lastPosition;
        private RaycastHit[] _raycastHits = new RaycastHit[1];
        private bool _isStopped;
        
        private void Start() {
            _lastPosition = transform.position;
        }

        
        private void Update() {
            if (_lastPosition == Vector3.zero || _isStopped) {
                return;
            }
            
            Physics.RaycastNonAlloc(_lastPosition, transform.position,  _raycastHits, Vector3.Distance(transform.position, _lastPosition), (int) GameLayers.HEALTHABLE_OBJECTS);

            if (_raycastHits.Length == 0) {
                return;
            }

            var hit = _raycastHits[0];
            
            if (hit.transform == null) {
                return;
            }

            var healthable = hit.transform.gameObject.GetComponent<Healthable>();

            if (healthable == null) {
                return;
            }
            
            if (healthable != null) {
                _onHitHealthable.Dispatch(healthable, this);
            }
        }

        public void Stop() {
            _isStopped = true;
        }
    }
}