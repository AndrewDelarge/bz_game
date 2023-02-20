using System;
using System.Collections.Generic;
using System.Linq;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.Source;
using UnityEngine;

namespace game.gameplay.control
{
    public class InputListener : MonoBehaviour, IInputRawListener
    {
        public const string AXIS_WASD = "wasd";
        public const string AXIS_MOUSE = "mouse";
        
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
            
            GCore.Get<IInputManager>().RegisterInput(dataProvider);
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
        }
    }

    public interface IInputRawListener
    {
        public event Action<InputRawData> updated;
    }

    public class KeyboardInputDataProvider : IInputable
    {
        public event Action<InputData> updated;
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;
        public event Action<Vector3> inputVector;
        
        private IInputRawListener _inputListener;
        private InputData _inputData = new InputData();
        private InputRawData _rawData;
        
        private List<InputActionField<InputAction>> _actionPool = new List<InputActionField<InputAction>>();
        private List<InputActionField<InputAction>> _filledActions = new List<InputActionField<InputAction>>();

        private Dictionary<KeyCode, InputActionType> _actionDictionary = new Dictionary<KeyCode, InputActionType>()
        {
            {KeyCode.LeftShift, InputActionType.SPRINT},
            {KeyCode.F, InputActionType.KICK},
        };

        public void Init(IInputRawListener listener)
        {
            _inputListener = listener;
            _inputListener.updated += OnUpdateRawData;
            
            foreach (var action in _actionDictionary) {
                var inputAction = new InputAction(action.Value, InputStatus.NONE);
                _actionPool.Add(new InputActionField<InputAction>(inputAction));
            }
        }

        public InputRawData GetRaw()
        {
            return _rawData;
        }
        
        private void OnUpdateRawData(InputRawData raw)
        {
            _rawData = raw;
            _filledActions.Clear();
            
            foreach (var button in _rawData.buttons)
            {
                if (_actionDictionary.TryGetValue(button.code, out var type) == false)
                {
                    continue;
                }

                var action = _actionPool.First();
                _actionPool.Remove(action);
                action.Update(new InputAction(type, button.status));

                _filledActions.Add(action);
            }
            
            _inputData.Update(_rawData.axes[InputListener.AXIS_WASD], _rawData.axes[InputListener.AXIS_MOUSE], _filledActions);
            
            updated?.Invoke(_inputData);

            foreach (var filledAction in _filledActions) {
                filledAction.isAbsorbed = false;
                _actionPool.Add(filledAction);
            }
        }

    }
}