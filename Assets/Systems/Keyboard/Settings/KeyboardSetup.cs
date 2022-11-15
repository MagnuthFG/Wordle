using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Keyboard
{
    public class KeyboardSetup : ScriptableObject
    {
        public struct Layout {
            public string[] Characters;
        }

        [SF] private Layout[] _characters = null;
    }
}