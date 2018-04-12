using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infect : Ability {

    public new AbilityType AbilityClassification = AbilityType.SPECIAL;

    private List<Property> _mPlayerProps;
    private List<Property> _mEnemyProps;
    private Property _mPropertyToTransfer;

    private bool _mTriggered = false;

    public override void UseAbility()
    {
        if (!Target)
        {
            Debug.Log("Out of range.");
            return;
        }
        GetPlayerProperties();
        if (!_mTriggered)
            DisplayPropertyUI();
        else
            TransferProperty();
    }

    private void GetPlayerProperties()
    {
        _mPlayerProps = Player.GetComponent<PropertyHolder>().GetStealableProperties();
    }

    private void DisplayPropertyUI()
    {
        Debug.Log("Opening display");
        _mTriggered = true;
    }

    private void TransferProperty()
    {
        Target.GetComponent<PropertyHolder>().AddProperty(_mPropertyToTransfer);
        _mTriggered = false;
    }
}
