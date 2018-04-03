using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	
	public Interactor Actor;
	public string InteractionString;
	public bool HoldTrigger;
	public bool PressTrigger;

	void Start()
	{
		Actor = null;
		//Will become true if the interactor presses/holds the interaction key while in this interactable's area
		HoldTrigger = false;
		PressTrigger = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (Actor = collision.gameObject.GetComponent<Interactor>())
			Actor.PromptedInteraction = this;
	}
}
