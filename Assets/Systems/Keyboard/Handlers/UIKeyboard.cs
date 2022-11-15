using SF = UnityEngine.SerializeField;
using Action = System.Action<string>;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

namespace Magnuth.Interface
{
    [AddComponentMenu("Magnuth/Interface/UI Keyboard")]
    public class UIKeyboard : UIElement, IButtonTarget
    {
        [Header("Keyboard")]
        [SF] private Vector3 _centre  = Vector3.zero;
        [SF] private Vector3 _spacing = Vector3.zero;
        [Space]
        [SF] private Vector3 _widthHeightDepth = Vector3.one;
        [SF] private GameObject _prefab = null;

        private Queue<UIKey> _pressed = new();
        private System.IDisposable _listener = null;
        private Subscription<Action> _subscribers = new();

        private Dictionary<string, UIKey> _keys = new(){
            { "Q", null }, { "W", null }, { "E", null }, { "R", null }, 
            { "T", null }, { "Y", null }, { "U", null }, { "I", null }, 
            { "O", null }, { "P", null }, { "A", null }, { "S", null }, 
            { "D", null }, { "F", null }, { "G", null }, { "H", null }, 
            { "J", null }, { "K", null }, { "L", null }, { "+", null },
            { "Z", null }, { "X", null }, { "C", null }, { "V", null }, 
            { "B", null }, { "N", null }, { "M", null }, { "-", null },
        };

// INITIALISATION

        /// <summary>
        /// Initialises: input callback
        /// </summary>
        private void OnEnable(){
            _listener = InputSystem.onAnyButtonPress.Call(
                ctrl => OnAnyKeyInput(ctrl.name.ToUpper())
            );
        }

        /// <summary>
        /// Deinitialises: input callback
        /// </summary>
        private void OnDisable(){
            _listener.Dispose();
        }

        /// <summary>
        /// Initialises: instantiates keys
        /// </summary>
        private void Start() => BuildKeyboard();

// INPUT CALLBACK

        /// <summary>
        /// Event: on input pressed
        /// </summary>
        private void OnAnyKeyInput(string input){
            input = ProcessInput(input);
            if (!_keys.ContainsKey(input)) 
                return;

            var key = _keys[input];
            key.OnPointerDown(null);

            _pressed.Enqueue(key);
            Invoke("OnReleaseKey", 0.1f);
        }

        /// <summary>
        /// Event: on input invoke
        /// </summary>
        private void OnReleaseKey(){
            if (_pressed.Count == 0) return;
            var key = _pressed.Dequeue();
            key.OnPointerUp(null);
        }

        /// <summary>
        /// Returns: matches user input to keyboard keys
        /// </summary>
        private string ProcessInput(string input){
            switch (input){
                case "ENTER"      : return "+";
                case "NUMPADPLUS" : return "+";
                case "BACKSPACE"  : return "-";
                case "NUMPADMINUS": return "-";
                default: return input;
            }
        }

// INTERFACE

        /// <summary>
        /// Instantiates: keyboard keys
        /// </summary>
        public void BuildKeyboard(){
            var scaler = GetChildScaler();

            int count  = 10;
            int column = 0;
            int row    = 0;

            var half  = count * 0.5f;
            var xSize = (_widthHeightDepth.x + _spacing.x);
            var ySize = (_widthHeightDepth.y + _spacing.y);
            var point = _centre;

            var keys = new List<string>(_keys.Keys);

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
                ui.Initialise(point, _widthHeightDepth, scaler, _resScaler);

                if (++column >= count){
                    column = 0;
                    row   += 1;
                    count  = 9;
                    half   = count * 0.5f;
                }
            }
        }

        /// <summary>
        /// Event: on clicked key button
        /// </summary>
        public void OnClicked(params object[] args){
            if (args == null || args.Length == 0) return;
            
            var input = (string)args[0];
            if (string.IsNullOrEmpty(input)) return;

            _subscribers.NotifySubscribers(input);
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