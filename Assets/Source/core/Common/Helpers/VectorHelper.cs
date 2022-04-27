using UnityEngine;

namespace game.Source.core.Common.Helpers
{
    public class VectorHelper
    {
        public static bool IsInViewAngle(Transform current, Vector3 target, float viewAngle)
        {
            var dirToTarget = (target - current.position).normalized;

            if (Vector3.Angle(current.forward, dirToTarget) < viewAngle)
                return true;

            return false;
        }

        public static Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            var heading = to - from;
            var distance = heading.magnitude;
            return heading / distance;
        }
    }
}