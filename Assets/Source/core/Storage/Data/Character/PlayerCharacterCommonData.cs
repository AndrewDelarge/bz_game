using System.Collections.Generic;
using game.Gameplay.Characters;
using game.Gameplay.Characters.Player;
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


        private Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, PlayerCharacterContext>> _states =
            new () {
                {CharacterStateEnum.IDLE, new PlayerIdleState()},
                {CharacterStateEnum.WALK, new PlayerWalkState()},
                {CharacterStateEnum.RUN, new PlayerRunState()},
            };
        
        private Dictionary<PlayerActionStateEnum, CharacterState<PlayerActionStateEnum, PlayerCharacterContext>> _actionStates =
            new () {
                {PlayerActionStateEnum.IDLE, new PlayerActionIdleState()},
                {PlayerActionStateEnum.KICK, new PlayerActionKickState()},
            };
        
        public float kickPhysicsImpulseDelay => _kickPhysicsImpulseDelay;
        public float kickFlightSphereDistance => _kickFlightSphereDistance;
        public float kickSphereRadius => _kickSphereRadius;
        public float kickAngle => _kickAngle;
        public float kickPower => _kickPower;
        public float yKick => _yKick;

        public Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, PlayerCharacterContext>> states =>
            _states;
        public Dictionary<PlayerActionStateEnum, CharacterState<PlayerActionStateEnum, PlayerCharacterContext>> actionStates =>
            _actionStates;
    }
}