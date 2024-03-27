using game.core.storage.Data.Equipment;
using game.Gameplay;

namespace game.core.storage.Data.Models
{
    public interface IEquipment
    {
        void Init(EquipmentData data);

        EquipmentData data { get; }

        HealthChange<DamageType> GetDamage();
    }
}