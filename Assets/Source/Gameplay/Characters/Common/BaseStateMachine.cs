namespace game.Source.Gameplay.Characters.Common
{
    public class BaseStateMachine
    {
        public BaseState currentState;

        public void ChangeState(BaseState state)
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