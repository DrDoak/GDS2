using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkLine : AttackInfo {

	public float range = 0f;
	public Vector2 direction = new Vector2 (0f, 0f);


	protected override void OnAttack() {
		base.OnAttack ();
		FactionType fac = GetComponent<Attackable> ().Faction;

		// GetComponent<HitboxMaker> ().AddHitType (HitType);
		//LineHitbox lbox = 	GetComponent<HitboxMaker>().createLineHB (range, realD, realOff, Damage, Stun, HitboxDuration, realKB,fac, true);
		LineHitbox lbox = GetComponent<HitboxMaker>().createLineHB(range, direction, HitboxOffset, Damage, Stun, HitboxDuration, Knockback, true,Element);
		lbox.Stun = Stun;
	}
}
