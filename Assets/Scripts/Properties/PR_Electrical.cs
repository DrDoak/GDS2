using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Electrical : Property {

	public override void OnHitboxCreate (Hitbox hitboxCreated) {
		hitboxCreated.Element = ElementType.LIGHTNING;
		hitboxCreated.Damage = hitboxCreated.Damage * 1.2f;
	}
}
