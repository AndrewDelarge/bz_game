using UnityEngine;

namespace game.Gameplay
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _markerObject;
        public new string name => _name;
        public GameObject markerObject => _markerObject;
        
        private void Start()
        {
            if (_markerObject == null) {
                _markerObject = gameObject;
            }
        }
    }
}