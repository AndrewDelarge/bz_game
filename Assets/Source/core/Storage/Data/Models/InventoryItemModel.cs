using game.core.storage.Data.Abilities.Inventory;

namespace game.core.storage.Data.Models {
	public class InventoryItemModel {
		private InventoryItemData _data;
		
		public string title => _data.title;
		public string description => _data.description;
		public float weight => _data.weight;
		
		public InventoryItemModel(InventoryItemData data) {
			_data = data;
		}
	}
}