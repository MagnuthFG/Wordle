using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Wordl.Interface {
    public class UIGrid : UIElement
    {
        [Header("Grid")]
        [SF] private Vector2Int _count  = Vector2Int.one;

        [Header("Position")]
        [SF] private Vector2 _centre  = Vector2.one * 0.5f;
        [SF] private Vector2 _spacing = Vector2.zero;
        [Space]
        [SF] private Vector2 _evenColumnOffset   = Vector2.zero;
        [SF] private Vector2 _unevenColumnOffset = Vector2.zero;
        [Space]
        [SF] private Vector2 _evenRowOffset   = Vector2.zero;
        [SF] private Vector2 _unevenRowOffset = Vector2.zero;

        [Header("Element")]
        [SF] private Vector2    _size   = Vector2.one;
        [SF] private GameObject _prefab = null;

// INITIALISATION

        /// <summary>
        /// Initialises the grid
        /// </summary>
        private void Start() => BuildGrid();

// INTERFACE

        /// <summary>
        /// Instantiates the grid
        /// </summary>
        private void BuildGrid(){
            var half   = _count / 2;
            var point  = _centre;

            var size   = transform.localScale;
            var scaler = GetChildScaler(size, _tfmScaler);

            for (int y = -half.y; y < half.y + _count.y % 2; y++){
                for (int x = -half.x; x < half.x + _count.x % 2; x++){
                    point.x = _centre.x + (_size.x + _spacing.x) * x;
                    point.y = _centre.y + (_size.y + _spacing.y) * y;

                    bool evenRow = y % 2 == 0;
                    point.x += evenRow ? _evenRowOffset.x : _unevenRowOffset.x;
                    point.y += evenRow ? _evenRowOffset.y : _unevenRowOffset.y;

                    bool evenColumn = x % 2 == 0;
                    point.x += evenColumn ? _evenColumnOffset.x : _unevenColumnOffset.x;
                    point.y += evenColumn ? _evenColumnOffset.y : _unevenColumnOffset.y;

                    var obj = Instantiate(_prefab, transform);
                    obj.transform.localPosition = point;
                    obj.transform.localScale    = _size;

                    var ui = obj.GetComponent<UIElement>();
                    ui.SetScalerAndRes(scaler, _resScaler);
                }
            }
        }
    }
}