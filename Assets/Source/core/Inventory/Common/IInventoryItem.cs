namespace game.core.Inventory.Common
{
    public interface IInventoryItem
    {
        string title { get; }
        string description { get; }
        float weight { get; }
    }
}