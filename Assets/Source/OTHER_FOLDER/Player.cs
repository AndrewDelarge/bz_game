using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
    public class Player : MonoBehaviour
    {
        public AICharacter near { get; internal set; }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && near != null)
            {
                near.OnPressE(this);
            }
        }
    }
}
