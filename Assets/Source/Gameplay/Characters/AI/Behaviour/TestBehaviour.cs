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
    }

    public abstract class BaseBehaviourState : IBaseState<BehaviourState, BehaviourContext>
    {
        public abstract BehaviourState type { get; }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void HandleState(float deltaTime) { }
        public virtual void HandleInput(InputData data) { }
        public virtual void Init(BehaviourContext context) { }
    }
    
    public class IdleBehaviourState : BaseBehaviourState
    {
        public override BehaviourState type => BehaviourState.IDLE;
    }

    public class ChasingTargetBehaviourState : BaseBehaviourState
    {
        private BehaviourContext _context;
        private INavigator _navigator;
        private AICharacterControl _control = new ();
        private float _lastUpdate;
        private const float UPDATE_TICK = 0.5f;

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
            
            if (_lastUpdate > 0) {
                _lastUpdate -= deltaTime;
                return;
            }

            if (_control.hasTarget) {
                _context.stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
                return;
            }
            
            _lastUpdate = UPDATE_TICK;

            var path = _navigator.GetPath(_context.character.currentPosition, _context.target);
            
            if (path == null) {
                return;
            }
            
            _control.FollowPath(path);
        }
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
            if (Vector3.Distance(_player.currentPosition, _context.character.currentPosition) > AGRODISTANCE)
            {
                return;
            }
            
            _context.target = _charactersManager.player.currentPosition;

            _context.stateMachine.ChangeState(BehaviourState.TARGET_FOLLOW);
        }
    }
    
    public class TestBehaviour : AIBehaviour {
        private ICharacter _character;
        private CharactersManager _charactersManager;
        private float _lastUpdate;

        private List<BaseBehaviourState> _baseStates = new ()
        {
             new IdleBehaviourState(),
             new SearchTargetBehaviourState(),
             new ChasingTargetBehaviourState(),
        };
        
        public BaseStateMachineWithStack<BehaviourState, BaseBehaviourState, BehaviourContext> stateMachine = new ();

        public override void Init(ICharacter character) {
            _character = character;
            
            _charactersManager = AppCore.Get<CharactersManager>();

            var context = new BehaviourContext();
            context.character = _character;
            context.stateMachine = stateMachine;
            
            
            stateMachine.Init(_baseStates, new BehaviourContext());
            
            stateMachine.ChangeState(BehaviourState.IDLE);
        }

        public override void Update(float deltaTime) {
            stateMachine.HandleState(deltaTime);
        }
    }

    public class BehaviourContext
    {
        public Vector3 target;
        public ICharacter character;
        public BaseStateMachineWithStack<BehaviourState, BaseBehaviourState, BehaviourContext> stateMachine;
    }
}