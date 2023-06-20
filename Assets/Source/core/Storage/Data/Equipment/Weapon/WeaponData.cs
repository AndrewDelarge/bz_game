using System;
using System.Collections.Generic;
using game.Gameplay;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.core.storage.Data.Equipment.Weapon
{
    public abstract class WeaponData : EquipmentData
    {
        [Header("Timings")] 
        [SerializeField] private float _shotTime = .65f;
        [SerializeField] private float _reloadTime = 2f;
        [SerializeField] private int _magazineCapacity = 2;
        [SerializeField] private float _damage = 100;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private ProjectileData _defaultProjectile;

        [SerializeField] private Vector2 _spreadX;
        [SerializeField] private Vector2 _spreadY;
        [SerializeField] private int _projectilesForShot;
        
        [SerializeReference] private EquipmentViewBase _viewTemplate;
        [SerializeField] private List<WeaponFxData> _fx;
        public EquipmentViewBase viewTemplate => _viewTemplate;
        public float shotTime => _shotTime;
        public float reloadTime => _reloadTime;
        public int magazineCapacity => _magazineCapacity;
        public IReadOnlyList<WeaponFxData> fx => _fx;
        
        public float damage => _damage;
        public DamageType damageType => _damageType;
        public ProjectileData defaultProjectile => _defaultProjectile;
        public Vector2 spreadX => _spreadX;
        public Vector2 spreadY => _spreadY;
        public int projectilesForShot => _projectilesForShot;
        
        [Serializable]
        public class WeaponFxData {
            [SerializeField] private string _name;
            [SerializeField] private GameObject _prefab;

            public string name => _name;
            public GameObject prefab => _prefab;
        }
    }
}