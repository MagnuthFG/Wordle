using SF = UnityEngine.SerializeField;
using Action = System.Action<char>;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Magnuth.Interface
{
    [AddComponentMenu("Magnuth/Interface/UI Keyboard")]
    public class UIKeyboard : UIElement, IButtonTarget
    {
        [Header("Keyboard Input")]
        [SF] private InputActionReference _submitInput = null;
        [SF] private InputActionReference _removeInput = null;
        [SF] private InputActionReference _keysInput   = null;

        [Header("Keyboard Grid")]
        [SF] private Vector3 _centre  = Vector3.zero;
        [SF] private Vector3 _spacing = Vector3.zero;
        [Space]
        [SF] private Vector3 _widthHeightDepth = Vector3.one;
        [SF] private GameObject _prefab = null;

        private Queue<UIKey>         _pressed     = new(7);
        private Subscription<Action> _subscribers = new();

        private const float  RELEASE_DELAY = 0.1f;
        private const string RELEASE_PARAM = "OnReleaseKey";

        private readonly Dictionary<char, UIKey> _keys = new(){
            { 'q', null }, { 'w', null }, { 'e', null }, { 'r', null  }, 
            { 't', null }, { 'y', null }, { 'u', null }, { 'i', null  }, 
            { 'o', null }, { 'p', null }, { 'a', null }, { 's', null  }, 
            { 'd', null }, { 'f', null }, { 'g', null }, { 'h', null  }, 
            { 'j', null }, { 'k', null }, { 'l', null }, { '\b', null },
            { 'z', null }, { 'x', null }, { 'c', null }, { 'v', null  }, 
            { 'b', null }, { 'n', null }, { 'm', null }, { '\n', null },
        };

// INITIALISATION

        /// <summary>
        /// Initialises the keys
        /// </summary>
        private void Start() => BuildKeyboard();

        /// <summary>
        /// Initialises the input callback
        /// </summary>
        private void OnEnable(){
            _submitInput.action.canceled += ctx => OnSubmitInput(ctx);
            _removeInput.action.canceled += ctx => OnRemoveInput(ctx);
            _keysInput.action.canceled   += ctx => OnKeysInput(ctx);

            _submitInput.action.Enable();
            _removeInput.action.Enable();
            _keysInput.action.Enable();
        }

        /// <summary>
        /// Deinitialises the input callback
        /// </summary>
        private void OnDisable(){
            _removeInput.action.canceled -= OnSubmitInput;
            _removeInput.action.canceled -= OnRemoveInput;
            _keysInput.action.canceled   -= OnKeysInput;

            _submitInput.action.Disable();
            _removeInput.action.Disable();
            _keysInput.action.Disable();
        }

// INTERFACE

        /// <summary>
        /// Instantiates the keyboard keys
        /// </summary>
        public void BuildKeyboard(){
            var origin = _transform.localPosition;
            var scaler = GetScaler();

            int count  = 10;
            var half   = 5f;
            int column = 0;
            int row    = 0;

            var keys  = new List<char>(_keys.Keys);
            var xSize = (_widthHeightDepth.x + _spacing.x);
            var ySize = (_widthHeightDepth.y + _spacing.y);
            var point = _centre;
   
            for (int i = 0; i < keys.Count; i++){
                point.x = _centre.x - (xSize * half) + (xSize * column);
                point.y = _centre.y - (ySize * row);

                var obj = Instantiate(_prefab, _transform);
                var ui  = obj.GetComponent<UIKey>();
                if (ui == null) continue;

                var key = keys[i];
                _keys[key] = ui;

                ui.SetKey(key);
                ui.SetTarget(this.gameObject);

                ui.Initialise(
                    point, origin, 
                    _widthHeightDepth, scaler
                );

                if (++column >= count){
                    column = 0;
                    row   += 1;
                    count  = 9;
                    half   = 4.5f;
                }
            }
        }

        /// <summary>
        /// On clicked key button callback
        /// </summary>
        public void OnClicked(params object[] args){
            if (args?.Length == 0) return;
            
            var input = (char)args[0];
            if (input == '\0') return;

            _subscribers.NotifySubscribers(input);
        }

// INPUT CALLBACK

        /// <summary>
        /// On submit word input callback
        /// </summary>
        private void OnSubmitInput(InputAction.CallbackContext ctx){
            ActivateKey('\n');
        }

        /// <summary>
        /// On remove character input callback
        /// </summary>
        private void OnRemoveInput(InputAction.CallbackContext ctx){
            ActivateKey('\b');
        }

        /// <summary>
        /// On keyboard key input callback
        /// </summary>
        private void OnKeysInput(InputAction.CallbackContext ctx){
            var key = ctx.control.name[0];
            ActivateKey(key);
        }


        /// <summary>
        /// Triggers the keyboard key from input
        /// </summary>
        private void ActivateKey(char character){
            if (!_keys.ContainsKey(character)) return;

            var key = _keys[character];
            key.OnPointerDown(null);

            _pressed.Enqueue(key);
            Invoke(RELEASE_PARAM, RELEASE_DELAY);
        }

        /// <summary>
        /// On input invoke event
        /// </summary>
        private void OnReleaseKey(){
            if (_pressed.Count == 0) return;
            var key = _pressed.Dequeue();
            key.OnPointerUp(null);
        }

// SUBSCRIPTIONS

        /// <summary>
        /// Subscribes to keyboard callback
        /// <br>Outputs: keyboard letter</brA>
        /// </summary>
        public void Subscribe(Action subscriber) 
            => _subscribers.AddSubscriber(subscriber);

        /// <summary>
        /// Unsubscribes from keyboard callback
        /// </summary>
        public void Unsubscribe(Action subscriber) 
            => _subscribers.RemoveSubscriber(subscriber);
    }
}