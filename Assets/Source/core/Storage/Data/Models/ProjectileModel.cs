using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay.Weapon;

namespace game.core.storage.Data.Models
{
    public class ProjectileModel {
        protected ProjectileView _viewTemplate;
        protected float _speed;

        public float speed => _speed;
        public ProjectileView viewTemplate => _viewTemplate;
        public ProjectileModel(ProjectileData data)
        {
            _speed = data.speed;
            _viewTemplate = data.view;
        }
    }
}