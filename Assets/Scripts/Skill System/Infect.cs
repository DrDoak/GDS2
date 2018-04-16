﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infect : Ability {

    public new AbilityType AbilityClassification = AbilityType.SPECIAL;

    private List<Property> _mPlayerProps;
    private List<Property> _mEnemyProps;

    private bool _mTriggered = false;

    public override void UseAbility()
    {
        if (!Target)
        {
            Debug.Log("Out of range.");
            return;
        }
        GetPlayerProperties();
        GetTargetProperties();
        if (!_mTriggered)
            DisplayPropertyUI();
        else
            TransferProperty();
    }

    private void GetPlayerProperties()
    {
        _mPlayerProps = Player.GetComponent<PropertyHolder>().GetStealableProperties();
    }

    private void GetTargetProperties()
    {
        _mEnemyProps = Target.GetComponent<PropertyHolder>().GetStealableProperties();
    }


    private void DisplayPropertyUI()
    {
        GUIHandler.SetAbility(this);
        GUIHandler.CreatePropertyList(_mPlayerProps, "player", Player.transform.position, true);
        GUIHandler.CreatePropertyList(_mEnemyProps, "bad guy", Target.transform.position, false);
        _mTriggered = true;
    }

    private void TransferProperty()
    {
        GUIHandler.ClosePropertyLists();
        if(_mPropertyToTransfer)
            Target.GetComponent<PropertyHolder>().AddProperty(_mPropertyToTransfer);
        _mTriggered = false;
        Target = null;
    }
}