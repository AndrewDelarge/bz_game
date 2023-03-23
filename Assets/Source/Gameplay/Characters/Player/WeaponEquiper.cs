using System;
using game.core.storage.Data.Equipment;
using game.core.storage.Data.Equipment.Weapon;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.Gameplay.Characters.Player
{
    public class WeaponEquiper : ICharacterEquiper<EquipmentData>
    {
        private BoneListenerManager _boneListenerManager;

        public WeaponEquiper(BoneListenerManager boneListenerManager)
        {
            _boneListenerManager = boneListenerManager;
        }
        
        public Type GetDataType() => typeof(WeaponData);
        
        public GameObject Equip(EquipmentData equipment)
        {
            var weaponData = (WeaponData) equipment;

            var bone = _boneListenerManager.bones[BoneListenerManager.CharacterBone.RIGHT_HAND_WEAPON];
            
            return Object.Instantiate(weaponData.view, bone.transform);
        }

        public void Unequip(GameObject equipmentView)
        {
            Object.Destroy(equipmentView);
        }
    }
}