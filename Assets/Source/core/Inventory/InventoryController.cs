using System.Collections.Generic;
using game.core.Inventory.Common;

namespace game.core.Inventory
{
    public class InventoryController : IInventory {
        public IEnumerable<IInventoryItem> _items { get; }

        public void Add(IInventoryItem item)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(IInventoryItem item)
        {
            throw new System.NotImplementedException();
        }
    }
}