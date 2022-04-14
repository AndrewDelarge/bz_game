using UnityEngine;

namespace game.core.Storage.Data.Character
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Character/PlayerData")]
    public class PlayerCommonData : ScriptableObject
    {
        // Model data
        [SerializeField] private float _normalSpeed = 1.3f;
        [SerializeField] private float _speedMultiplier = 2.8f;
        [SerializeField] private float _speedSmoothTime = 2f;
        
        [Header("Kick data")]
        [SerializeField] private float _kickPhysicsImpulseDelay = .25f;
        [SerializeField] private float _kickFlightSphereDistance = 0f;
        [SerializeField] private float _kickSphereRadius = .8f;
        [SerializeField] private float _kickAngle = 50f;
        [SerializeField] private float _kickPower = 20;
        
        public float normalSpeed => _normalSpeed;
        public float speedMultiplier => _speedMultiplier;
        public float speedSmoothTime => _speedSmoothTime;
        public float kickPhysicsImpulseDelay => _kickPhysicsImpulseDelay;
        public float kickFlightSphereDistance => _kickFlightSphereDistance;
        public float kickSphereRadius => _kickSphereRadius;
        public float kickAngle => _kickAngle;
        public float kickPower => _kickPower;
    }
}