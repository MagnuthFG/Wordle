using SF = UnityEngine.SerializeField;
using UnityEngine;

namespace Magnuth.Interface
{
    [AddComponentMenu("Magnuth/Interface/UI Object"),
     RequireComponent(typeof(MeshRenderer))]
    public class UIObject : UIElement
    {
        [Header("Properties")]
        [SF] protected Color _colour = Color.white;
        protected Material _material = null;

// INITIALISATION

        /// <summary>
        /// Initialises: material instance and colour
        /// </summary>
        protected override void Awake(){
            base.Awake();
            
            var renderer = GetComponent<MeshRenderer>();
            _material = renderer.material;
            _material.color = _colour;
        }

// SETTINGS

        /// <summary>
        /// Specifies: material colour
        /// </summary>
        public void SetColour(Color colour){
            if (_material == null) return;
            _material.color = colour;
        }

        /// <summary>
        /// Specifies: material property colour
        /// </summary>
        public void SetColour(Color colour, string property){
            if (_material == null) return;
            _material.SetColor(property, colour);
        }
    }
}