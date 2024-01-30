using System.Collections.Generic;
using UnityEngine;

namespace game.core.common {
	public interface INavigator {
		List<Vector3> GetPath(Vector3 start, Vector3 target);
	}
}