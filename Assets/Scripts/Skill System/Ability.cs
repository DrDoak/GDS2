using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    public static GameObject Player;
    public string AbilityName;
    public AbilityType AbilityClassification;

    private int _mtier = 1;

    public abstract void UseAbility();
}

public enum AbilityType
{
    STEALTH, MAGIC, COMBAT
}