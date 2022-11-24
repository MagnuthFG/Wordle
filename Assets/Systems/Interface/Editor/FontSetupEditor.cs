using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;
using System;

namespace Magnuth.Interface
{
    [CustomEditor(typeof(FontSetup))]
    public class FontSetupEditor : Editor
    {
        private SerializedObject   _sObj  = null;
        private SerializedProperty _atlas = null;
        private SerializedProperty _tiles = null;
        private SerializedProperty _size  = null;
        private SerializedProperty _chars = null;

// INITIALISATION

        /// <summary>
        /// Initialises the interface
        /// </summary>
        private void OnEnable(){
            InitProperties();

        }

        /// <summary>
        /// Initialises the serialized properties
        /// </summary>
        private void InitProperties(){
            _sObj = serializedObject;

            _atlas = _sObj.FindProperty("_atlas");
            _tiles = _sObj.FindProperty("_tiles");

            _size  = _sObj.FindProperty("_size");
            _chars = _sObj.FindProperty("_characters");
        }

// INTERFACE

        /// <summary>
        /// Creates the inspector interface
        /// </summary>
        public override VisualElement CreateInspectorGUI(){
            var root = new VisualElement();

            AddTexProperties(root);
            AddCharProperties(root);
            AddButtons(root);

            return root;
        }

        /// <summary>
        /// Appends the texture properties
        /// </summary>
        private void AddTexProperties(VisualElement root){
            root.Add(new Label("Texture"));
            root.Add(new PropertyField(_atlas));
            root.Add(new PropertyField(_tiles));
            root.Add(new Label(""));
        }

        /// <summary>
        /// Appends the character properties
        /// </summary>
        private void AddCharProperties(VisualElement root){
            var height  = EditorGUIUtility.singleLineHeight * 2f;
            var texture = (Texture)_atlas.objectReferenceValue;
            var size    = _size.vector2Value;

            root.Add(new Label("Characters"));

            for (int i = 0; i < _chars.arraySize; i++){
                var info  = _chars.GetArrayElementAtIndex(i);

                var id    = info.FindPropertyRelative("ID");
                var coord = info.FindPropertyRelative("Coord");
                var cval  = coord.vector2Value;

                //var group = CreateCharGroup(height);
                //group.Add();

                var image = CreateImage(texture, height, size, cval);
                root.Add(image);

                var text = CreateIDText(id);
                root.Add(text);

                //root.Add(new PropertyField(id, ""));
                root.Add(new PropertyField(coord, ""));

                //root.Add(group);
                root.Add(new Label(""));
            }
        }

        /// <summary>
        /// Appends the build button
        /// </summary>
        private void AddButtons(VisualElement root){
            var button = new Button(
                () => OnBuildButton()
            );

            button.text = "Build";
            root.Add(button);
        }

// ELEMENTS

        /// <summary>
        /// 
        /// </summary>
        private GroupBox CreateCharGroup(float height){
            var group = new GroupBox();

            var style = group.style;
            style.height = height;

            var align = style.alignItems;
            align.value = Align.Auto;

            return group;
        }

        /// <summary>
        /// 
        /// </summary>
        private Image CreateImage(Texture texture, float height, Vector2 size, Vector2 coord){
            var image = new Image(){
                image = texture,
                uv = new Rect(coord, size)
            };

            var style = image.style;
            style.width  = height;
            style.height = height;

            style.marginTop    = 5;
            style.marginBottom = 5;
            style.marginRight  = 5;

            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        private TextField CreateIDText(SerializedProperty property){
            var text = new TextField(2, false, false, '\0');
            text.value = new string((char)property.intValue, 1);

            text.RegisterValueChangedCallback(
                (txt) => OnIDChanged(property, txt.newValue)
            );

            return text;
        }

// EVENTS

        /// <summary>
        /// 
        /// </summary>
        private void OnIDChanged(SerializedProperty id, string value){
            if (value.Length > 1 && char.IsLetter(value[0]))
                value = value.Substring(0, 1);

            id.intValue = char.Parse(value);

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        /// <summary>
        /// On build button pressed
        /// </summary>
        private void OnBuildButton(){
            var script = ((FontSetup)target);

            script.CreateCharacters();

            _sObj.Update();
        }
    }
}