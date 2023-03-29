using game.core.storage.Data.Models;

namespace game.Gameplay.Weapon
{
    public class WeaponStateContext
    {
        public readonly WeaponStateMachine stateMachine;
        public readonly IWeaponView view;
        public readonly WeaponModel data;
        
        public WeaponStateContext(WeaponStateMachine stateMachine, EquipmentViewBase view, EquipmentModel data)
        {
            this.stateMachine = stateMachine;
            this.view = (IWeaponView) view;
            this.data = (WeaponModel) data;
        }
    }
}