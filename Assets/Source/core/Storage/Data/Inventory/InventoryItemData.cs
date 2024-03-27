using game.core.Inventory.Common;
using UnityEngine;

namespace game.core.storage.Data.Abilities.Inventory {
	[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "GameData/InventoryItem", order = 0)]
	public class InventoryItemData : ScriptableObject {
		[SerializeField] private string _title;
		[SerializeField] private string _description;
		[SerializeField] private float _weight;

		public string title => _title;
		public string description => _description;
		public float weight => _weight;
	}
}