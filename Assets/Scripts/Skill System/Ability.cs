using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public static GameObject Player;
    public static AbilityManager Manager;

    public string AbilityName;
    public AbilityType AbilityClassification;
    public string AnimStateName;
    public bool Ultimate = false;
    public GameObject Creator;

    protected GameObject Target;
    protected List<Property> _mPropertyToTransfer;
    protected List<Ability> _mSelected;
    protected List<Property> _mPropertiesToKeep;


	//I added this for when you want to use Melee Hitboxes
	//instead of Distance (circle) hitboxes. 
	public bool UseAttackHitbox = false;

    protected int _mtier = 1;

    public abstract void UseAbility();

    public void Awake()
    {
        ClearLists();
    }

    public virtual void TriggerAnimation()
    {
        //TriggerAnimation using the string AnimStateName
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
    ENVRIONMENTAL, ELEMENTAL, COMBAT, SPECIAL
}