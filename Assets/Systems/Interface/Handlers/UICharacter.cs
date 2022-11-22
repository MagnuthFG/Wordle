using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface
{
    [AddComponentMenu("Magnuth/Interface/UI Character")]
    public class UICharacter : UIObject
    {
        [SF] private string _character = "";
        [SF] private FontSetup _font = null;

// PROPERTIES

        public string Character => _character;

// INITIALISATION

        /// <summary>
        /// Initialises: material property
        /// </summary>
        private void Start() => SetCharater(_character);

// SETTINGS

        /// <summary>
        /// Specifies: displayed character
        /// </summary>
        public void SetCharater(string input){
            if (input == null) return;
            _character = input.Trim();

            if (_character.Length > 0){
                var vector = _font.GetVector(_character[0]);
                _material.SetVector("_Rect", vector);

            } else _material.SetVector("_Rect", Vector4.zero);
        }

// INSPECTOR

        /// <summary>
        /// Updates: material properties
        /// </summary>
        protected override void OnInspectorUpdate(){
            base.OnInspectorUpdate();

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