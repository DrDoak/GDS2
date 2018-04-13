using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Explosive : Property {

    Vector2 scl = new Vector2(5.0f, 5.0f);
    Vector2 off = new Vector2(0f, 0f);
    float dmg = 50.0f;
    float stun = 1.0f;
    float hd = 0.5f;
    Vector2 kb = new Vector2(25.0f, 60.0f);

    public override void OnCreation()
    {
        Debug.Log("PR_Explosive on create");

    }

    public override void OnHit()
    {

    }

    public override void OnDeath()
    {
        Debug.Log("PR_Explosive on death (kaboom)");
		GetComponent<HitboxMaker>().CreateHitbox(scl, off, dmg, stun, hd, kb, false,false);
		Instantiate(FindObjectOfType<GameManager>().ExplosionPrefab, transform.position, transform.rotation);
    }
}
