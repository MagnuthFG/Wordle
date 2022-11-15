using SF = UnityEngine.SerializeField;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Magnuth.Interface
{
    [AddComponentMenu("Magnuth/Interface/UI Key")]
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
        /// Event: on mouse released
        /// </summary>
        public override void OnPointerUp(PointerEventData data){
            SetColour(_hovering ? _highlighted : _colour);
            _btnTarget?.OnClicked(Key);
        }
    }
}