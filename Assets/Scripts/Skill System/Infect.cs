using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infect : Ability {

    public new AbilityType AbilityClassification = AbilityType.SPECIAL;

    public override void UseAbility()
    {
        GetPlayerProperties();
        GetEnemyProperties();
        DisplayPropertyUI();
        TransferProperty();
    }

    private void GetPlayerProperties()
    {

    }

    private void GetEnemyProperties()
    {

    }

    private void DisplayPropertyUI()
    {

    }

    private void TransferProperty()
    {

    }
}
