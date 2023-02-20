﻿using System;
using UnityEngine;

namespace game.Gameplay.PhysicsObjects
{
    public class PhysicsSleepable : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        private void Awake()
        {
            _rigidbody.Sleep();
        }
    }
}