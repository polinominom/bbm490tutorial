using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour 
{
	public delegate void OnActionPress(GameObject unit, bool state);

	public event OnActionPress OnPress;
	private EventTrigger eventTrigger;

	void Start()
	{
		eventTrigger = GetComponent<EventTrigger> ();
		AddEventTrigger (OnPointDown, EventTriggerType.PointerDown);
		AddEventTrigger (OnPointUp, EventTriggerType.PointerUp);

	}


	void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
	{
		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent ();
		trigger.AddListener ((eventData) => action ());

		EventTrigger.Entry entry = new EventTrigger.Entry () {
			callback = trigger,
			eventID = triggerType
		};
	
		eventTrigger.triggers.Add (entry);
	}

	void OnPointDown()
	{
		if (OnPress != null) 
		{
			OnPress (gameObject, true);
		}
	}

	void OnPointUp()
	{
		if (OnPress != null) 
		{
			OnPress (gameObject, false);
		}		
	}
}
