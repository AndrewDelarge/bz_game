using System;
using game.core.storage.Data.Equipment;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay.Weapon;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.Gameplay.Characters.Player
{
    public class WeaponEquiper : ICharacterEquiper<EquipmentData, EquipmentViewBase>
    {
        private BoneListenerManager _boneListenerManager;

        public WeaponEquiper(BoneListenerManager boneListenerManager)
        {
            _boneListenerManager = boneListenerManager;
        }
        
        public Type GetDataType() => typeof(WeaponData);
        
        public EquipmentViewBase Equip(EquipmentData equipment)
        {
            var weaponData = (WeaponData) equipment;

            var bone = _boneListenerManager.bones[BoneListenerManager.CharacterBone.RIGHT_HAND_WEAPON];
            
            return Object.Instantiate(weaponData.viewTemplate, bone.transform);
        }

        public void Unequip(EquipmentViewBase equipmentView)
        {
            Object.Destroy(equipmentView);
        }
    }
}