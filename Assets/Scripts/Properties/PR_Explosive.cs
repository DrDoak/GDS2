using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Explosive : Property {

    Vector2 scl = new Vector2(3.5f, 3.5f);
    Vector2 off = new Vector2(0f, 0f);
    float dmg = 50.0f;
    float stun = 1.0f;
    float hd = 0.5f;
    Vector2 kb = new Vector2(20.0f, 10.0f);
	float oldDeathTime = 0.0f;
	Resistence fireVulnerability;
		
	public override void OnAddProperty()
	{
		oldDeathTime = GetComponent<Attackable> ().DeathTime;
		fireVulnerability = GetComponent<Attackable>().AddResistence(ElementType.FIRE, -100.0f, false, false);

		GetComponent<Attackable>().DeathTime = 0.0f;
	}

	public override void OnRemoveProperty()
	{
		GetComponent<Attackable>().DeathTime = oldDeathTime;
		GetComponent<Attackable>().RemoveResistence(fireVulnerability);
	}


/*	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (hb.HasElement(ElementType.FIRE)) {
			HitboxDoT hd = hb as HitboxDoT;
			if (hd != null) {
				GetComponent<Attackable> ().DamageObj (hb.Damage * Time.deltaTime);
			} else {
				GetComponent<Attackable> ().DamageObj (hb.Damage);
			}
		}
	}*/
    public override void OnDeath()
    {
		GetComponent<HitboxMaker>().CreateHitbox(scl, off, dmg, stun, hd, kb, false,false,ElementType.FIRE);
		Instantiate(FindObjectOfType<GameManager>().FXExplosionPrefab, transform.position, transform.rotation);
    }
}
