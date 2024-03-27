using System.Collections.Generic;

namespace game.core.Inventory.Common
{
    public interface IInventory
    {
        IEnumerable<IInventoryItem> _items { get; }
        void Add(IInventoryItem item);
        void Remove(IInventoryItem item);
    }
}