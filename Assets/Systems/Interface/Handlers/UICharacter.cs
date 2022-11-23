using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface
{
    [AddComponentMenu("Magnuth/Interface/UI Character")]
    public class UICharacter : UIObject
    {
        [SF] private string _character = "";
        [SF] private FontSetup _font = null;
        
        private const string RECT_PARAM = "_Rect";

// PROPERTIES

        public char Character {
            get => _character.Length > 0 ? _character [0] : '\0';
        }

// INITIALISATION

        /// <summary>
        /// Initialises: material property
        /// </summary>
        protected virtual void Start(){
            if (_character.Length == 0) return;
            SetCharater(_character[0]);
        }

// SETTINGS

        /// <summary>
        /// Specifies: displayed character
        /// </summary>
        public void SetCharater(char input){
            _material.SetVector(
                RECT_PARAM, 
                _font.GetVector(input)
            );

            _character = string.Concat(
                string.Empty, input
            );
        }

// INSPECTOR

        /// <summary>
        /// Updates: material properties
        /// </summary>
        protected override void OnInspectorUpdate(){
            base.OnInspectorUpdate();

            if (_character.Length > 1){
                _character = _character.Substring(0, 1);
            }

            // Doesn't work because Unity wont allow
            // material instancing in editor mode
            //_material = GetComponent<MeshRenderer>().material;

            //if (!string.IsNullOrEmpty(_character)){
            //    if (_character.Length > 0 && _character[0] != _char){
            //        SetCharater(_character);
            //    }
            //}
        }
    }
}