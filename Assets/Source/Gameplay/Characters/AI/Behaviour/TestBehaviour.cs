using System;
using System.Collections.Generic;
using System.Linq;
using game.core;
using game.core.InputSystem;
using game.core.InputSystem.Interfaces;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour
{
    public class AICharacterControl
    {
        private const float THRESHOLD = 1f;
        
        private IControlable _controlable;

        private List<Vector3> _currentPath;
        
        private Vector3 _currentTargetPoint;

        private InputData _inputData = new InputData();
        
        private float _lastDistance;
        public bool hasTarget => _currentTargetPoint != default;
        
        public void Init(IControlable controlable) {
            _controlable = controlable;
        }

        public void FollowPath(List<Vector3> path) {
            _currentPath = path;

            _currentTargetPoint = _currentPath[0];
            _currentPath.Remove(_currentTargetPoint);
        }

        public void Update(float deltaTime) {
            if (_currentTargetPoint == default) {
                return;
            }

            var isReached = IsPointReached(_currentTargetPoint);
            if (isReached && _currentPath.Count == 0) {
                _currentTargetPoint = default;
                return;
            }
            
            if (isReached) {
                _currentTargetPoint = TakeNextPoint();
            }

            var currentDistance = Vector3.Distance(_currentTargetPoint, _controlable.currentPosition);
            
            if (isReached || currentDistance > _lastDistance)
            {
                var direction = GetDirection(_currentTargetPoint, _controlable.currentPosition);
                _inputData.Update(direction, direction, new List<InputActionField<InputAction>>());
            }

            _lastDistance = currentDistance;
            
            _controlable.OnDataUpdate(_inputData);
        }
        
        private bool IsPointReached(Vector3 point) {
            return Vector3.Distance(point, _controlable.currentPosition) <= THRESHOLD;
        }

        private Vector3 TakeNextPoint() {
            var point = _currentPath[0];
            _currentPath.Remove(point);
            return point;
        }

        private Vector2 GetDirection(Vector3 vectorA, Vector3 vectorB) {
            return new Vector2(vectorA.x - vectorB.x, vectorA.z - vectorB.z)  ;
        }
    }
    public class TestBehaviour : AIBehaviour {
        private ICharacter _character;
        private const float UPDATE_TICK = 2f;
        private INavigator _navigator;
        private AICharacterControl _control = new AICharacterControl();

        private float _lastUpdate;
        
        public TestBehaviour() {
            _navigator = AppCore.Get<LevelManager>().navigator;
        }

        public override void Init(ICharacter character) {
            _character = character;

            _control.Init(character.controlable);
        }

        public override void Update(float deltaTime) {
            _control.Update(deltaTime);
            
            if (_lastUpdate > 0) {
                _lastUpdate -= deltaTime;
                return;
            }

            if (_control.hasTarget) {
                return;
            }
            
            _lastUpdate = UPDATE_TICK;

            var manager = AppCore.Get<CharactersManager>();

            var path = _navigator.GetPath(_character.currentPosition, manager.player.currentPosition);
            
            if (path == null) {
                return;
            }
            
            _control.FollowPath(path);
        }
    }
}