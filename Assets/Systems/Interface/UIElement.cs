using SF = UnityEngine.SerializeField;
using System.Collections.Generic;
using UnityEngine;

namespace Wordl.Interface {
    public abstract class UIElement : MonoBehaviour
    {
        [Header("Transform")]
        [SF] protected Vector3 _point = Vector3.zero;
        [SF] protected Vector3 _size  = Vector3.one;

        protected float   _resScaler  = 1f;
        protected Vector3 _sizeScaler = Vector3.one;
        protected List<UIElement> _elements = new();

// INITIALISATION

        /// <summary>
        /// Initialises: element size and position
        /// </summary>
        protected virtual void Awake(){
            _point = transform.localPosition;
            _size  = transform.localScale;
        }

        /// <summary>
        /// Initialises: element transform and scalers
        /// </summary>
        /// <param name="position">Local position</param>
        /// <param name="size">Local scale</param>
        /// <param name="scaler">Recursive transform size multiplier</param>
        /// <param name="resolution">Min resolution percentage multiplier</param>
        public void Initialise(Vector3 position, Vector3 size, Vector3 scaler, float resolution){
            _point      = position;
            _size       = size;
            _resScaler  = resolution;
            _sizeScaler = scaler;

            // Must set unscaled position first
            transform.localPosition = _point;
            transform.localScale    = _size;

            // Then set scaled position
            SetTransform(_point, _size);
        }

// TRANSFORM

        /// <summary>
        /// Sets: element position
        /// </summary>
        /// <param name="position">Local position</param>
        public void SetPosition(Vector3 position){
            _point = position;
            transform.localPosition = _point;
        }

        /// <summary>
        /// Sets: element size
        /// </summary>
        /// <param name="size">Local scale</param>
        public void SetSize(Vector3 size){
            _size = Rescale(size);
            transform.localScale = _size;

            UpdateElements();
        }

        /// <summary>
        /// Sets: element position and size
        /// </summary>
        /// <param name="position">Local position</param>
        /// <param name="size">Local scale</param>
        public void SetTransform(Vector3 position, Vector3 size){
            _point = Rescale(position);
            _size  = Rescale(size);

            transform.localPosition = _point;
            transform.localScale    = _size;

            UpdateElements();
        }

        /// <summary>
        /// Returns: value rescaled by resolution
        /// <br>Independent of parent size</br>
        /// </summary>
        /// <param name="value">Default value</param>
        private Vector3 Rescale(Vector3 value){
            value.x = value.x * _resScaler / _sizeScaler.x;
            value.y = value.y * _resScaler / _sizeScaler.y;
            value.z = value.z * _resScaler / _sizeScaler.z;
            return value;
        }

// SCALING

        /// <summary>
        /// Initialises: transform and resolution scalers
        /// </summary>
        /// <param name="scaler">Recursive transform size multiplier</param>
        /// <param name="resolution">Min resolution percentage multiplier</param>
        public void SetScalerAndRes(Vector3 scaler, float resolution){
            _resScaler = resolution;
            _sizeScaler = scaler;

            _point = Rescale(_point);
            _size  = Rescale(_size);

            transform.localPosition = _point;
            transform.localScale    = _size;

            UpdateElements();
        }

        /// <summary>
        /// Specifies: Min resolution percentage scaler
        /// </summary>
        /// <param name="resolution">Min resolution percentage multiplier</param>
        public void SetResolution(float resolution){
            _resScaler = resolution;

            _size = Rescale(_size);
            transform.localScale = _size;

            UpdateElements();
        }

        /// <summary>
        /// Specifies: Recursive transform scaler
        /// </summary>
        /// <param name="scaler">Recursive transform size multiplier</param>
        public void SetScaler(Vector3 scaler){
            _sizeScaler = scaler;

            _size = Rescale(_size);
            transform.localScale = _size;

            UpdateElements();
        }

        /// <summary>
        /// Returns: Child transform scaler
        /// </summary>
        public Vector3 GetChildScaler(){
            var scaler = _sizeScaler;

            scaler.x = _size.x <= _sizeScaler.x ?
                _size.x * _sizeScaler.x :
                _size.x / _sizeScaler.x;

            scaler.y = _size.y <= _sizeScaler.y ?
                _size.y * _sizeScaler.y :
                _size.y / _sizeScaler.y;

            return scaler;
        }

// UPDATING

        /// <summary>
        /// Updates child element scalers
        /// </summary>
        private void UpdateElements(){
            if (!HasElements()) return;

            var sizeScaler = GetChildScaler();

            for (int i = 0; i < _elements.Count; i++){
                _elements[i].SetScalerAndRes(
                    sizeScaler, _resScaler
                );
            }
        }

        /// <summary>
        /// Returns: If this element contains child elements
        /// </summary>
        private bool HasElements(){
            var count = transform.childCount;
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

            for (int i = 0; i < transform.childCount; i++){
                var child   = transform.GetChild(i).gameObject;
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
        private void OnValidate(){
            OnInspectorUpdate();
        }

        /// <summary>
        /// Updates gameobject transform via element transform
        /// </summary>
        protected virtual void OnInspectorUpdate(){
            if (transform.localPosition != _point)
                transform.localPosition = _point;

            if (transform.localScale != _size)
                transform.localScale = _size;
        }
    }
}