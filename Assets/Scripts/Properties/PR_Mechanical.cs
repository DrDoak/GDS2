﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Mechanical : Property {

	private bool m_isActive = false;
	private float m_activeTime = 0.0f;

	protected virtual void SetActive(bool active) {
		Debug.Log ("Setting door active");
		if (m_isActive != active) {
			m_isActive = active;
			if (m_isActive) {
				GameObject.Instantiate (FXHit.Instance.FXHitLightning, transform.position, Quaternion.identity);
				OnActive ();
			} else
				OnDisable ();
		}
	}
	protected virtual void OnActive() {
	}
	protected virtual void OnDisable() {
	}
	public override void OnUpdate ()
	{
		//base.OnUpdate ();
		Debug.Log (GetComponent<PropertyHolder> ().HasProperty ("Electrical"));
		if (GetComponent<PropertyHolder> ().HasProperty ("Electrical")) {
			SetActive (true);
		} else {
			if (m_activeTime > 0f) {
				m_activeTime -= Time.deltaTime;
				SetActive (true);
			} else {
				SetActive (false);
			}
		}
	}
	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (hb.HasElement(ElementType.LIGHTNING)) {
			m_activeTime = 3f;
		}
	}
}