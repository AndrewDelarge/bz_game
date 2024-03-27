using game.core.Equipment;
using game.core.GameItem;
using game.core.Inventory.Common;
using game.core.storage.Data.Models;

namespace game.Gameplay.GameItem
{
    public class WeaponItem : IItem, IInventoryItemProvider, IEquipmentProvider
    {
        public int id { get; }
        public IInventoryItem item { get; }
        public IEquipment equipment { get; }
    }
}