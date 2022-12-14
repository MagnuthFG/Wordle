using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface 
{
    [AddComponentMenu("Magnuth/Interface/UI Grid")]
    public class UIGrid : UIElement
    {
        [Header("Grid")]
        [SF] private Vector2Int _count  = Vector2Int.one;

        [Header("Position")]
        [SF] private Vector3 _centre  = Vector3.zero;
        [SF] private Vector3 _spacing = Vector3.zero;
        [Space]
        [SF] private Vector3 _evenColumnOffset   = Vector3.zero;
        [SF] private Vector3 _unevenColumnOffset = Vector3.zero;
        [Space]
        [SF] private Vector3 _evenRowOffset   = Vector3.zero;
        [SF] private Vector3 _unevenRowOffset = Vector3.zero;

        [Header("Element")]
        [SF] private Vector3    _widthHeightDepth = Vector3.one;
        [SF] private GameObject _prefab = null;

// INITIALISATION

        /// <summary>
        /// Initialises the grid
        /// </summary>
        protected virtual void Start() => BuildGrid();

// INTERFACE

        /// <summary>
        /// Instantiates the grid
        /// </summary>
        private void BuildGrid(){
            var origin = _transform.localPosition;
            var scaler = GetScaler();

            var xSize  = (_widthHeightDepth.x + _spacing.x);
            var ySize  = (_widthHeightDepth.y + _spacing.y);
            var yShift = _count.y % 2 == 0 ? ySize * 0.5f : ySize;
            
            var half   = _count / 2;
            var point  = _centre;

            for (int y = half.y + _count.y % 2; y > -half.y; y--){
            for (int x = -half.x; x < half.x + _count.x % 2; x++){
                point.x  = _centre.x + (xSize * x);
                point.y  = _centre.y + (ySize * y) - yShift;

                bool evenRow = y % 2 == 0;
                point.x += evenRow ? _evenRowOffset.x : _unevenRowOffset.x;
                point.y += evenRow ? _evenRowOffset.y : _unevenRowOffset.y;

                bool evenColumn = x % 2 == 0;
                point.x += evenColumn ? _evenColumnOffset.x : _unevenColumnOffset.x;
                point.y += evenColumn ? _evenColumnOffset.y : _unevenColumnOffset.y;

                var obj = Instantiate(_prefab, _transform);
                var ui  = obj.GetComponent<UIElement>();
                if (ui == null) continue;

                ui.Initialise(point, origin, _widthHeightDepth, scaler);
            }}
        }
    }
}