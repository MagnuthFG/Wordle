using SF = UnityEngine.SerializeField;
using UnityEngine.EventSystems;
using UnityEngine;

// https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IPointerClickHandler.html

// REMEMBER add restricyion so only one component of this type
// to UIElement so that UI game objects can only have one UI transform

namespace Wordl.Interface {
	public class UIButton : UIObject, IPointerClickHandler
	{
		[Header("Button")]
		[SF] protected MonoBehaviour _target = null;
		protected IButtonTarget _btnTarget = null;
		
// INITIALISATION

		/// <summary>
		/// Initialises: Button target
		/// </summary>
		protected override void Awake(){
			_btnTarget = (IButtonTarget)_target;
		}
		
// INTERFACE

		/// <summary>
		/// 
		/// </summary>
		public void OnPointerClick(PointerEventData data){
	        _btnTarget?.OnClicked();
	    }
	}
}