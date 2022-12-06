using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;

namespace Magnuth.Interface
{
    [DisallowMultipleComponent]
    public abstract class UIElement : MonoBehaviour
    {
        [Header("Transform")]
        [SF] protected Vector3 _point   = Vector3.zero;
        [SF] protected Vector3 _size    = Vector3.one;
        [SF] protected Sizing  _sizing  = Sizing.Absolute;

        protected Transform _transform = null;
        protected List<UIElement> _elements = new();

// INITIALISATION

        /// <summary>
        /// Initialises the element size and position
        /// </summary>
        protected virtual void Awake(){
            _transform = transform;
            _point = _transform.localPosition;
            _size  = _transform.localScale;
        }

        /// <summary>
        /// Initialises the element transform
        /// </summary>
        /// <param name="position">Desired local position</param>
        /// <param name="origin">Parent local position</param>
        /// <param name="size">Desired local scale</param>
        /// <param name="scaler">Parent scaler</param>
        public void Initialise(Vector3 position, Vector3 origin, Vector3 size, Vector3 scaler){
            _transform.localPosition = position;
            _transform.localScale = size;

            _point = position;
            _size = size;

            Rescale(scaler);
            Reposition(origin);
        }

// TRANSFORM

        /// <summary>
        /// Changes the element transform
        /// </summary>
        /// <param name="position">Desired local position</param>
        /// <param name="origin">Parent local position</param>
        /// <param name="size">Desired local scale</param>
        /// <param name="scaler">Parent scaler</param>
        public void SetTransform(Vector3 position, Vector3 origin, Vector3 size, Vector3 scaler){
            _transform.localPosition = position;
            _transform.localScale = size;

            _point = position;
            _size = size;

            Rescale(scaler);
            Reposition(origin);

            // NotifyTransform -> Retransform
        }


        /// <summary>
        /// Changes the current position
        /// </summary>
        /// <param name="position">Desired local position</param>
        /// <param name="origin">Parent local position</param>
        public void SetPosition(Vector3 position, Vector3 origin){
            _transform.localPosition = position;
            _point = position;

            Reposition(origin);
            NotifyPosition();
        }

        /// <summary>
        /// Repositions element based on parent position
        /// </summary>
        /// <param name="scaler">Parent local position</param>
        public void Reposition(Vector3 origin){
            // If positioning is relative return
            // if fixed then move in the other direction for length

            
        }


        /// <summary>
        /// Changes the current size
        /// </summary>
        /// <param name="size">Desired local scale</param>
        /// <param name="scaler">Parent scaler</param>
        public void SetSize(Vector3 size, Vector3 scaler){
            _transform.localScale = size;
            _size = size;

            Rescale(scaler);
            NotifySize();
        }

        /// <summary>
        /// Rescales current size based on parent scaler
        /// </summary>
        /// <param name="scaler">Parent scaler value</param>
        public void Rescale(Vector3 scaler){
            if (_sizing == Sizing.Relative) return;
                // notify size
            // else
            _transform.localScale = new Vector3(
                _size.x / scaler.x,
                _size.y / scaler.y,
                _size.z / scaler.z
            );
        }


        /// <summary>
        /// Returns current scaler value
        /// </summary>
        protected Vector3 GetScaler(){
            var size = transform.localScale;

            return new Vector3(
                size.x / _size.x,
                size.y / _size.y,
                size.y / _size.z
            );
        }

// NOTIFICATION

        /// <summary>
        /// Notifies child elements of new position
        /// </summary>
        protected void NotifyPosition(){
            if (!HasElements()) return;
            var point = transform.localPosition;

            for (int i = 0; i < _elements.Count; i++)
                _elements[i].Reposition(point);
        }

        /// <summary>
        /// Notifies child element of new size
        /// </summary>
        protected void NotifySize(){
            if (!HasElements()) return;
            var scaler = GetScaler();

            for (int i = 0; i < _elements.Count; i++)
                _elements[i].Rescale(scaler);
        }


        /// <summary>
        /// Returns: If this element contains child elements
        /// </summary>
        private bool HasElements(){
            var count = _transform.childCount;
            if (count == 0) return false;

            if (_elements.Count == count) 
                return true;

            AddElements();
            return true;
        }

        /// <summary>
        /// Adds child elements to list of elements
        /// </summary>
        private void AddElements(){
            _elements.Clear();

            for (int i = 0; i < _transform.childCount; i++){
                var child   = _transform.GetChild(i).gameObject;
                var element = child.GetComponent<UIElement>();

                if (element == null){
                    element = child.AddComponent<UIGroup>();
                } 
                
                _elements.Add(element);
            }
        }

// INSPECTOR

        /// <summary>
        /// On inspector field changed event
        /// </summary>
        private void OnValidate() => OnInspectorUpdate();

        /// <summary>
        /// Updates: gameobject transform via element transform
        /// </summary>
        protected virtual void OnInspectorUpdate(){
            _transform = transform;

            if (_transform.localPosition != _point)
                _transform.localPosition = _point;

            if (_transform.localScale != _size)
                _transform.localScale = _size;
        }
    }
}