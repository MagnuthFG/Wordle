using SF = UnityEngine.SerializeField;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Wordl.Interface {
	public class UIButton : UIObject, IPointerClickHandler
	{
		[Header("Button")]
		[SF] protected GameObject _target = null;
		protected IButtonTarget _btnTarget = null;
		
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
		/// Event: on mouse clicked
		/// </summary>
		public virtual void OnPointerClick(PointerEventData data){
	        _btnTarget?.OnClicked();
	    }
	}
}