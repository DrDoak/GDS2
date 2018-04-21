﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability:ScriptableObject {

    public static GameObject Player;
    public static AbilityManager Manager;
    public string AbilityName;
    public AbilityType AbilityClassification;
    public string AnimStateName;
    protected GameObject Target;
    protected List<Property> _mPropertyToTransfer;
    protected List<Ability> _mSelected;
    protected List<Property> _mPropertiesToKeep;

	//I added this for when you want to use Melee Hitboxes
	//instead of Distance (circle) hitboxes. 
	public bool UseAttackHitbox = false;

    private int _mtier = 1;

    public abstract void UseAbility();

    void Awake()
    {
        ClearLists();
    }

    public virtual void TriggerAnimation()
    {
        //TriggerAnimation using the string AnimStateName
    }
    /// <summary>
    /// Old functionality requirement. Use SetTransferLists instead
    /// </summary>
    /// <param name="p"></param>
    public void UpdateProperty(Property p)
    {
        _mPropertyToTransfer.Add(p);
		Debug.Log ("Updated property: " + p);
    }

    /// <summary>
    /// Old functionality requirement. Use SetTransferLists instead
    /// </summary>
    /// <param name="a"></param>
    public void UpdateAbility(Ability a)
    {
        _mSelected.Add(a);
    }
    
    public void SetTarget(GameObject g)
    {
        Target = g;
    }

    protected void ClearLists()
    {
        _mPropertyToTransfer = new List<Property>();
        _mPropertiesToKeep = new List<Property>();
        _mSelected = new List<Ability>();
    }

    /// <summary>
    /// Sets the appropriate Lists for transferring between player and Target
    /// </summary>
    /// <param name="forPlayerProps"></param>
    /// <param name="forPlayerAbs"></param>
    /// <param name="forTargetProps"></param>
    public void SetTransferLists(List<Property> forPlayerProps, List<Ability> forPlayerAbs, List<Property> forTargetProps)
    {
        _mPropertiesToKeep = forPlayerProps;
        _mPropertyToTransfer = forTargetProps;
        _mSelected = forPlayerAbs;
    }

}

public enum AbilityType
{
    STEALTH, MAGIC, COMBAT, SPECIAL
}