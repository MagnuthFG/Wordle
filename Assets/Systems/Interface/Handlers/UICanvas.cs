using SF = UnityEngine.SerializeField;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Magnuth.Interface 
{
    [AddComponentMenu("Magnuth/Interface/UI Canvas"), DefaultExecutionOrder(-1)]
    public sealed class UICanvas : UIElement
    {
        [Header("Canvas")]
        [SF] private Vector2Int _defResolution = new Vector2Int(1920, 1080);

        private Resolution _resolution = default;
        
// INITIALISATION

        /// <summary>
        /// Initialises: Canvas elements
        /// </summary>
        protected override void Awake(){
            _resolution = Screen.currentResolution;
            _resScaler  = GetResolution();
            _sizeScaler = GetScaler();

            UpdateElements();
        }

        /// <summary>
        /// Returns: Resolution scaler
        /// </summary>
        private float GetResolution(){
            var xPercent = Mathf.InverseLerp(
                0, _defResolution.x, _resolution.width
            );
            var yPercent = Mathf.InverseLerp(
                0, _defResolution.y, _resolution.height
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

        /// <summary>
        /// Monitors current screen resolution
        /// </summary>
        private void Update(){
            _resolution = Screen.currentResolution;

            if (_resolution.width  == _defResolution.x &&
                _resolution.height == _defResolution.y)
                return;

            _resScaler = GetResolution();
            UpdateElements();
        }
    }
}