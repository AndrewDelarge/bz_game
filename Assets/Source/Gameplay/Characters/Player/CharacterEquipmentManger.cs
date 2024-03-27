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
        private IEquipment _currentEquipment;
        private EquipmentViewBase _currentEquipmentView;
        private CharacterAnimation _animation;
        private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;
        private Dictionary<CharacterEquiperType, ICharacterEquiper<EquipmentData, EquipmentViewBase>> _equipers = new ();

        public EquipmentData currentEquipmentData => _currentEquipmentData;
        public IEquipment currentEquipment => _currentEquipment;
        public EquipmentViewBase currentEquipmentView => _currentEquipmentView;
        
        public bool isEquiped => _currentEquipment != null;


        public void Init(CharacterAnimation animation, CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine, 
            CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine)
        {
            _animation = animation;
            _mainStateMachine = mainStateMachine;
            _actionStateMachine = actionStateMachine;
        }
        
        public void AddEquiper(ICharacterEquiper<EquipmentData, EquipmentViewBase> equiper) {
            _equipers.Add(equiper.type, equiper);
        }

        public void Equip(EquipmentData equipment)
        {
            _currentEquipmentData = equipment;

            _currentEquipment = _currentEquipmentData.CreateModel();
            _animation.SetAnimationSet(equipment.animationSet);
            
            if (_equipers.ContainsKey(equipment.equiperType)) {
                _currentEquipmentView = _equipers[equipment.equiperType].Equip(equipment);
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
            if (isEquiped == false) {
                return;
            }

            foreach (var state in _currentEquipmentData.statesOverrides) {
                _mainStateMachine.RemoveReplacedState(state.overrideStateType);
            }
            
            foreach (var state in _currentEquipmentData.actionStatesOverrides) {
                _actionStateMachine.RemoveReplacedState(state.overrideStateType);
            }

            if (_equipers.ContainsKey(_currentEquipmentData.equiperType)) {
                _equipers[_currentEquipmentData.equiperType].Unequip(_currentEquipmentView);
            }

            _animation.ResetAnimationSetToDefault();
            
            _currentEquipmentData = null;
            _currentEquipment = null;
            _currentEquipmentView = null;
        }
    }

    public interface ICharacterEquiper<T, TView>
    {
        CharacterEquiperType type { get; }
        TView Equip(T equipment);
        void Unequip(TView equipmentView);
    }

    public enum CharacterEquiperType {
        WEAPON = 0
    }
}