using System.Collections.Generic;
using game.core.common;
using UnityEngine;
using UnityEngine.AI;
using ILogger = game.core.Common.ILogger;

namespace game.core.level {
	public class Navigator : INavigator {
		private NavMeshPath _cachedRawPath;
		private List<Vector3> _cachedPath = new List<Vector3>();
		private ILogger _logger;
		public void Init()
		{
			_cachedRawPath = new NavMeshPath();
			_logger = AppCore.Get<ILogger>();
		}
        
		public List<Vector3> GetPath(Vector3 start, Vector3 target)
		{
			_cachedPath = new List<Vector3>();
			if (CalculatePath(start, target) == false) {
				_logger.Log("[Navigator] : Error pathfinding");
				return null;
			}
            
			_logger.Log($"[Navigator] : Path was found #[{_cachedRawPath.status.ToString()}]#");

			for (int i = 0; i < _cachedRawPath.corners.Length - 1; i++)
			{
				Debug.DrawLine(_cachedRawPath.corners[i], _cachedRawPath.corners[i + 1], Color.red, 999f);
			}
            
			foreach (var corner in _cachedRawPath.corners)
			{
				_cachedPath.Add(corner);
			}

			return _cachedPath;
		}

		private bool CalculatePath(Vector3 start, Vector3 target)
		{
			return NavMesh.CalculatePath(start, target, 1, _cachedRawPath);
		}
	}
}