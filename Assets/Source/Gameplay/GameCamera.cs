using Cinemachine;
using UnityEngine;

namespace game.Gameplay {
	public class GameCamera : MonoBehaviour {
		[SerializeField] private Camera _camera;
		[SerializeField] private CinemachineVirtualCamera _cinemachine;

		public Camera camera => _camera;

		public void SetTarget(Transform transform) {
			_cinemachine.Follow = _cinemachine.LookAt = transform;
		} 
	}
}