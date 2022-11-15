using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface
{
    public class UISlot : UIObject
    {
        [SF] private UICharacter _character = null;

// PROPERTIES

        public string Character => _character.Character;

// SETTINGS

        /// <summary>
        /// Specifies: displayed character
        /// </summary>
        public void SetCharacter(string input){
            _character.SetCharater(input);
        }
    }
}