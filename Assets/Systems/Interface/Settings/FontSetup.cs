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
        [SF] private Texture2D _atlas = null;
        [SF] private Vector2Int _columnsRows = Vector2Int.zero;

        private List<string> _fontIDs = new();
        private List<Rect> _fontRects = new();
        
// HANDLING

        /// <summary>
        /// 
        /// </summary>
        public void Build(){
            var size = _atlas.texelSize;
            var area = size / _columnsRows;

            for (int y = 0; y < _columnsRows.y; y++){
                for (int x = 0; x < _columnsRows.x; x++){

                }
            }
        }

        /// <summary>
        /// Returns: 
        /// </summary>
        public Rect GetRect(string character){

            return default;
        }
    }
}