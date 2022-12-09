using SF = UnityEngine.SerializeField;
using Random = System.Random;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Magnuth.Interface;

namespace Wordl
{
    [DefaultExecutionOrder(1)]
    public class WordHandler : MonoBehaviour
    {
        [SF] private TextAsset  _wordList = null;
        [SF] private UIKeyboard _keyboard = null;
        [SF] private UIGrid     _slotGrid = null;
        [Space]
        [SF] private Canvas _uiCanvas = null;
        [SF] private Text   _uiText = null;
        [Space]
        [SF] private Color _correct   = Color.green;
        [SF] private Color _incorrect = Color.yellow;
        [SF] private Color _wrong     = Color.red;

        private string _wordToday   = string.Empty;
        private string _wordInput   = string.Empty;
        private List<string> _words = null;

        private List<CharSlot>  _changed  = null;
        private Stack<CharSlot> _unfilled = null;
        private Stack<CharSlot> _filled   = null;
        private Dictionary<char, UIKey> _keys  = null;

        private Color _defSlotColour = Color.magenta;
        private Color _defKeyColour  = Color.magenta;

        private const char   NULLCHAR      = '\0';
        private const char   RETURNCHAR    = '\n';
        private const char   BACKSPACECHAR = '\b';
        private const string COLOUR_PARAM  = "_Color2";

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
            FetchDefaults();
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
        
        /// <summary>
        /// Resets the interface and starts a new game
        /// </summary>
        public void ResetGame(){
            foreach (var key in _keys.Values){
                key.SetColour(_defKeyColour, COLOUR_PARAM);
            }

            foreach (var slot in _filled){
                slot.SetColour(_defSlotColour, COLOUR_PARAM);
                slot.SetCharacter(NULLCHAR);
                _unfilled.Push(slot);
            }

            _wordInput = string.Empty;
            _wordToday = GetTodaysWord();

            _filled.Clear();
            _changed.Clear();

            _uiCanvas.enabled = false;
            this.enabled = true;
        }

// LOADING

        /// <summary>
        /// Picks the word of the day
        /// </summary>
        private void LoadTodaysWord(){
            _words = GetWordList();
            
            if (_words?.Count > 0){
                _wordToday = GetTodaysWord();

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
        private string GetTodaysWord(){
            //var date = System.DateTime.Now.Date;
            //var seed = $"{date.Year}{date.Month}{date.Day}";

            var time   = System.DateTime.Now.TimeOfDay;
            var seed   = $"{time.Hours}{time.Minutes}{time.Seconds}";

            var random = new Random(int.Parse(seed));
            var index  = random.Next(0, _words.Count);

            return _words[index].ToLower();
        }


        /// <summary>
        /// Retrieves and adds the input slots
        /// </summary>
        private void FetchSlots(){
            var slots = _slotGrid.GetComponentsInChildren<CharSlot>();

            _unfilled = new Stack<CharSlot>(slots.Length);
            _filled   = new Stack<CharSlot>(slots.Length);
            _changed  = new List<CharSlot>(_wordToday.Length);

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

        /// <summary>
        /// Retrieves the default settings
        /// </summary>
        private void FetchDefaults(){
            var slot = _unfilled.Peek();

            var sMaterial  = slot.Material;
            _defSlotColour = sMaterial.GetColor(COLOUR_PARAM);

            var kMaterial = _keys['a'].Material;
            _defKeyColour = kMaterial.GetColor(COLOUR_PARAM);
        }

// WORD INPUT

        /// <summary>
        /// On UI keyboard input callback
        /// </summary>
        private void OnKeyboardInput(char input){
            if (!this.enabled) return;

            switch (input){
                case NULLCHAR:                        break;
                case RETURNCHAR:    CheckWord();      break;
                case BACKSPACECHAR: RemoveFromWord(); break;
                default:            AddToWord(input); break;
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
            
			_wordInput = _wordInput.Remove(
				_wordInput.Length - 1
			);

            if (_filled.Count == 0) return;
            var slot = _filled.Pop();
            slot.SetCharacter(NULLCHAR);

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

            if (won || (!won && _unfilled.Count == 0)){ 
                DisplayWinLoose(won);
            }
        }

        /// <summary>
        /// Displays the final game result in the console
        /// </summary>
        private void DisplayWinLoose(bool won){
            var colour  = won ? _correct : _wrong;

            var message = won ?
                "Congratulation! You guessed the right word!" +
                $"\nThe word you guessed was {_wordToday.ToUpper()}" :
                
                "I'm sorry. You have no more chances left." +
                $"\nThe word we were looking for was {_wordToday.ToUpper()}.";

            var hex = ColorUtility.ToHtmlStringRGB(colour);
            Debug.Log($"<color=#{hex}><b>{message}</b></color>");

            // Placeholder until I have had the
            // time to create my own UI text script
            _uiText.text = message;
            _uiCanvas.enabled = true;

            this.enabled = false;
        }

// WORD COMPARISON

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