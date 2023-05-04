using game.Gameplay.Weapon;
using UnityEngine;

namespace game.core.storage.Data.Equipment.Weapon
{
    [CreateAssetMenu(menuName = "GameData/Equipment/Projectile", fileName = "Create projectile", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] protected ProjectileView _view;
        [SerializeField] protected float _speed = 10f;

        public ProjectileView view => _view;
        public float speed => _speed;
    }
}