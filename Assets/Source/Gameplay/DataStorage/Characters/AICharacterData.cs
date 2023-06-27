using game.core.storage.Data.AI;
using UnityEngine;

namespace game.core.Storage.Data.Character
{
    [CreateAssetMenu(menuName = "GameData/Character/AICharacterData", fileName = "AICharacterCommonData", order = 0)]
    public class AICharacterData : CharacterCommonData
    {
        [SerializeField] private BehaviourData _behaviourData;

        public BehaviourData behaviourData => _behaviourData;
    }
}