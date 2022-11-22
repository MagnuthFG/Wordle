using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;

namespace Magnuth.Interface
{
    [CreateAssetMenu(fileName = "Font Setup",
     menuName = "Magnuth/Interface/Font Setup")]
    public class FontSetup : ScriptableObject
    {
        [System.Serializable]
        public class CharInfo {
            public char ID = default;
            public Vector2 Coord = Vector2.zero;

            public CharInfo(char ID, Vector2 coord) {
                this.Coord = coord;
                this.ID = ID;
            }
        }

        [SF] private Texture2D _atlas = null;
        [SF] private Vector2Int _tiles = new Vector2Int(8, 8);

        [SF] private Vector2 _size = Vector2.zero;
        [SF] private List<CharInfo> _characters = new();

// INITIALISATION

        /// <summary>
        /// Creates the font data
        /// </summary>
        public void CreateCharacters(){
            var point = Vector2.zero;
            var blank = ' ';

            _size = GetCharacterSize();
            _characters.Clear();

            for (int y = 0; y < _tiles.y; y++){
            for (int x = 0; x < _tiles.x; x++){
                point.x = _size.x * x;
                point.y = _size.y * y;

                if (!ValidCharacter(point, _size)) 
                    continue;

                _characters.Add(
                    new CharInfo(blank, point)
                );
            }}
        }

        /// <summary>
        /// Returns true if the tile contains a character
        /// </summary>
        private bool ValidCharacter(Vector2 point, Vector2 size){
            var width  = _atlas.width;
            var height = _atlas.height;

            var pixels = _atlas.GetPixels(
                (int)(width  * point.x),
                (int)(height * point.y),
                (int)(width  * size.x),  
                (int)(height * size.y)
            );

            for (int i = 0; i < pixels.Length; i++){
                if (pixels[i].r > 0.5f) return true;     
            }

            return false;
        }

        /// <summary>
        /// Returns the character tile size, range 0 to 1
        /// </summary>
        private Vector2 GetCharacterSize(){
            var width  = _atlas.width;
            var height = _atlas.height;

            return new Vector2(
                Mathf.InverseLerp(0, width, width / _tiles.x),
                Mathf.InverseLerp(0, height, height / _tiles.y)
            );
        }

// HANDLING

        /// <summary>
        /// Returns the character rect
        /// </summary>
        public Rect GetRect(char character){
            var info = _characters.Find(
                c => c.ID == character
            );

            if (info == null)
                return Rect.zero;

            return new Rect(
                info.Coord.x, info.Coord.y,
                _size.x, _size.y
            );
        }

        /// <summary>
        /// Returns the character vector
        /// </summary>
        public Vector4 GetVector(char character){
            var info = _characters.Find(
                c => c.ID == character
            );

            if (info == null) 
                return Vector4.zero;

            return new Vector4(
                info.Coord.x, info.Coord.y, 
                _size.x, _size.y
            );
        }
    }
}