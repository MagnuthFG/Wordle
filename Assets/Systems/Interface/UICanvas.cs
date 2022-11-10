using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Wordl.Interface 
{
    [DefaultExecutionOrder(-1)]
    public class UICanvas : UIElement
    {
        [SF] private Vector2Int _defResolution = new Vector2Int(1920, 1080);

// INITIALISATION

        /// <summary>
        /// Initialises: Canvas elements
        /// </summary>
        private void Awake(){
            _resScaler = GetResolution();
            _tfmScaler = GetScaler();

            for (int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);

                var element = child.GetComponent<UIElement>();
                if (element == null) continue;

                element.SetScalerAndRes(_tfmScaler, _resScaler);
            }
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
        private Vector2 GetScaler(){
            return transform.localScale;
        }
    }
}