﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Transfer : Ability {
    private const float DISTANCE_ADDITION = 2f;

    protected static Ab_Transfer Instance;

    private List<Property> _mPlayerProps;
    private List<Property> _mEnemyProps;
    protected bool _mTriggered = false;

    //Upgrade data
    public static bool InfectUpgrade;
    public static float _maxDistance = 2f;
    public static int _maxTransfers = 1;

    public int upgradeIndex = 0;


	protected void Awake()
    {
        base.Awake();
        UseAttackHitbox = true;
        InfectUpgrade = false;
        Instance = this;
		AbilityClassification = AbilityType.SPECIAL;
        upgradeIndex = 0;
        Player.GetComponent<PropertyHolder>().NumTransfers = _maxTransfers;
    }

    public override void UseAbility()
    {
        if (Target == null)
        {
            AtkAbilityHitTrigger at = (AtkAbilityHitTrigger)Player.GetComponent<Fighter>().TryAttack("take");
            if (at != null)
                at.mAbility = this;
        }
        else
        {
			if (Target.GetComponent<PropertyHolder> () == null) {
				Target = null;
				return;
			}
            GetPlayerProperties();
            GetTargetProperties();
            if (!_mTriggered)
                DisplayPropertyUI();
            else
                TransferProperty();
			Target = null;
        }
    }

    public override void Upgrade()
    {
        switch (upgradeIndex)
        {
            case 0:
                UpgradeToInfect();
                break;
            case 1:
            case 3:
                UpgradeMaxDistance();
                break;
            case 2:
            case 4:
                UpgradeNumTransfers();
                break;
        }
        upgradeIndex++;
    }

    protected void UpgradeToInfect()
    {
        InfectUpgrade = true;
    }

    protected void UpgradeMaxDistance()
    {
        _maxDistance += DISTANCE_ADDITION;
    }

    protected void UpgradeNumTransfers()
    {
        _maxTransfers++;
        //THIS DOES NOTHING BUT WHY
        Player.GetComponent<PropertyHolder>().NumTransfers = _maxTransfers;
    }

    private void GetPlayerProperties()
    {
        _mPlayerProps = Player.GetComponent<PropertyHolder>().GetVisibleProperties();
    }

    private void GetTargetProperties()
    {
        _mEnemyProps = Target.GetComponent<PropertyHolder>().GetVisibleProperties();
    }

    private void DisplayPropertyUI()
    {
        GUIHandler.SetAbility(this);
        GUIHandler.CreateTransferMenu(Player.GetComponent<PropertyHolder>(), Target.GetComponent<PropertyHolder>());
    }

    private void CheckRemovals()
    {
        List<Property> toRemove = new List<Property>();
        foreach (Property p in _mPropertyToTransfer)
            if (_mEnemyProps.Contains(p))
                toRemove.Add(p);

        foreach (Property p in toRemove)
            _mPropertyToTransfer.Remove(p);

        toRemove = new List<Property>();
        foreach (Property p in _mPropertiesToKeep)
            if (_mPlayerProps.Contains(p))
                toRemove.Add(p);

        foreach (Property p in toRemove)
            _mPropertiesToKeep.Remove(p);
    }
    /// <summary>
    /// TransferProperty transfers both properties and skills between player and target
    /// </summary>
    protected virtual void TransferProperty()
    {
        PauseGame.Resume();
        CheckRemovals();

        foreach (Property p in _mPropertyToTransfer)
            Target.GetComponent<PropertyHolder>().TransferProperty(p, Player.GetComponent<PropertyHolder>());
        foreach (Property p in _mPropertiesToKeep)
            Player.GetComponent<PropertyHolder>().TransferProperty(p, Target.GetComponent<PropertyHolder>());

        if (InfectUpgrade)
        {
            int i = 0;
            foreach (Ability a in _mSelected)
            {
                Player.GetComponent<AbilityControl>().AbsorbAbility(a, i);
                i++;
            }
        }

        _mTriggered = false;
        Target = null;
    }

}