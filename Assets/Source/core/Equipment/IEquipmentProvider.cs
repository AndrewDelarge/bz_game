using game.core.storage.Data.Models;

namespace game.core.Equipment
{
    public interface IEquipmentProvider
    {
        IEquipment equipment { get; }
    }
}