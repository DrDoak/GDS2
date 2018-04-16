using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkAbilityHitTrigger : AtkDash {

	public Ability mAbility;
	// Use this for initialization
	public override void OnHitConfirm(GameObject other, Hitbox hb, HitResult hr) {
		if (mAbility != null) {
			mAbility.SetTarget (other);
			mAbility.UseAbility ();
		} else {
			Debug.Log ("Ability not assigned to " + GetType ());
		}
	}
}
