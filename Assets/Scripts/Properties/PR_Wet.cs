using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Wet : Property {

    Resistence fireResist;
    Resistence lightningWeakness;

    public override void OnAddProperty()
    {
        fireResist = GetComponent<Attackable>().AddResistence(ElementType.FIRE, 25.0f);
        lightningWeakness = GetComponent<Attackable>().AddResistence(ElementType.LIGHTNING, -25.0f);
    }

    public override void OnRemoveProperty()
    {
        GetComponent<Attackable>().RemoveResistence(fireResist);
        GetComponent<Attackable>().RemoveResistence(lightningWeakness);
    }
}
