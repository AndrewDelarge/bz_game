using game.core;
using game.core.storage.Data.AI;
using UnityEngine;

namespace game.Gameplay.DataStorage.AI {
	[CreateAssetMenu(menuName = "GameData/AIBehaviour/IdleBehaviour", fileName = "IdleBehaviour", order = 0)]
	public class IdleBehaviour : BehaviourData
	{
		public override AIBehaviour GetBehaviour() => new Characters.AI.Behaviour.IdleBehavior();
	}
}