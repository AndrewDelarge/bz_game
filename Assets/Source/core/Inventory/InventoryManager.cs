using System.Collections.Generic;
using game.core.common;
using game.core.Inventory.Common;
using game.сore.Common;

namespace game.core.Inventory {
	public class InventoryManager : ICoreManager, IInitalizeable
	{
		private List<IInventory> _inventories;
		public void Init()
		{
			_inventories = new List<IInventory>();
			
			_inventories.Add(new InventoryController());
		}
	}
}