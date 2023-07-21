using System.Collections.Generic;
using game.core.InputSystem;
using game.core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Common {
	public class AICharacterControl
	{
		private const float THRESHOLD = 1f;
        
		private IControlable _controlable;

		private List<Vector3> _currentPath;
        
		private Vector3 _currentTargetPoint;

		private InputData _inputData = new ();
        
		public bool hasTarget => _currentTargetPoint != default;
        
		public void Init(IControlable controlable) {
			_controlable = controlable;
		}

		public void FollowPath(List<Vector3> path) {
			_currentPath = path;

			_currentTargetPoint = _currentPath[0];
			_currentPath.Remove(_currentTargetPoint);
		}

		public void StopFollow() {
			_currentPath = null;
			_currentTargetPoint = default;
		}

		public void Update(float deltaTime) {
			if (_currentTargetPoint == default) {
				_inputData.Reset();
				_controlable.OnDataUpdate(_inputData);
				return;
			}

			var isReached = IsPointReached(_currentTargetPoint);
			if (isReached && _currentPath.Count == 0) {
				_currentTargetPoint = default;
				return;
			}
            
			if (isReached) {
				_currentTargetPoint = TakeNextPoint();
				var direction = GetDirection(_currentTargetPoint, _controlable.currentPosition);
				_inputData.Update(direction, direction, new List<InputActionField<InputAction<InputActionType>>>());
			} 

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
}