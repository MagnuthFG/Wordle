using SF = UnityEngine.SerializeField;
using UnityEngine;
using System.Collections.Generic;
using Magnuth.Interface;

namespace Wordl
{
    [DefaultExecutionOrder(1)]
    public class SlotManager : MonoBehaviour
    {
        [SF] private UIGrid     _slotGrid = null;
        [SF] private UIKeyboard _keyboard = null;

        private Stack<UISlot> _unfilled = null;
        private Stack<UISlot> _filled   = null;

// INITIALISATION

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

        /// <summary>
        /// Initialises: slot stacks
        /// </summary>
        private void Start(){
            var slots = _slotGrid.GetComponentsInChildren<UISlot>();
            _unfilled = new Stack<UISlot>();
            _filled   = new Stack<UISlot>();

            for (int i = 0; i < slots.Length; i++){
                _unfilled.Push(slots[i]);
            }
        }

// INPUT CALLBACK

        /// <summary>
        /// 
        /// </summary>
        private void OnKeyboardInput(string input){
            // If return then move slot back on top of unfilled stack
            // If filled passes (count - 1 % 5 == 0) then it cant go below it

            if (_unfilled.Count == 0) return;
            var slot = _unfilled.Pop();
            
            slot.SetCharacter(input);
            
            _filled.Push(slot);
        }
    }
}