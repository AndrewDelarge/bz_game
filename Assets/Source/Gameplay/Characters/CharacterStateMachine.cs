namespace game.Source.Gameplay.Characters
{
    public class CharacterStateMachine
    {
        public CharacterState currentState;

        public void ChangeState(CharacterState state)
        {
            currentState?.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
}