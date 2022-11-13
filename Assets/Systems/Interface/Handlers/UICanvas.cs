using SF = UnityEngine.SerializeField;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Wordl.Interface 
{
    [DefaultExecutionOrder(-1)]
    public class UICanvas : UIElement
    {
        [Header("Canvas")]
        [SF] private Vector2Int _defResolution = new Vector2Int(1920, 1080);

// INITIALISATION

        /// <summary>
        /// Initialises: Canvas elements
        /// </summary>
        protected override void Awake(){
            _resScaler  = GetResolution();
            _sizeScaler = GetScaler();

            UpdateElements();
        }

        /// <summary>
        /// Returns: Resolution scaler
        /// </summary>
        private float GetResolution(){
            var current = Screen.currentResolution;
            
            var xPercent = Mathf.InverseLerp(
                0, _defResolution.x, current.width
            );
            var yPercent = Mathf.InverseLerp(
                0, _defResolution.y, current.height
            );

            return Mathf.Min(xPercent, yPercent);
        }

        /// <summary>
        /// Returns: Transform scaler
        /// </summary>
        private Vector3 GetScaler(){
            return transform.localScale;
        }

// RESOLUTION


    }
}