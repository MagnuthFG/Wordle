using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;
using Wordl.Interface;

namespace Wordl.Keyboard
{
    public class UIKeyboard : UIElement, IButtonTarget
    {
        [Header("Keyboard")]
        [SF] private Vector3 _centre  = Vector3.zero;
        [SF] private Vector3 _spacing = Vector3.zero;
        [Space]
        [SF] private Vector3 _widthHeightDepth = Vector3.one;
        [SF] private GameObject _prefab = null;

        private readonly Dictionary<string, UIKey> _keys = new(){
            { "Q", null }, { "W", null }, { "E", null }, { "R", null }, 
            { "T", null }, { "Y", null }, { "U", null }, { "I", null }, 
            { "O", null }, { "P", null }, { "A", null }, { "S", null }, 
            { "D", null }, { "F", null }, { "G", null }, { "H", null }, 
            { "J", null }, { "K", null }, { "L", null }, { "Z", null }, 
            { "X", null }, { "C", null }, { "V", null }, { "B", null }, 
            { "N", null }, { "M", null },
        };

// INITIALISATION

        private void Start() => BuildKeyboard();

// INPUT

        // CHECK KEYBOARD INPUT

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
                    row   += 1;
                    count -= row;
                    half   = count * 0.5f;
                    column = 0;
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

            Debug.Log($"Clicked on {input}");
        }
    }
}