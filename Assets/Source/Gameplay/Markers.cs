using System;
using System.Collections.Generic;
using UnityEngine;

namespace game.Gameplay
{
    [Serializable]
    public class Markers
    {
        [SerializeField] private List<Marker> _markers;

        private Dictionary<string, Marker> _markersDict;
        
        public IReadOnlyDictionary<string, Marker> markers => _markersDict;
        
        public void Init() {
            _markersDict = new Dictionary<string, Marker>();

            foreach (var marker in _markers) {
                _markersDict.Add(marker.name, marker);
            }

            _markers = null;
        }

        public Marker GetMarker(string name) {
            if (_markersDict.ContainsKey(name)) {
                return _markersDict[name];
            }

            return null;
        }
    }
}