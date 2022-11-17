using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// CREDIT: Based on the version created by christophfranke123 and Zoodinger
// https://answers.unity.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html

namespace Magnuth
{
    [System.Serializable]
    public class UDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SF] private List<TKey> _keys = new List<TKey>();
        [SF] private List<TValue> _values = new List<TValue>();

// SERIALIZATION

        /// <summary>
        /// Add to dictionary
        /// </summary>
        public new void Add(TKey key, TValue value){
            if (TryAdd(key, value)){
                _keys.Add(key);
                _values.Add(value);
            }
        }

        /// <summary>
        /// Saves dictionary to lists
        /// </summary>
        public void OnBeforeSerialize(){
            _keys.Clear();
            _values.Clear();

            if (typeof(TKey) == typeof(Object) ||
                typeof(TKey).IsSubclassOf(typeof(Object))){

                foreach (var element in this.Where(element => element.Key != null)){
                    _keys.Add(element.Key);
                    _values.Add(element.Value);
                }

            } else {
                foreach (var element in this){
                    _keys.Add(element.Key);
                    _values.Add(element.Value);
                }
            }
        }

        /// <summary>
        /// Loads dictionary from lists
        /// </summary>
        public void OnAfterDeserialize(){
            Clear();

            if (typeof(TKey) == typeof(Object) ||
                typeof(TKey).IsSubclassOf(typeof(Object))){

                for (int i = 0; i < _keys.Count; i++){
                    var key = _keys[i];

                    if (key != null)
                        Add(key, _values[i]);
                }

            } else {
                for (int i = 0; i < _keys.Count; i++){
                    Add(_keys[i], _values[i]);
                }
            }
        }

#if UNITY_EDITOR

        private int _marked = -1;

        /// <summary>
        /// Lists the dictionary items inside inspector
        /// </summary>
        /// <param name="parent">The serialized object containing this dictionary</param>
        /// <param name="dictionary">The serialized property of this dictionary</param>
        public void DrawInspector(SerializedObject parent, SerializedProperty dictionary){
            var keys    = dictionary.FindPropertyRelative("_keys");
            var values  = dictionary.FindPropertyRelative("_values");

            EditorGUI.BeginChangeCheck();

            DrawList(keys, values);
            DrawButtons(keys, values);

            if (EditorGUI.EndChangeCheck()){
                parent.ApplyModifiedProperties();
                parent.Update();
            }
        }

        /// <summary>
        /// Displays the dictionary items in the inspector
        /// </summary>
        private void DrawList(SerializedProperty keys, SerializedProperty values){
            var idle = EditorGUIUtility.IconContent("d_curvekeyframesemiselectedoverlay");
            var marked = EditorGUIUtility.IconContent("d_curvekeyframeselected");
            var empty = new GUIContent("");

            for (int i = 0; i < keys.arraySize; i++){
				GUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(
                    keys.GetArrayElementAtIndex(i), empty
                );
                EditorGUILayout.PropertyField(
                    values.GetArrayElementAtIndex(i), empty
                );

                if (GUILayout.Button(_marked == i ? marked : idle))
                    _marked = i;

                GUILayout.EndHorizontal();
			}
        }

        /// <summary>
        /// Displays the control buttons in the inspector
        /// </summary>
        private void DrawButtons(SerializedProperty keys, SerializedProperty values){
            var plus = EditorGUIUtility.IconContent("Toolbar Plus");
            var minus = EditorGUIUtility.IconContent("Toolbar Minus");
            var up = EditorGUIUtility.IconContent("tab_prev");
            var down = EditorGUIUtility.IconContent("tab_next");

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(plus)){
                InsertDefault<TKey>(keys);
                InsertDefault<TValue>(values);
            }

            if (GUILayout.Button(minus)){
                keys.DeleteArrayElementAtIndex(_marked);
                values.DeleteArrayElementAtIndex(_marked);
                _marked = -1;
            }

            if (GUILayout.Button(up)){
                var target = _marked - 1 < 0 ? keys.arraySize - 1 : _marked - 1;
                keys.MoveArrayElement(_marked, target);
                values.MoveArrayElement(_marked, target);
                _marked = target;
            }

            if (GUILayout.Button(down)){
                var target = (_marked + 1) % keys.arraySize;
                keys.MoveArrayElement(_marked, target);
                values.MoveArrayElement(_marked, target);
                _marked = target;
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Inserts a new default element in the dictionary
        /// </summary>
        private void InsertDefault<T>(SerializedProperty property){
            property.InsertArrayElementAtIndex(property.arraySize);
            var element = property.GetArrayElementAtIndex(property.arraySize - 1);

            switch (element.propertyType){
                case SerializedPropertyType.Enum:
                    System.Enum.TryParse(typeof(T), default(T).ToString(), out var result);
                    element.enumValueIndex = (int)result;
                    break;

                default: break;
            }
        }

#endif
    }
}