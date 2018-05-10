using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Passive_AttackRate : Ability {

    Ab_Melee parent;

    new void Awake()
    {
        base.Awake();
        parent = FindObjectOfType<Ab_Melee>();
        Passive = true;
        _mtier = 1;
    }

    public override void UseAbility()
    {
        if (_mtier < 3)
        {
            _mtier++;
            parent.UpgradeAttackRate();
        }
    }
}
