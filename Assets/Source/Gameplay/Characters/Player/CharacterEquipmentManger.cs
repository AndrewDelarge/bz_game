using System;
using System.Collections.Generic;
using game.core.storage.Data.Equipment;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
    public class CharacterEquipmentManger
    {
        private EquipmentData _currentEquipment;
        private GameObject _currentEquipmentView;
        private CharacterAnimation _animation;
        private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;
        private Dictionary<Type, ICharacterEquiper<EquipmentData>> _equipers = new ();

        public EquipmentData currentEquipment => _currentEquipment;
        public GameObject currentEquipmentView => _currentEquipmentView;
        

        public void Init(CharacterAnimation animation, CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine, 
            CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine)
        {
            _animation = animation;
            _mainStateMachine = mainStateMachine;
            _actionStateMachine = actionStateMachine;
        }
        
        public void AddEquiper(ICharacterEquiper<EquipmentData> equiper) {
            _equipers.Add(equiper.GetDataType(), equiper);
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

            //TODO: REMOVE DIRTY HACK!
            var baseType = equipment.GetType().BaseType;
            if (baseType != null && _equipers.ContainsKey(baseType)) {
                _currentEquipmentView = _equipers[baseType].Equip(equipment);
            }
        }

        public void Unequip()
        {
            if (_equipers.ContainsKey(_currentEquipment.GetType())) {
                _equipers[_currentEquipment.GetType()].Unequip(_currentEquipmentView);
            }
        }
    }

    public interface ICharacterEquiper<T>
    {
        Type GetDataType();
        GameObject Equip(T equipment);
        void Unequip(GameObject equipmentView);
    }
}