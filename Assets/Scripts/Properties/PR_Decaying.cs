using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Decaying : Property {

    float decay_damage = 0.01f;

    public override void OnCreation()
    {
        Debug.Log("PR_Decaying on create");
    }

    public override void OnUpdate()
    {
        Debug.Log("PR_Decaying on update");
        GetComponent<Attackable>().DamageObj(decay_damage);
    }

}
