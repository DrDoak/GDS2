using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Parasite : Property {

    float parasite_damage = 1f;

    public override void OnHitConfirm(Hitbox myHitbox, GameObject objectHit, HitResult hr)
    {
        objectHit.GetComponent<Attackable>().DamageObj(myHitbox.Damage);  // double damage
    }

    public override void OnUpdate()
    {
        GetComponent<Attackable>().DamageObj(parasite_damage);
    }
}
