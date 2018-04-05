using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkLine : AttackInfo {

	public float range = 0f;
	public Vector2 direction = new Vector2 (0f, 0f);


	/*public override void OnAttack() {
		base.OnAttack ();
		Vector2 realKB = Knockback;
		Vector2 realOff = HitboxOffset;
		Vector2 realD = direction;
		string fac = GetComponent<Attackable> ().Faction;
		if (GetComponent<PhysicsSS> ().FacingLeft) {
			realKB = new Vector2 (-Knockback.x, Knockback.y);
			realOff = new Vector2 (-HitboxOffset.x, HitboxOffset.y);
			realD = new Vector2 (-direction.x, direction.y);
		}
		GetComponent<HitboxMaker> ().AddHitType (HitType);
		LineHitbox lbox = 	GetComponent<HitboxMaker>().createLineHB (range, realD, realOff, Damage, Stun, HitboxDuration, realKB,fac, true);
		lbox.Stun = Stun;
	}*/
}
