using SF = UnityEngine.SerializeField;
using Convert = System.Convert;
using UnityEngine;
using TMPro;

namespace Wordl.Interface
{
    public class UICharacter : UIObject
    {
        [SF] private string _character = " ";
        [SF] private TMP_FontAsset _font = null;

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
            if (string.IsNullOrEmpty(input)) return;

            _character = input;
            SetCharater(input[0]);
        }

        /// <summary>
        /// Specifies: displayed character
        /// </summary>
        private void SetCharater(char input){
            var table = _font.characterTable;

            var unicode   = Convert.ToUInt32(input);
            var character = table.Find(c => c.unicode == unicode);
            if (character == null) return;

            var rect    = character.glyph.glyphRect;
            var padding = _font.atlasPadding;

            var data = new Vector4(
                rect.x      - padding,
                rect.y      - padding,
                rect.width  + padding * 2f,
                rect.height + padding * 2f
            );

            _material.SetVector("_Rect", data);
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