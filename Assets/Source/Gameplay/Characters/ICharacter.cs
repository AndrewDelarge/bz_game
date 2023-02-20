namespace game.Gameplay.Characters
{
    public interface ICharacter {

        public void Init();
        public bool isPlayer { get; }
    }
}