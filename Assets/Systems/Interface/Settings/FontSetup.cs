using HIDE = UnityEngine.HideInInspector;
using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;

namespace Magnuth.Interface
{
    [CreateAssetMenu(fileName = "Font Setup",
     menuName = "Magnuth/Interface/Font Setup")]
    public class FontSetup : ScriptableObject
    {
        [Header("Texture")]
        [SF] private Texture2D  _atlas = null;
        [SF] private Vector2Int _columnsRows = Vector2Int.zero;

        [SF] private List<string> _fontIDs   = new();
        [SF] private List<Rect>   _fontRects = new();
        
// HANDLING

        /// <summary>
        /// Creates the font data
        /// </summary>
        [ContextMenu("Build")]
        public void Build(){
            var width  = _atlas.width  / _columnsRows.x;
            var height = _atlas.height / _columnsRows.y;
            var rect   = new Rect(0, 0, width, height);

            for (int y = 0; y < _columnsRows.y; y++){
                for (int x = 0; x < _columnsRows.x; x++){
                    rect.x = width  * x;
                    rect.y = height * y;
                    _fontRects.Add(rect);

                    var temp = (y * _columnsRows.x) + x;
                    _fontIDs.Add(temp.ToString());
                }
            }
        }

        /// <summary>
        /// Adds a new character rect
        /// </summary>
        public void AddCharacter(string character, Rect rect){
            var index = _fontIDs.IndexOf(character);
            
            if (index < 0){
                _fontIDs.Add(character);
                _fontRects.Add(rect);
            
            } else _fontRects[index] = rect;
        }

        /// <summary>
        /// Removes the character rect
        /// </summary>
        public void RemoveCharacter(string character){
            var index = _fontIDs.IndexOf(character);
            if (index < 0) return;

            _fontIDs.RemoveAt(index);
            _fontRects.RemoveAt(index);
        }

        /// <summary>
        /// Returns: character rect on font atlas
        /// </summary>
        public Rect GetRect(string character){
            var index = _fontIDs.IndexOf(character);

            if (index < 0) return default;
            return _fontRects[index];
        }
    }
}