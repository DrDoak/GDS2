﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkPropSteal : AttackInfo {
	public override void OnHitConfirm(GameObject other, Hitbox hb, HitResult hr) {
		//Debug.Log ("Hit Confirm with: " + other);
		if (other.GetComponent<PropertyHolder> () != null) {
			PropertyHolder other_ph = other.GetComponent<PropertyHolder> ();
			PropertyHolder m_ph = GetComponent<PropertyHolder> ();
			List<Property> pList = other_ph.GetStealableProperties ();
			foreach (Property p in pList) {
				other_ph.RemoveProperty (p);
				m_ph.AddProperty (p);
			}
		}
	}
}