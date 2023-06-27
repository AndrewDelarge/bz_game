using game.core;
using game.core.storage.Data.AI;
using UnityEngine;

namespace game.Gameplay.DataStorage.AI
{
    [CreateAssetMenu(menuName = "GameData/AIBehaviour/TestBehaviour", fileName = "TestBehaviour", order = 0)]
    public class TestBehaviour : BehaviourData
    {
        public override AIBehaviour GetBehaviour() => new Characters.AI.Behaviour.TestBehaviour();
    }
}