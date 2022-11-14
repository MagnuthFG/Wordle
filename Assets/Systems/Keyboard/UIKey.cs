using SF = UnityEngine.SerializeField;
using UnityEngine;
using Wordl.Interface;
using UnityEngine.EventSystems;

namespace Wordl.Keyboard
{
    public class UIKey : UIButton
    {
        [SF] private UICharacter _character = null;

// PROPERTIES

        public string Key => _character.Character;

// SETTINGS

        /// <summary>
        /// Specifies: displayed character
        /// </summary>
        public void SetKey(string input){
            _character.SetCharater(input);
        }

// INTERFACE

        /// <summary>
        /// Event: on mouse clicked
        /// </summary>
        public override void OnPointerClick(PointerEventData data){
            _btnTarget?.OnClicked(Key);
        }
    }
}