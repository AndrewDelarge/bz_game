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


        [SerializeField] private PlayerCharacterCommonData data;

        private CharacterEquipmentManger _equipmentManger;
        private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;

        private InputData _data;
        private bool isInited;
        private EquipmentData _equipmentData;

        public bool isPlayer => true;
        public HealthChange<DamageType> GetDamage()
        {
            return _equipmentManger.currentEquipment.GetDamage();
        }

        public bool isListen => true;

        public void Init()
        {
            AppCore.Get<IInputManager>().RegisterControlable(this);

            _equipmentData = Resources.Load<EquipmentData>("Data/Weapons/ShotgunWeapon");
            
            _animation.Init(data.animationSet);
            _equipmentManger = new CharacterEquipmentManger();
            
            InitStates();

            _equipmentManger.Init(_animation, _mainStateMachine, _actionStateMachine);
            _equipmentManger.AddEquiper(new WeaponEquiper(_boneListenerManager));
            
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
            
            _mainStateMachine = new CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext>(context, data.states);
            _actionStateMachine = new CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext>(context, data.actionStates);

            context.mainStateMachine = _mainStateMachine;
            context.actionStateMachine = _actionStateMachine;
            context.camera = _camera;

            _mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
            _actionStateMachine.ChangeState(PlayerActionStateEnum.IDLE);
            
            _mainStateMachine.onStateChanged.Add(_actionStateMachine.OnStateChangeHandler);
            _actionStateMachine.onStateChanged.Add(_mainStateMachine.OnStateChangeHandler);
        }
        

        public void OnDataUpdate(InputData data)
        {
            _data = data;

            if (data.GetAction(InputActionType.DEBUG_0) is {value: {status: InputStatus.DOWN}}) {
                Equip(_equipmentData);
            }
        }

        public void Equip(EquipmentData equipment) {
            _equipmentManger.Equip(equipment);
        }
    }
}