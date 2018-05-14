using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_NanoSword : Ability {

    private bool selected;

    new void Awake()
    {
        base.Awake();
        selected = false;
    }

    public override void UseAbility()
    {
        Debug.Log("Nano Sword triggered!");
        
    }

    public override void Select()
    {
        if (!selected)
            EventManager.MeleeSpecialEvent += UseAbility;
        else
            EventManager.MeleeSpecialEvent -= UseAbility;

        selected = !selected;
    }
}
