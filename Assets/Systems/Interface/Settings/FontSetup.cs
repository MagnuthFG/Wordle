using SF = UnityEngine.SerializeField;
using UnityEngine;
using System.Collections.Generic;

namespace Magnuth.Interface
{
    public class FontSetup : ScriptableObject
    {
        [SF] private Texture2D _atlas = null;      
        private bool _initialised = false;

        // Must be able to include:
        // - Find char rects on atlas using alpha channel?
        //   Generate this automatically when the atlas is assigned
        //
        // - Each char must be drawn in the inspector
        //   Create thumbnails for each char seems exessive?
        //
        // - Set char ID for each drawn font character
        //   User must be able to specify this via inspector
        //
        // - Get the rect from character ID or font character
        private Dictionary<string, Rect> _characters = new(); // Replace with uDictionary


        /// <summary>
        /// Returns: 
        /// </summary>
        public Rect GetCharacter(string character){
            return default;
        }
    }
}