using System;
using System.Collections.Generic;
using game.core.storage.Data.Equipment;
using game.core.storage.Data.Equipment.Weapon;
using game.core.storage.Data.Models;
using game.Gameplay.Characters.Common;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
    public class CharacterEquipmentManger
    {
        private EquipmentData _currentEquipmentData;
        private EquipmentModel _currentEquipment;
        private EquipmentViewBase _currentEquipmentView;
        private CharacterAnimation _animation;
        private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;
        private Dictionary<Type, ICharacterEquiper<EquipmentData, EquipmentViewBase>> _equipers = new ();

        public EquipmentData currentEquipmentData => _currentEquipmentData;
        public EquipmentModel currentEquipment => _currentEquipment;
        public EquipmentViewBase currentEquipmentView => _currentEquipmentView;
        

        public void Init(CharacterAnimation animation, CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine, 
            CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine)
        {
            _animation = animation;
            _mainStateMachine = mainStateMachine;
            _actionStateMachine = actionStateMachine;
        }
        
        public void AddEquiper(ICharacterEquiper<EquipmentData, EquipmentViewBase> equiper) {
            _equipers.Add(equiper.GetDataType(), equiper);
        }

        public void Equip(EquipmentData equipment)
        {
            _currentEquipmentData = equipment;

            _currentEquipment = _currentEquipmentData.CreateModel();
            _animation.SetAnimationSet(equipment.animationSet);
            
            //TODO: REMOVE DIRTY HACK!
            var baseType = equipment.GetType().BaseType;
            if (baseType != null && _equipers.ContainsKey(baseType)) {
                _currentEquipmentView = _equipers[baseType].Equip(equipment);
            }
            
            _currentEquipmentView.Init(_currentEquipment);
            
            foreach (var state in equipment.statesOverrides) {
                _mainStateMachine.ReplaceState(state.overrideStateType, state.GetState());
            }
            
            foreach (var state in equipment.actionStatesOverrides) {
                _actionStateMachine.ReplaceState(state.overrideStateType, state.GetState());
            }
        }

        public void Unequip()
        {
            if (_equipers.ContainsKey(_currentEquipment.GetType())) {
                _equipers[_currentEquipment.GetType()].Unequip(_currentEquipmentView);
            }
        }


        public HealthChange<DamageType> GetDamageValue()
        {
            return _currentEquipment.GetDamage();
        }
        
    }

    public interface ICharacterEquiper<T, TView>
    {
        Type GetDataType();
        TView Equip(T equipment);
        void Unequip(TView equipmentView);
    }
}