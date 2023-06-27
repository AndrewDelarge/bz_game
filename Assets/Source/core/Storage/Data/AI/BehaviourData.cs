using UnityEngine;

namespace game.core.storage.Data.AI
{
    public abstract class BehaviourData : ScriptableObject
    {
        public abstract AIBehaviour GetBehaviour();
    }
}