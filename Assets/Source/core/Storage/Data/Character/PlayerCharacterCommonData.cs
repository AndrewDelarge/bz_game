using UnityEngine;

namespace game.core.Storage.Data.Character
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "GameData/Character/PlayerData")]
    public class PlayerCharacterCommonData : CharacterCommonData
    {
        [Header("Kick data")]
        [SerializeField] private float _kickPhysicsImpulseDelay = .25f;
        [SerializeField] private float _kickFlightSphereDistance = 0f;
        [SerializeField] private float _kickSphereRadius = .8f;
        [SerializeField] private float _kickAngle = 50f;
        [SerializeField] private float _kickPower = 20;
        [SerializeField] private float _yKick = -.7f;
        
        public float kickPhysicsImpulseDelay => _kickPhysicsImpulseDelay;
        public float kickFlightSphereDistance => _kickFlightSphereDistance;
        public float kickSphereRadius => _kickSphereRadius;
        public float kickAngle => _kickAngle;
        public float kickPower => _kickPower;
        public float yKick => _yKick;
    }
}