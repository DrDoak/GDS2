﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Fan : PR_Mechanical {

	Vector2 scl = new Vector2(10.0f, 2.0f);
	Vector2 off = new Vector2(4f, 0f);
	float dmg = 0.0f;
	float stun = 0.0f;
	float hd = -0.5f;
	Vector2 kb = new Vector2(120.0f, 0.0f);
	HitboxDoT dotBox;
	HitboxMulti launchBox;
	GameObject fx;

	Vector2 scl2 = new Vector2(1.0f, 3.0f);
	Vector2 off2 = new Vector2(-1f, 0f);
	Vector2 kb2 = new Vector2(20.0f, 0.0f);

	protected override void OnActive() {
		dotBox = GetComponent<HitboxMaker>().CreateHitboxDoT(scl, off, dmg, stun, hd, kb,true, true, ElementType.PHYSICAL);
		launchBox = GetComponent<HitboxMaker>().CreateHitboxMulti(scl2, off2, dmg, stun, hd, kb2,true, true, ElementType.PHYSICAL);
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXFan);
	}
	protected override void OnDisable() {
		if (dotBox != null) {
			Destroy (dotBox);
			Destroy (launchBox);
			GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
		}
	}
}