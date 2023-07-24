using System.Collections.Generic;
using game.core;
using game.core.InputSystem;
using game.Gameplay.Characters.AI.Common;
using game.Gameplay.Characters.Common;
using game.Gameplay.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour
{
    public enum BehaviourState
    {
        IDLE = 0,
        TARGET_SEARCHING = 1,
        TARGET_FOLLOW = 2,
        ATTACK = 3,
        ABILITY = 4,
    }

    public abstract class BaseBehaviourState : IBaseState<BehaviourState, BehaviourContext>
    {
        public abstract BehaviourState type { get; }
        public virtual bool CheckEnterCondition() => true;
        public virtual bool CheckExitCondition() => true;
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void HandleState(float deltaTime) { }
        public virtual void HandleInput(InputData data) { }
        public virtual void Init(BehaviourContext context) { }
    }

    public class ChasingTargetBehaviourState : BaseBehaviourState {
        private const float DISAGRO_DISTANCE = 10f;
        private const float LAST_TARGET_POS_DELTA = 2f;
        private const float UPDATE_TICK = 0.5f;

        private BehaviourContext _context;
        private INavigator _navigator;
        private AICharacterControl _control = new ();
        private float _lastUpdate;
        private Vector3 _lastTargetPosition;

        public override BehaviourState type => BehaviourState.TARGET_FOLLOW;

        public override void Init(BehaviourContext context)
        {
            _context = context;
            _navigator = AppCore.Get<LevelManager>().navigator;
            _control.Init(_context.character.controlable);
        }

        public override void HandleState(float deltaTime)
        {
            _control.Update(deltaTime);

            if (_context.target == null || IsTargetToFar()) {
                _control.StopFollow();
                _context.stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
                return;
            }

            if (_lastUpdate > 0) {
                _lastUpdate -= deltaTime;
                return;
            }
            
            if (IsPathActual() == false) {
                _control.StopFollow();
            }

            if (_control.hasTarget) {
                return;
            }

            _lastUpdate = UPDATE_TICK;

            var path = _navigator.GetPath(_context.character.currentPosition, _context.target.currentPosition);
            
            if (path == null) {
                return;
            }

            _lastTargetPosition = _context.target.currentPosition;
            _control.FollowPath(path);
        }


        private bool IsTargetToFar() => Vector3.Distance(_context.character.currentPosition, _context.target.currentPosition) > DISAGRO_DISTANCE;

        private bool IsPathActual() => Vector3.Distance(_lastTargetPosition, _context.target.currentPosition) > LAST_TARGET_POS_DELTA;
    }
    
    public class SearchTargetBehaviourState : BaseBehaviourState
    {
        private const int AGRODISTANCE = 10;
        private BehaviourContext _context;
        private CharactersManager _charactersManager;
        private ICharacter _player;

        public override BehaviourState type => BehaviourState.TARGET_SEARCHING;

        public override void Init(BehaviourContext context)
        {
            _context = context;
            _charactersManager = AppCore.Get<CharactersManager>();
            _player = _charactersManager.player;
        }

        public override void HandleState(float deltaTime)
        {
            if (Vector3.Distance(_player.currentPosition, _context.character.currentPosition) > AGRODISTANCE) {
                return;
            }
            
            _context.target = _charactersManager.player;

            _context.stateMachine.ChangeState(BehaviourState.TARGET_FOLLOW);
        }
    }
    
    public class TestBehaviour : AIBehaviour {
        private List<BaseBehaviourState> _baseStates = new ()
        {
             new IdleBehaviourState(),
             new SearchTargetBehaviourState(),
             new ChasingTargetBehaviourState(),
        };
        
        public BaseStateMachineWithStack<BehaviourState, BaseBehaviourState, BehaviourContext> stateMachine = new ();

        public override void Init(ICharacter character) {
            var context = new BehaviourContext();
            context.character = character;
            context.stateMachine = stateMachine;
            
            stateMachine.Init(_baseStates, context);
            
            stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
        }

        public override void Update(float deltaTime) {
            stateMachine.HandleState(deltaTime);
        }
    }

    
    public class IdleBehavior : AIBehaviour {
        private BaseBehaviourState _state = new IdleBehaviourState();
        public override void Init(ICharacter character) { }

        public override void Update(float deltaTime) {
            _state.HandleState(deltaTime);
        }
    }

    public class BehaviourContext
    {
        public ICharacter target;
        public ICharacter character;
        public BaseStateMachineWithStack<BehaviourState, BaseBehaviourState, BehaviourContext> stateMachine;
    }
}