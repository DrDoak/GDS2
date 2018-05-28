﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Passive_Bleed :Ability {

    private bool selected = false;
    private int percentage = 10;

    public override void UseAbility()
    {
        //Percentage-based logic
        int r = Random.Range(0, 100);
        if (r > percentage) return;

        //Add Bleeding Property
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
