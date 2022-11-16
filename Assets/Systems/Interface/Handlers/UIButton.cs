using SF = UnityEngine.SerializeField;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Magnuth.Interface
{
	[AddComponentMenu("Magnuth/Interface/UI Button"), 
     RequireComponent(typeof(BoxCollider))]
	public class UIButton : UIObject,
	IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SF] protected Color _highlighted = Color.white;
        [SF] protected Color _pressed	  = Color.white;

        [Header("Button")]
		[SF] protected GameObject _target = null;
		protected IButtonTarget _btnTarget = null;

		protected bool _hovering = false;

// INITIALISATION

		/// <summary>
		/// Initialises: button target
		/// </summary>
		protected override void Awake(){
			base.Awake();
            SetTarget(_target);
        }
		
// SETTINGS

		/// <summary>
		/// Specifies: button target
		/// </summary>
		public void SetTarget(GameObject target){
			if (target == null) return;

			_target	   = target;
			_btnTarget = target.GetComponent<IButtonTarget>();
        }

// INTERFACE

        /// <summary>
        /// Event: on mouse over
        /// </summary>
        public virtual void OnPointerEnter(PointerEventData eventData){
            SetColour(_highlighted);
			_hovering = true;
		}

        /// <summary>
        /// Event: on mouse exited
        /// </summary>
        public virtual void OnPointerExit(PointerEventData eventData){
            SetColour(_colour);
            _hovering = false;
        }

        /// <summary>
        /// Event: on mouse clicked
        /// </summary>
        public virtual void OnPointerDown(PointerEventData eventData){
			SetColour(_pressed);
		}

        /// <summary>
        /// Event: on mouse released
        /// </summary>
        public virtual void OnPointerUp(PointerEventData eventData){
            SetColour(_hovering ? _highlighted : _colour);
            _btnTarget?.OnClicked();
        }
	}
}