using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    public static GameObject Player;
    public static GameObject Manager;
    public string AbilityName;
    public AbilityType AbilityClassification;
    public string AnimStateName;

    private int _mtier = 1;

    public abstract void UseAbility();

    public virtual void TriggerAnimation()
    {
        //TriggerAnimation using the string AnimStateName
    }

}

public enum AbilityType
{
    STEALTH, MAGIC, COMBAT, SPECIAL
}