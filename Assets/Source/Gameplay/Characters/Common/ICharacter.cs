using game.core;
using game.core.common;
using game.core.InputSystem.Interfaces;
using game.core.level;
using game.Gameplay.Characters.Common.Abilities;
using UnityEngine;

namespace game.Gameplay.Characters.Common
{
    public interface ICharacter {
        public void Init();
        public bool isPlayer { get; }
        public bool canChangeState { get; }
        public Vector3 currentPosition { get; }
        public Transform currentTransform { get; }
        // TODO: Получение статов персонажа или одного стата вместо просто дамаги но что имеем то имеем пока или хз
        HealthChange<DamageType> GetDamage();
        public IControlable controlable { get; }
        public AIBehaviour behaviour { get; }
        public AbilitySystem abilitySystem { get; }
        public CharacterAnimation animator { get; }
        public Healthable healthable { get; }
        public INavigator navigator { get; }
    }
}