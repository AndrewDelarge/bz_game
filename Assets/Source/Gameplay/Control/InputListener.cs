using System;
using System.Collections.Generic;
using System.Linq;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.gameplay.control
{
    public class InputListener : MonoBehaviour, IInputRawListener
    {
        public const string AXIS_WASD = "wasd";
        public const string AXIS_MOUSE = "mouse";
        
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;
        public event Action<Vector3> inputVector;

        public event Action<InputRawData> updated; 

        private KeyCode[] _trackingKeys = {
            KeyCode.W,
            KeyCode.A,
            KeyCode.D,
            KeyCode.S,
            KeyCode.F,
            KeyCode.LeftShift
        };

        private List<InputRawButton> _buttonsPool = new List<InputRawButton>();

        private InputRawData _inputRaw = new InputRawData();
        
        private void Start()
        {
            foreach (var key in _trackingKeys)
            {
                _buttonsPool.Add(new InputRawButton(key, InputStatus.NONE));
            }

            var dataProvider = new KeyboardInputDataProvider();
            dataProvider.Init(this);
            
            Core.Get<IInputManager>().RegisterInput(dataProvider);
        }

        private void Update()
        {
            _inputRaw.Reset();

            _buttonsPool.ForEach(x => x.SetStatus(InputStatus.NONE));
            
            _inputRaw.axes[AXIS_WASD] = Vector2.zero;
            
            // TODO [Implement] mouse position 
            _inputRaw.axes[AXIS_MOUSE] = Vector2.zero;
            
            
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                _inputRaw.axes[AXIS_WASD] = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                // inputVector?.Invoke(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            }
            
            
            foreach (var button in _buttonsPool)
            {
                if (Input.GetKeyDown(button.code))
                {
                    button.SetStatus(InputStatus.DOWN);
                    continue;
                }

                if (Input.GetKey(button.code))
                    button.SetStatus(InputStatus.PRESSED);

                if (Input.GetKeyUp(button.code))
                    button.SetStatus(InputStatus.UP);
            }
            
            foreach (var button in _buttonsPool)
            {
                if (button.status == InputStatus.NONE) 
                    continue;
                
                _inputRaw.buttons.Add(button);
            }
            
            updated?.Invoke(_inputRaw);
            //
            // for (int i = 0; i < _trackingKeys.Length; i++)
            // {
            //     if (Input.GetKeyDown(_trackingKeys[i]))
            //     {
            //         _inputRaw.buttons.Add(new InputRawButton(_trackingKeys[i], InputStatus.DOWN));
            //         // keyDown?.Invoke(_trackingKeys[i]);
            //     }
            //
            //     if (Input.GetKey(_trackingKeys[i]))
            //     {
            //         _inputRaw.buttons.Add(new InputRawButton(_trackingKeys[i], InputStatus.PRESSED));
            //     }
            //     
            //     if (Input.GetKeyUp(_trackingKeys[i]))
            //     {
            //         _inputRaw.buttons.Add(new InputRawButton(_trackingKeys[i], InputStatus.UP));
            //         // keyUp?.Invoke(_trackingKeys[i]);
            //     }
            // }
            
        }
    }

    public interface IInputRawListener
    {
        public event Action<InputRawData> updated;
    }

    public class KeyboardInputDataProvider : IInputable
    {
        private IInputRawListener _inputListener;

        private Dictionary<KeyCode, InputActionType> _actionDictionary = new Dictionary<KeyCode, InputActionType>()
        {
            {KeyCode.LeftShift, InputActionType.SPRINT},
            {KeyCode.F, InputActionType.KICK},
        };

        private InputData _inputData = new InputData();
        private InputRawData _rawData;
        
        private List<InputActionField<InputAction>> _actionPool = new List<InputActionField<InputAction>>();

        public event Action<InputData> updated;
        
        public void Init(IInputRawListener listener)
        {
            _inputListener = listener;
            _inputListener.updated += OnUpdateRawData;
            
            foreach (var action in _actionDictionary)
            {
                _actionPool.Add(new InputActionField<InputAction>(new InputAction(action.Value, InputStatus.NONE)));
            }
        }

        public InputRawData GetRaw()
        {
            return _rawData;
        }
        
        private void OnUpdateRawData(InputRawData raw)
        {
            _rawData = raw;
            var actions = new List<InputActionField<InputAction>>();
            
            // TODO [Refactor] some pool or something
            foreach (var button in _rawData.buttons)
            {
                if (_actionDictionary.TryGetValue(button.code, out var type) == false)
                {
                    continue;
                }

                var action = new InputActionField<InputAction>(new InputAction(type, button.status));

                actions.Add(action);
            }
            
            
            _inputData.Update(_rawData.axes[InputListener.AXIS_WASD], _rawData.axes[InputListener.AXIS_MOUSE], actions);
            
            updated?.Invoke(_inputData);
        }

        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;
        public event Action<Vector3> inputVector;
    }
}