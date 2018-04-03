/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
	//This script is intended for all pushable/draggable objects
	private Interactable m_triggerArea;
	private bool m_firstTrigger;
	Vector2 offset;
	Vector3 char_offset;
	public bool FacingLeft;
	BasicMovement m_basicMovement;
	public bool Draggable;

	// Use this for initialization
	void Start()
	{
		//trigger_area = gameObject.GetComponentInChildren<Interactable>();
		m_firstTrigger = true;
	}
	
	// Update is called once per frame
	void Update()
	{

		foreach (Interactable area in gameObject.GetComponentsInChildren<Interactable>())
		{
			if (area.PressTrigger)
			{
				//If one of the trigger areas is activated by the interactor assign it to trigger_area
				m_triggerArea = area;
				m_basicMovement = m_triggerArea.Actor.gameObject.GetComponent<BasicMovement>();
			}
		}

		// Assign the right direction 
		if (m_triggerArea.gameObject.GetComponent<BoxCollider2D>().offset.x > 0)
			FacingLeft = true;
		if (m_triggerArea.gameObject.GetComponent<BoxCollider2D>().offset.x < 0)
			FacingLeft = false;

		//Update the basic movement script
		m_basicMovement.DirPush = FacingDirection;
		m_basicMovement.CanDrag = Draggable;
		m_basicMovement.IsDragging = true;

		//Run when the player presses the interaction key
		if (m_triggerArea.PressTrigger && m_firstTrigger)
		{

			offset = m_triggerArea.gameObject.GetComponent<Collider2D>().offset;
			//Because of the perspective, the character has to be slightly futher away when pushing the block down
			if (offset.y > 0)
				char_offset = new Vector3(offset.x * 2.5f, offset.y * 2.7f);
			else
				char_offset = new Vector3(offset.x * 2.5f, offset.y * 1f);
            
			m_triggerArea.Actor.transform.position = transform.position + char_offset;
			m_firstTrigger = false;
		}

		if (m_triggerArea.PressTrigger)
		{
			transform.position = -char_offset + m_triggerArea.Actor.transform.position;
		}
		else
		{
			m_firstTrigger = true;
			m_basicMovement.IsDragging = false;
		}
	}
}
*/