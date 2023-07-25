using System.Collections.Generic;
using game.core;
using game.Gameplay.Characters.Common;
using game.Gameplay.Common;

namespace game.Gameplay.Characters.AI.Behaviour
{
    public class TestBehaviour : AIBehaviour {
        private List<BaseBehaviourState> _baseStates = new ()
        {
             new IdleBehaviourState(),
             new SearchTargetBehaviourState(),
             new ChasingTargetBehaviourState(),
             new AttackBehaviourState(),
        };
        
        public BaseStateMachineWithStack<BehaviourState, BaseBehaviourState, BehaviourContext> stateMachine = new ();

        public override void Init(ICharacter character) {
            var context = new BehaviourContext {
                character = character,
                stateMachine = stateMachine
            };

            stateMachine.Init(_baseStates, context);
            
            stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
        }

        public override void Update(float deltaTime) {
            stateMachine.HandleState(deltaTime);
        }
    }
}