namespace game.Gameplay.Characters.Common
{
    public interface ICharacter {

        public void Init();
        public bool isPlayer { get; }
    }
}