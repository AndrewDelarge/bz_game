using System;
using System.Collections.Generic;
using UnityEngine;

namespace game.Gameplay.Characters
{
    public class BoneListenerManager : MonoBehaviour
    {
        [SerializeField] private List<BoneListener> _followers;

        private Dictionary<CharacterBone, GameObject> _followersDict = new Dictionary<CharacterBone, GameObject>();

        public IReadOnlyDictionary<CharacterBone, GameObject> bones => _followersDict;

        
        private void Awake()
        {
            foreach (var follower in _followers)
            {
                _followersDict.Add(follower.type, follower.follower.gameObject);
            }
        }

        public enum CharacterBone
        {
            RIGHT_HAND_WEAPON = 0,
        }
        
        [Serializable]
        public class BoneListener
        {
            public CharacterBone type;
            public ObjectFollower follower;
        }
    }
    
    
}