namespace game.Gameplay.Characters.Common
{
    public interface ICharacter {

        public void Init();
        public bool isPlayer { get; }

        // TODO: Получение статов персонажа или одного стата вместо просто дамаги но что имеем то имеем пока или хз
        HealthChange<DamageType> GetDamage();
    }
}