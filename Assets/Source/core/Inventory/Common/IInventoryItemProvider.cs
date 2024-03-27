using game.core.Inventory.Common;

namespace game.core.Inventory.Common
{
    public interface IInventoryItemProvider
    {
        IInventoryItem item { get; }
    }
}