using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Explosive : Property {

    Vector2 scl = new Vector2(7.0f, 7.0f);
    Vector2 off = new Vector2(0f, 0f);
    float dmg = 50.0f;
    float stun = 1.0f;
    float hd = 0.5f;
    Vector2 kb = new Vector2(25.0f, 60.0f);

    public override void OnCreation()
    {

    }

	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (hb.Element == ElementType.FIRE) {
			//Debug.Log ("Additional FIre damage");
			GetComponent<Attackable> ().DamageObj (hb.Damage * 2f);
		}
	}
    public override void OnDeath()
    {
		GetComponent<HitboxMaker>().CreateHitbox(scl, off, dmg, stun, hd, kb, false,false,ElementType.FIRE);
		Instantiate(FindObjectOfType<GameManager>().FXExplosionPrefab, transform.position, transform.rotation);
    }
}
