using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Burner : PR_Mechanical {

	Vector2 scl = new Vector2(2.0f, 5.0f);
	Vector2 off = new Vector2(0f, 4f);
	float dmg = 20.0f;
	float stun = 0.0f;
	float hd = -0.5f;
	Vector2 kb = new Vector2(0.0f, 50.0f);
	HitboxDoT dotBox;
	HitboxMulti launchBox;
	GameObject fx;

	protected override void OnActive() {
		dotBox = GetComponent<HitboxMaker>().CreateHitboxDoT(scl, off, dmg, stun, hd, kb,true, true, ElementType.PHYSICAL);
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXBurner);
	}
	protected override void OnDisable() {
		if (dotBox != null) {
			Destroy (dotBox);
			GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
		}
	}
}
