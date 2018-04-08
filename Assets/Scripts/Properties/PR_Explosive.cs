using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Explosive : Property {

    Vector2 scl = new Vector2(5.0f, 5.0f);
    Vector2 off = new Vector2(0f, 0f);
    float dmg = 25.0f;
    float stun = 1.0f;
    float hd = 1.0f;
    Vector2 kb = new Vector2(3.0f, 3.0f);

    public override void OnCreation()
    {
        Debug.Log("PR_Explosive on create");
    }

    public override void OnHit()
    {
        Debug.Log("PR_Explosive on hit (kaboom)");
        GetComponent<HitboxMaker>().CreateHitbox(scl, off, dmg, stun, hd, kb);
    }

}
