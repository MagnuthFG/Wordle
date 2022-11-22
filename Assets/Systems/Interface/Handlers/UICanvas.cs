using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface 
{
    [AddComponentMenu("Magnuth/Interface/UI Canvas"), DefaultExecutionOrder(-1)]
    public sealed class UICanvas : UIElement
    {
        [Header("Canvas")]
        [SF] private Camera _uiCamera = null;
        [SF] private Vector2Int _defResolution = new Vector2Int(1920, 1080);

        private Vector2 _current    = default;
        private Vector2 _resolution = default;
        
// INITIALISATION

        /// <summary>
        /// Initialises: canvas elements
        /// </summary>
        protected override void Awake(){
            base.Awake();
            UpdatedResolution();
            ChangeSize();
        }

// SETTINGS

        /// <summary>
        /// Changes the current canvas size
        /// </summary>
        private void ChangeSize(){
            transform.localScale = GetSize();
            NotifySize();
        }

        /// <summary>
        /// Returns the current canvas size
        /// </summary>
        private Vector3 GetSize(){
            return new Vector3(
                _resolution.x / _defResolution.x,
                _resolution.y / _defResolution.y,
                1f
            );
        }

// RESOLUTION

        /// <summary>
        /// Monitors current screen resolution
        /// </summary>
        private void LateUpdate(){
            if (UpdatedResolution()){ 
                ChangeSize();
            }
        }

        /// <summary>
        /// Returns if current resolution was updated
        /// </summary>
        private bool UpdatedResolution(){
            //_current.x = Screen.width;
            //_current.y = Screen.height;
            _current.x = 1920 * 2;
            _current.y = 1080;

            if (!ChangedResolution()) 
                return false;

            _resolution = _current;
            return true;
        }

        /// <summary>
        /// Returns if resolution has changed
        /// </summary>
        private bool ChangedResolution(){
            return _current.x != _resolution.x ||
                   _current.y != _resolution.y;
        }
    }
}