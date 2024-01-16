using game.core.storage.Data.Models;
using game.Gameplay.Characters.Common;

namespace game.Gameplay.Weapon
{
    public class WeaponStateContext
    {
        public readonly WeaponStateMachine stateMachine;
        public readonly IWeaponView view;
        public readonly WeaponModel data;
        public readonly ICharacter owner;
        public readonly ProjectileController projectileController;
        
        public WeaponStateContext(WeaponStateMachine stateMachine, EquipmentViewBase view, EquipmentModel data, ICharacter owner, ProjectileController projectileController)
        {
            this.stateMachine = stateMachine;
            this.view = (IWeaponView) view;
            this.data = (WeaponModel) data;
            this.owner = owner;
            this.projectileController = projectileController;
        }
    }
}