using System;
using System.Collections.Generic;
using System.Linq;
using game.core.Common;
using game.core.InputSystem.Interfaces;
using game.core.InputSystem;
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
            KeyCode.LeftShift,
            KeyCode.R,
            KeyCode.Mouse0,
            KeyCode.Mouse1,
            KeyCode.P,
            KeyCode.Space
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
            
            AppCore.Get<IInputManager>().RegisterInput(dataProvider);
        }

        private void Update()
        {
            _inputRaw.Reset();

            _buttonsPool.ForEach(x => x.SetStatus(InputStatus.NONE));
            
            _inputRaw.axes[AXIS_WASD] = Vector2.zero;
            
            _inputRaw.axes[AXIS_MOUSE] = CalculateMousePosition();

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

        private Vector2 CalculateMousePosition() {
            var mousePosition = Input.mousePosition;
            mousePosition.x -= Screen.width / 2f;
            mousePosition.y -= Screen.height / 2f;
            return mousePosition;
        }
    }

    public interface IInputRawListener
    {
        public event Action<InputRawData> updated;
    }

    public class KeyboardInputDataProvider : IInputable
    {
        public IWhistle<InputData> updated => _updated;
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;
        public event Action<Vector3> inputVector;
        
        private IInputRawListener _inputListener;
        private InputData _inputData = new InputData();
        private InputRawData _rawData;
        private Whistle<InputData> _updated = new Whistle<InputData>();
        
        private List<InputActionField<InputAction<InputActionType>>> _actionPool = new ();
        private List<InputActionField<InputAction<InputActionType>>> _filledActions = new ();

        private Dictionary<KeyCode, InputActionType> _actionDictionary = new Dictionary<KeyCode, InputActionType>()
        {
            {KeyCode.LeftShift, InputActionType.SPRINT},
            {KeyCode.F, InputActionType.KICK},
            {KeyCode.R, InputActionType.RELOAD},
            {KeyCode.Mouse1, InputActionType.AIM},
            {KeyCode.Mouse0, InputActionType.SHOT},
            {KeyCode.P, InputActionType.DEBUG_0},
            {KeyCode.Space, InputActionType.SHOT},
        };

        public void Init(IInputRawListener listener)
        {
            _inputListener = listener;
            _inputListener.updated += OnUpdateRawData;
            
            foreach (var action in _actionDictionary) {
                var inputAction = new InputAction<InputActionType>(action.Value, InputStatus.NONE);
                _actionPool.Add(new InputActionField<InputAction<InputActionType>>(inputAction));
            }
        }
        
        public void Dispose() {
            _inputListener.updated -= OnUpdateRawData;
            _updated.Clear();
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
                action.Update(new InputAction<InputActionType>(type, button.status));

                _filledActions.Add(action);
            }
            
            _inputData.Update(_rawData.axes[InputListener.AXIS_WASD], _rawData.axes[InputListener.AXIS_MOUSE], _filledActions);
            
            _updated.Dispatch(_inputData);

            foreach (var filledAction in _filledActions) {
                filledAction.isAbsorbed = false;
                _actionPool.Add(filledAction);
            }
        }

    }
}