using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : Ability {

    public new AbilityType AbilityClassification = AbilityType.SPECIAL;

    private List<Ability> _mPlayerAbilities;
    private List<Ability> _mTargetAbilities;

    private Ability _mSelected;

    private bool _mTriggered = false;

    public override void UseAbility()
    {
        if (!Target)
            return;
        if(!_mTriggered)
        {
            Debug.Log("Looting");
            UpdateAbilityLists();
            DisplayPlayerAbilities();
            DisplayTargetAbilities();
            _mTriggered = true;
        }
        else
        {
            LootAbility();
        }
    }

    private void DisplayTargetAbilities()
    {

    }

    private void DisplayPlayerAbilities()
    {

    }

    private void LootAbility()
    {
        if(_mSelected)
            Player.GetComponent<AbilityControl>().AbsorbAbility(_mSelected);
        _mTriggered = false;
    }

    private void UpdateAbilityLists()
    {
        _mPlayerAbilities = Player.GetComponent<BasicAbilityControl>().Abilities;
        _mTargetAbilities = Target.GetComponent<BasicAbilityControl>().Abilities;
    }
}
