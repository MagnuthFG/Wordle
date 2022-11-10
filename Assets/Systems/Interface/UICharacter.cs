using SF = UnityEngine.SerializeField;
using Convert = System.Convert;
using UnityEngine;
using TMPro;

namespace Wordl.Interface
{
    public class UICharacter : UIObject
    {
        [SF] private TMP_FontAsset _font = null;

// INITIALISATION

        /// <summary>
        /// Initialises the interface
        /// </summary>
        private void Start(){
            SetCharater("T"); // TEMP
        }

// SETTINGS

        /// <summary>
        /// Changes the displayed character
        /// </summary>
        public void SetCharater(string input){
            SetCharater(input[0]);
        }

        /// <summary>
        /// Changes the displayed character
        /// </summary>
        public void SetCharater(char input){
            var table   = _font.characterTable;

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
    }
}