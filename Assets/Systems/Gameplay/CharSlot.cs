using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface
{
    public class CharSlot : UIObject
    {
        [SF] private UICharacter _character = null;

// PROPERTIES

        public Color Colour    => _colour;
        public Material Material => _material;
        public char Character => _character.Character;

// SETTINGS

        /// <summary>
        /// Specifies: displayed character
        /// </summary>
        public void SetCharacter(char input){
            _character.SetCharater(input);
        }
    }
}