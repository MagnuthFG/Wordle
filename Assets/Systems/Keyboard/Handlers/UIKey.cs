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

        public char Character => _character.Character;

// SETTINGS

        /// <summary>
        /// Changes the displayed character
        /// </summary>
        public void SetKey(char input){
            _character?.SetCharater(input);
        }

// INTERFACE

        /// <summary>
        /// On button released event
        /// </summary>
        public override void OnPointerUp(PointerEventData data){
            SetColour(_hovering ? _highlighted : _colour);
            _btnTarget?.OnClicked(new object[1]{ Character });
        }
    }
}