using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Wordl.Interface
{
    [AddComponentMenu("Magnuth/Interface/UIObject"),
     RequireComponent(typeof(MeshRenderer))]
    public class UIObject : UIElement
    {
        [Header("Properties")]
        [SF] protected Color _colour = Color.white;
        protected Material _material = null;

// INITIALISATION

        /// <summary>
        /// Initialises: Material instance and colour
        /// </summary>
        protected override void Awake(){
            base.Awake();
            
            var renderer = GetComponent<MeshRenderer>();
            _material = renderer.material;
            _material.color = _colour;
        }

// SETTINGS

        /// <summary>
        /// Specifies: Material colour
        /// </summary>
        public void SetColour(Color colour){
            _material.color = colour;
        }
    }
}