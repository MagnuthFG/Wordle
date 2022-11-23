using SF = UnityEngine.SerializeField;
using Random = System.Random;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Magnuth.Interface;

namespace Wordl
{
    [DefaultExecutionOrder(1)]
    public class WordHandler : MonoBehaviour
    {
        [SF] private Color _correct = Color.green;
        [SF] private Color _incorrect = Color.yellow;
        [SF] private Color _wrong = Color.red;
        [Space]
        [SF] private TextAsset  _wordList = null;
        [SF] private UIKeyboard _keyboard = null;
        [SF] private UIGrid     _slotGrid = null;

        private string _wordToday = string.Empty;
        private string _wordInput = string.Empty;

        private List<UISlot>  _changed  = null;
        private Stack<UISlot> _unfilled = null;
        private Stack<UISlot> _filled   = null;
        private Dictionary<char, UIKey> _keys  = null;

        private const int WORDLENGTH = 5;
        private const string COLOUR_PARAM = "_Color2";

// INITIALISATION

        /// <summary>
        /// Initialises the word of the day
        /// </summary>
        private void Awake(){
            LoadTodaysWord();
        }

        /// <summary>
        /// Initialises slots and keys references
        /// </summary>
        private void Start(){
            FetchSlots();
            FetchKeys();
        }

        /// <summary>
        /// Initialises keyboard input callback
        /// </summary>
        private void OnEnable(){
            _keyboard.Subscribe(OnKeyboardInput);
        }

        /// <summary>
        /// Deinitialises keyboard input callback
        /// </summary>
        private void OnDisable(){
            _keyboard.Unsubscribe(OnKeyboardInput);
        }
        
// LOADING

        /// <summary>
        /// Picks the word of the day
        /// </summary>
        private void LoadTodaysWord(){
            var list = GetWordList();
            
            if (list?.Count > 0){
                _wordToday = GetTodaysWord(list);

            } else Debug.LogError(
                new System.NullReferenceException("No words in list")
            );
        }

        /// <summary>
        /// Returns a list of words
        /// </summary>
        private List<string> GetWordList(){
            if (_wordList == null) return null;
            var list = new List<string>();
            var word = "";

            var reader = new StringReader(_wordList.text);
            while ((word = reader.ReadLine()) != null){
                list.Add(word);
            }

            reader.Dispose();
            return list;
        }

        /// <summary>
        /// Returns the pseudo random word of the day
        /// </summary>
        private string GetTodaysWord(List<string> list){
            var date = System.DateTime.Now.Date;
            var seed = $"{date.Year}{date.Month}{date.Day}";

            var random = new Random(int.Parse(seed));
            var index = random.Next(0, list.Count);

            return list[index].ToLower();
        }


        /// <summary>
        /// Retrieves and adds the input slots
        /// </summary>
        private void FetchSlots(){
            var slots = _slotGrid.GetComponentsInChildren<UISlot>();
            _unfilled = new Stack<UISlot>(slots.Length);
            _filled   = new Stack<UISlot>(slots.Length);
            _changed  = new List<UISlot>(WORDLENGTH);

            for (int i = slots.Length - 1; i >= 0; i--){
                _unfilled.Push(slots[i]);
            }
        }

        /// <summary>
        /// Retrieves and adds the keyboard keys
        /// </summary>
        private void FetchKeys(){
            var keys = _keyboard.GetComponentsInChildren<UIKey>();
            _keys = new Dictionary<char, UIKey>(keys.Length);

            foreach (var key in keys){
                _keys.TryAdd(key.Character, key);
            }
        }

// WORD INPUT

        /// <summary>
        /// On UI keyboard input callback
        /// </summary>
        private void OnKeyboardInput(char input){
            switch (input){
                case '+': CompareWords();   break;
                case '-': RemoveFromWord(); break;
                default:  AddToWord(input); break;
            }
        }

        /// <summary>
        /// Adds character to end of word and slot row
        /// </summary>
        private void AddToWord(char input){
            if (_wordInput.Length >= WORDLENGTH) return;
            _wordInput = string.Concat(_wordInput, input);

            if (_unfilled.Count == 0) return;
            var slot = _unfilled.Pop();
            slot.SetCharacter(input);
            
            _filled.Push(slot);
            _changed.Add(slot);
        }

        /// <summary>
        /// Removes last character in word and slot row
        /// </summary>
        private void RemoveFromWord(){
            if (_wordInput.Length == 0) return;
            _wordInput = _wordInput.Remove(_wordInput.Length - 1);

            if (_filled.Count == 0) return;
            var slot = _filled.Pop();
            slot.SetCharacter('\0');

            _unfilled.Push(slot);
            _changed.Remove(slot);
        }

        /// <summary>
        /// Compares words and displays the result
        /// </summary>
        private void CompareWords(){
            if (_wordInput.Length != WORDLENGTH) return;



            _wordInput = string.Empty;
            _changed.Clear();
        }

        private void OLDCompareWords(){
            if (_wordInput.Length != WORDLENGTH) return;
            Debug.Log($"TODAY {_wordToday} INPUT {_wordInput}");

            for (int i = 0; i < _wordInput.Length; i++){
                var input = _wordInput[i];
                var level = 0;

                for (int j = 0; j < _wordToday.Length; j++){
                    if (input == _wordToday[j]){
                        level = i == j ? 2 : 1;
                        break;
                    }
                }

                _changed[i].SetColour(
                    level > 1 ? _correct : level > 0 ? _incorrect : _wrong,
                    COLOUR_PARAM
                );

                if (level > 0) continue;
                _keys[input].SetColour(_wrong, COLOUR_PARAM);
            }

            _wordInput = string.Empty;
            _changed.Clear();
        }
    }
}