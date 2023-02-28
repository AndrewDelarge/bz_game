using System.Collections.Generic;
using game.core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.core.storage.Data.Equipment;
using game.Gameplay.Characters;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
    public class PlayerCharacter : MonoBehaviour, IControlable, ICharacter
    {
        [Header("Components")]
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterAnimation _animation;
        [SerializeField] private BoneListenerManager _boneListenerManager;
        [SerializeField] private Healthable _healthable;
        
        // TODO: Camera manager and controller
        [SerializeField] private Camera _camera;


        [SerializeField] private CharacterAnimationSet _characterAnimationSet;
        [SerializeField] private PlayerCharacterCommonData data;

        private CharacterEquipmentManger _equipmentManger;
        private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;

        private InputData _data;
        private bool isInited;
        private EquipmentData _equipmentData;

        public bool isPlayer => true;
        public bool isListen => true;

        public void Init()
        {
            AppCore.Get<IInputManager>().RegisterControlable(this);

            _equipmentData = Resources.Load<EquipmentData>("Data/Weapons/ShotgunWeapon");
            
            _animation.Init(_characterAnimationSet);
            _equipmentManger = new CharacterEquipmentManger();
            
            InitStates();

            _equipmentManger.Init(_animation, _mainStateMachine, _actionStateMachine);
            
            isInited = true;
        }

        private void Update()
        {
            if (isInited == false) {
                return;
            }
            
            UpdateStateMachines();
        }

        private void UpdateStateMachines()
        {
            if (_data != null)
            {
                _actionStateMachine.HandleInput(_data);
                _mainStateMachine.HandleInput(_data);

                _data = null;
            }
            
            _actionStateMachine.HandleState();
            _mainStateMachine.HandleState();
        }

        private void InitStates()
        {
            var context = new PlayerCharacterContext(_healthable, _movement, _animation, data,
                _movement.transform, _equipmentManger, _boneListenerManager);
            
            _mainStateMachine = new CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext>(context,
                new Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, PlayerCharacterContext>>(){
                {CharacterStateEnum.IDLE, new PlayerIdleState()},
                {CharacterStateEnum.WALK, new PlayerWalkState()},
                {CharacterStateEnum.RUN, new PlayerRunState()},
            });

            _actionStateMachine = new CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext>(context, 
                new Dictionary<PlayerActionStateEnum, CharacterState<PlayerActionStateEnum, PlayerCharacterContext>>() {
                {PlayerActionStateEnum.IDLE, new PlayerActionIdleState()},
                {PlayerActionStateEnum.KICK, new PlayerActionKickState()},
            });


            context.mainStateMachine = _mainStateMachine;
            context.actionStateMachine = _actionStateMachine;
            context.camera = _camera;

            _mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
            _actionStateMachine.ChangeState(PlayerActionStateEnum.IDLE);
            
            _mainStateMachine.onStateChanged.Add(_actionStateMachine.OnStateChangeHandler);
            _actionStateMachine.onStateChanged.Add(_mainStateMachine.OnStateChangeHandler);
            
            
            _actionStateMachine.ChangeState(PlayerActionStateEnum.EQUIP);
        }
        

        public void OnDataUpdate(InputData data)
        {
            _data = data;

            if (data.GetAction(InputActionType.DEBUG_0) is {value: {status: InputStatus.DOWN}}) {
                Equip(_equipmentData);
            }
        }

        public void Equip(EquipmentData equipment) {
            // equiper for every equip type ? 
            
            _equipmentManger.Equip(equipment);
        }
    }

    public class CharacterEquipmentManger
    {
        private EquipmentData _currentEquipment;
        private CharacterAnimation _animation;
        private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;


        public EquipmentData currentEquipment => _currentEquipment;

        public void Init(CharacterAnimation animation, CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine, CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine)
        {
            _animation = animation;
            _mainStateMachine = mainStateMachine;
            _actionStateMachine = actionStateMachine;
        }

        public void Equip(EquipmentData equipment)
        {
            _currentEquipment = equipment;
            
            _animation.SetAnimationSet(equipment.animationSet);
            
            foreach (var state in equipment.statesOverrides) {
                _mainStateMachine.ReplaceState(state.overrideStateType, state.GetState());
            }
            
            foreach (var state in equipment.actionStatesOverrides) {
                _actionStateMachine.ReplaceState(state.overrideStateType, state.GetState());
            }
        }
    }
}