namespace game.Source.Gameplay.Characters
{
    public class BaseStateMachine
    {
        public CharacterState currentState;

        public void ChangeState(CharacterState state)
        {
            if (currentState == state)
            {
                return;
            }
            
            currentState?.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
}