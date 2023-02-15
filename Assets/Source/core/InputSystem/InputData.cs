using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace game.core.InputSystem
{
    public class InputData
    {
        public InputActionField<Vector2> move { get; private set; }
        public InputActionField<Vector2> aim { get; private set;  }
        public List<InputActionField<InputAction>> actions { get; private set; }

        public InputData()
        {
            move = new InputActionField<Vector2>(Vector2.zero);
            aim = new InputActionField<Vector2>(Vector2.zero);
        }

        public void Update(Vector2 move, Vector2 aim, List<InputActionField<InputAction>> actions)
        {
            this.move.Update(move);
            this.aim.Update(aim);
            this.actions = actions;
        }

        public InputActionField<InputAction> GetAction(InputActionType action)
        {
            foreach (var inputAction in actions)
                if (inputAction.value.type == action)
                    return inputAction;

            return null;
        }
    }

    public class InputRawData
    {
        public Dictionary<string, Vector2> axes = new Dictionary<string, Vector2>();
        
        public List<InputRawButton> buttons = new List<InputRawButton>();

        public void Reset()
        {
            axes.Clear();
            buttons.Clear();
        }
    }

    public class InputRawButton
    {
        public KeyCode code { get; }
        public InputStatus status { get; private set; }

        public InputRawButton(KeyCode code, InputStatus status)
        {
            this.code = code;
            this.status = status;
        }

        public void SetStatus(InputStatus status)
        {
            this.status = status;
        }
    }
    
    public class InputActionField<T>
    {
        public T value { get; set; }
        public bool isAbsorbed { get; set; }

        public InputActionField(T value)
        {
            this.value = value;
        }

        public void Update(T value)
        {
            this.value = value;
            isAbsorbed = false;
        }
    }

    public struct InputAction
    {
        public InputActionType type { get; }
        public InputStatus status { get; private set; }

        public InputAction(InputActionType type, InputStatus status)
        {
            this.type = type;
            this.status = status;
        }

        public void Update(InputStatus status) {
            this.status = status;
        }
    }
    
    public enum InputStatus
    {
        NONE = -1,
        DOWN = 1,
        PRESSED = 2,
        UP = 3
    }
    
    public enum InputActionType
    {
        INTERACT = 0,
        KICK = 1,
        AIM = 2, 
        ACTION = 3,
        SPRINT = 4
    }
}