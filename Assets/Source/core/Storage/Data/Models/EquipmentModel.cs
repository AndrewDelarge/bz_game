using game.core.storage.Data.Equipment;
using game.Gameplay;

namespace game.core.storage.Data.Models
{
    public abstract class EquipmentModel
    {
        protected EquipmentModel(EquipmentData data) {}

        public abstract HealthChange<DamageType> GetDamage();
    }
}