using game.core.Common;
using game.Gameplay;
using UnityEngine;

namespace game.Source.Gameplay.Weapon
{
    public class ProjectileView : MonoBehaviour
    {
        protected Whistle<Healthable> _onHitHealthable = new ();
        // protected Whistle<Rigidbody> _onHit;

        public IWhistle<Healthable> onHitHealthable => _onHitHealthable;

        private void OnCollisionEnter(Collision collision) {
            var healthable = collision.gameObject.GetComponent<Healthable>();

            if (healthable != null) {
                _onHitHealthable.Dispatch(healthable);
            }
        }
    }
}