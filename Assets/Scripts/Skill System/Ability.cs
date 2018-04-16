﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability:ScriptableObject {

    public static GameObject Player;
    public static AbilityManager Manager;
    public string AbilityName;
    public AbilityType AbilityClassification;
    public string AnimStateName;
    protected GameObject Target;
    protected Property _mPropertyToTransfer;
    protected Ability _mSelected;

    private int _mtier = 1;

    public abstract void UseAbility();

    public virtual void TriggerAnimation()
    {
        //TriggerAnimation using the string AnimStateName
    }

    public void UpdateProperty(Property p)
    {
        _mPropertyToTransfer = p;
    }

    public void UpdateAbility(Ability a)
    {
        _mSelected = a;
    }

    public void SetTarget(GameObject g)
    {
        Target = g;
    }

}

public enum AbilityType
{
    STEALTH, MAGIC, COMBAT, SPECIAL
}