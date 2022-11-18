using SF = UnityEngine.SerializeField;
using UnityEngine;
using System.Collections.Generic;
using Magnuth.Interface;

namespace Wordl
{
    [DefaultExecutionOrder(1)]
    public class WordHandler : MonoBehaviour
    {
        [SF] private UIGrid     _slotGrid = null;
        [SF] private UIKeyboard _keyboard = null;

        private string _word = string.Empty;
        private const int WORDLENGTH = 5;

        private Stack<UISlot> _unfilled = null;
        private Stack<UISlot> _filled   = null;

// INITIALISATION

        /// <summary>
        /// Initialises: slot stacks
        /// </summary>
        private void Start(){
            var slots = _slotGrid.GetComponentsInChildren<UISlot>();
            _unfilled = new Stack<UISlot>();
            _filled   = new Stack<UISlot>();

            for (int i = slots.Length - 1; i >= 0; i--){
                _unfilled.Push(slots[i]);
            }
        }

        /// <summary>
        /// Initialises: keyboard input callback
        /// </summary>
        private void OnEnable(){
            _keyboard.Subscribe(OnKeyboardInput);
        }

        /// <summary>
        /// Deinitialises: keyboard input callback
        /// </summary>
        private void OnDisable(){
            _keyboard.Unsubscribe(OnKeyboardInput);
        }

// WORD INPUT

        /// <summary>
        /// Event: on UI keyboard input callback
        /// </summary>
        private void OnKeyboardInput(string input){
            switch (input){
                case "+": CompareWord();    break;
                case "-": RemoveFromWord(); break;
                default:  AddToWord(input); break;
            }
        }

        /// <summary>
        /// Adds character to end of word and slot row
        /// </summary>
        private void AddToWord(string input){
            if (_word.Length >= WORDLENGTH) return;
            _word = string.Concat(_word, input);

            if (_unfilled.Count == 0) return;
            var slot = _unfilled.Pop();
            
            slot.SetCharacter(input);
            _filled.Push(slot);
        }

        /// <summary>
        /// Removes last character in word and slot row
        /// </summary>
        private void RemoveFromWord(){
            if (_word.Length == 0) return;
            _word = _word.Remove(_word.Length - 1);

            if (_filled.Count == 0) return;
            var slot = _filled.Pop();

            slot.SetCharacter("");
            _unfilled.Push(slot);
        }

        /// <summary>
        /// Submits word for comparison
        /// </summary>
        private void CompareWord(){
            if (_word.Length != WORDLENGTH) return;

            // Compare to selected word

            _word = string.Empty;
        }
    }
}