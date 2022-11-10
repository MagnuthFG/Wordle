using UnityEngine;

namespace Wordl.Interface {
    public abstract class UIElement : MonoBehaviour
    {
        protected float   _resScaler = 1f;
        protected Vector2 _tfmScaler = Vector2.one;

// TRANSFORM

        /// <summary>
        /// Specifies: Element size
        /// </summary>
        public void SetSize(Vector2 size){
            transform.localScale = Rescale(size);

            // Tell children of new size
        }

        /// <summary>
        /// Specifies: Element position and size
        /// </summary>
        public void SetTransform(Vector2 position, Vector2 size){
            transform.localPosition = Rescale(position);
            transform.localScale    = Rescale(size);

            // Tell children of new size
        }

        /// <summary>
        /// Returns: Value rescaled by resolution
        /// <br>Independent of parents scale</br>
        /// </summary>
        /// <param name="value">Default value</param>
        protected Vector2 Rescale(Vector2 value){
            return value * _resScaler / _tfmScaler;
        }

// SCALING

        /// <summary>
        /// Initialises: Transform and resolution scalers
        /// </summary>
        /// <param name="scaler">Recursive transform scaler</param>
        /// <param name="resolution">Min resolution percentage scaler</param>
        public void SetScalerAndRes(Vector2 scaler, float resolution){
            _resScaler = resolution;
            _tfmScaler = scaler;

            var scale    = Rescale(transform.localScale);
            var position = Rescale(transform.localPosition);
            var rescaler = GetChildScaler(scale, scaler);

            transform.localScale    = scale;
            transform.localPosition = position;

            for (int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);

                var element = child.GetComponent<UIElement>();
                if (element == null) continue;

                element.SetScalerAndRes(rescaler, resolution);
            }
        }

        /// <summary>
        /// Specifies: Min resolution percentage scaler
        /// </summary>
        public void SetResolution(float resolution){
            _resScaler = resolution;

            var size = Rescale(transform.localScale);
            transform.localScale = size;

            for (int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);

                var element = child.GetComponent<UIElement>();
                if (element == null) continue;

                element.SetResolution(_resScaler);
            }
        }

        /// <summary>
        /// Specifies: Recursive transform scaler
        /// </summary>
        public void SetScaler(Vector2 scaler){
            _tfmScaler = scaler;

            var size    = Rescale(transform.localScale);
            var rescale = GetChildScaler(size, scaler);
            transform.localScale = size;

            for (int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);

                var element = child.GetComponent<UIElement>();
                if (element == null) continue;

                element.SetScaler(rescale);
            }
        }


        /// <summary>
        /// Returns: Child transform scaler
        /// </summary>
        /// <param name="size">Current transform scale</param>
        /// <param name="scaler">Current transform scaler</param>
        protected Vector2 GetChildScaler(Vector2 size, Vector2 scaler){
            scaler.x = size.x <= scaler.x ?
                size.x * scaler.x :
                size.x / scaler.x;

            scaler.y = size.y <= scaler.y ?
                size.y * scaler.y :
                size.y / scaler.y;

            //Debug.Log($"Scaler {scaler}");
            return scaler;
        }
    }
}