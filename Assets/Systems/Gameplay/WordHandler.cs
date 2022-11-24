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
        [SF] private int        _chances  = 5;
        [SF] private TextAsset  _wordList = null;
        [Space]
        [SF] private UIKeyboard _keyboard = null;
        [SF] private UIGrid     _slotGrid = null;
        [Space]
        [SF] private Color _correct   = Color.green;
        [SF] private Color _incorrect = Color.yellow;
        [SF] private Color _wrong     = Color.red;

        private string _wordToday = string.Empty;
        private string _wordInput = string.Empty;

        private List<UISlot>  _changed  = null;
        private Stack<UISlot> _unfilled = null;
        private Stack<UISlot> _filled   = null;
        private Dictionary<char, UIKey> _keys  = null;

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
            _changed  = new List<UISlot>(_wordToday.Length);

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
                case '+': CheckWord();      break;
                case '-': RemoveFromWord(); break;
                default:  AddToWord(input); break;
            }
        }

        /// <summary>
        /// Adds character to end of word and slot row
        /// </summary>
        private void AddToWord(char input){
            if (_wordInput.Length == _wordToday.Length) return;
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
        /// Checks the input against the word of the day
        /// </summary>
        private void CheckWord(){
            var count = _wordToday.Length;
            if (_wordInput.Length != count) return;

            var score = CompareWords();
            var won   = score == count * 2;
            _chances -= 1;

            if (won || (!won && _chances == 0)){ 
                DisplayWinLoose(won);
            }
        }

        /// <summary>
        /// Displays the final game result in the console
        /// </summary>
        private void DisplayWinLoose(bool won){
            var colour  = won ? _correct : _wrong;

            var message = won ?
                "Congratulation! You guessed the right word!":
                "I'm sorry. You have no more chances left";

            var hex = ColorUtility.ToHtmlStringRGB(colour);
            Debug.Log($"<color=#{hex}>{message}</color>");

            _keyboard.enabled = false;
        }


        /// <summary>
        /// Compares words and displays the result
        /// <br>Returns the score of the guessed word</br>
        /// </summary>
        private int CompareWords(){
            var score = 0;

            for (int i = 0; i < _wordInput.Length; i++){
                var input  = _wordInput[i];
                var level  = CompareWord(input, i);
                    score += level;

                _changed[i].SetColour(
                    level > 1 ? _correct   : 
                    level > 0 ? _incorrect : _wrong,
                    COLOUR_PARAM
                );

                if (level > 0) continue;

                _keys[input].SetColour(
                    _wrong, COLOUR_PARAM
                );
            }

            _wordInput = string.Empty;
            _changed.Clear();
            return score;
        }

        /// <summary>
        /// Compares the character with the word of the day
        /// <br>Returns level of correctness</br>
        /// </summary>
        private int CompareWord(char character, int index){
            if (_wordToday[index] == character) return 2;

            for (int i = 0; i < _wordToday.Length; i++){
                if (_wordToday[i] == character) return 1;
            }

            return 0;
        }
    }
}