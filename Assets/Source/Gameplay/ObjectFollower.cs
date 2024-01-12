using System;
using UnityEngine;

namespace game.Gameplay
{
    public class ObjectFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private bool _keepCurrentObjectOffset;
        
        private Vector3 _defaultOffset;
        private Quaternion _defaultRotation;
        
        
        private void Awake()
        {
            _defaultOffset = transform.localPosition;
            _defaultRotation = transform.rotation;
        }


        private void LateUpdate()
        {
            transform.position = _target.position + (_keepCurrentObjectOffset ? _defaultOffset : Vector3.zero);
            var rotation = _target.rotation;
            rotation.eulerAngles += _keepCurrentObjectOffset ? _defaultRotation.eulerAngles : Quaternion.identity.eulerAngles;
            transform.rotation = rotation;
        }
    }
}