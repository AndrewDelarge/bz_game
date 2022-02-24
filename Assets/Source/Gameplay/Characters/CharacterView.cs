using game.gameplay.characters;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterAnimation _animation;
        [SerializeField] private Camera _camera;

        public ICharacterMovement movement => _movement;
        public ICharacterAnimation animation => _animation;
        public Camera camera => _camera;
    }
}