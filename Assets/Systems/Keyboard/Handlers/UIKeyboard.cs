using SF = UnityEngine.SerializeField;
using Action = System.Action<char>;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using UnityEngine;

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

        private Queue<UIKey>         _pressed     = new(7);
        private Subscription<Action> _subscribers = new();
        private System.IDisposable   _listener    = null;

        private Dictionary<char, UIKey> _keys = new(){
            { 'q', null }, { 'w', null }, { 'e', null }, { 'r', null }, 
            { 't', null }, { 'y', null }, { 'u', null }, { 'i', null }, 
            { 'o', null }, { 'p', null }, { 'a', null }, { 's', null }, 
            { 'd', null }, { 'f', null }, { 'g', null }, { 'h', null }, 
            { 'j', null }, { 'k', null }, { 'l', null }, { '-', null },
            { 'z', null }, { 'x', null }, { 'c', null }, { 'v', null }, 
            { 'b', null }, { 'n', null }, { 'm', null }, { '+', null },
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
            _listener = InputSystem.onAnyButtonPress.Call(
                ctrl => OnAnyKeyInput(ctrl.name)
            );
        }

        /// <summary>
        /// Deinitialises the input callback
        /// </summary>
        private void OnDisable(){
            _listener.Dispose();
        }

// INPUT CALLBACK

        /// <summary>
        /// On input pressed callback
        /// </summary>
        private void OnAnyKeyInput(string input){
            var character = ProcessInput(input);
            if (!_keys.ContainsKey(character)) 
                return;

            var key = _keys[character];
            key.OnPointerDown(null);

            _pressed.Enqueue(key);
            Invoke("OnReleaseKey", 0.1f);
        }

        /// <summary>
        /// On input invoke event
        /// </summary>
        private void OnReleaseKey(){
            if (_pressed.Count == 0) return;
            var key = _pressed.Dequeue();
            key.OnPointerUp(null);
        }

        /// <summary>
        /// Matches user input to keyboard keys
        /// </summary>
        private char ProcessInput(string input){
            switch (input){
                case "enter"      : return '+';
                case "numpadPlus" : return '+';
                case "backspace"  : return '-';
                case "numpadMinus": return '-';
                default: return input[0];
            }
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