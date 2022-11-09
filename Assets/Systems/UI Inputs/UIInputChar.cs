using SF = UnityEngine.SerializeField;
using Convert = System.Convert;
using UnityEngine;
using TMPro;

namespace Wordl
{
    [RequireComponent(typeof(MeshRenderer))]
    public class UIInputChar : MonoBehaviour
    {
        [SF] private TMP_FontAsset _font = null;
        private Material _material = null;

// INITIALISATION

        /// <summary>
        /// Initialises the input
        /// </summary>
        private void Awake(){
            var renderer = GetComponent<MeshRenderer>();
            _material = renderer.material;
            SetCharater("T"); // TEMP
        }

// INPUT HANDLING

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