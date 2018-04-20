using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Wooden : Property {

	public float m_flameDamage = 0f;
	const float FLAME_THREASHOLD = 8.0f;

	public override void OnAddProperty()
	{
		m_flameDamage = 0f;
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight, GetComponent<BasicMovement> ().TimeToJumpApex * 1.5f);
		}
	}
	public override void OnRemoveProperty()
	{
		m_flameDamage = 0f;
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight, GetComponent<BasicMovement> ().TimeToJumpApex / 1.5f);
		}
	}
	public override void OnUpdate() { 
		//m_flameDamage -= Time.deltaTime;
	}

	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (!GetComponent<PropertyHolder> ().HasProperty ("Flaming")) {
			if (hb.Element == ElementType.FIRE) {
				HitboxDoT hd = hb as HitboxDoT;
				if (hd != null) {
					m_flameDamage += (Time.deltaTime * hb.Damage);
				} else {
					m_flameDamage += hb.Damage;
				}
				if (m_flameDamage >= FLAME_THREASHOLD) {
					Debug.Log ("Adding flame property");
					GetComponent<PropertyHolder> ().AddProperty ("PR_Flaming");
				}
			}
		}
	}
}
