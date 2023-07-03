using game.core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Gameplay.Characters.Common
{
    public interface ICharacter {
        
        public void Init();
        public bool isPlayer { get; }
        public Vector3 currentPosition { get; }
        // TODO: Получение статов персонажа или одного стата вместо просто дамаги но что имеем то имеем пока или хз
        HealthChange<DamageType> GetDamage();
        public IControlable controlable { get; }
    }
}