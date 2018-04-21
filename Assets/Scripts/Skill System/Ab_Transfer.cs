using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Transfer : Ability {

    public new AbilityType AbilityClassification = AbilityType.COMBAT;

    private List<Property> _mPlayerProps;
    private List<Property> _mEnemyProps;
    private bool _mTriggered = false;

    void Awake()
    {
        ClearLists();
        Debug.Log("awake");
        UseAttackHitbox = true;
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
            GetPlayerProperties();
            GetTargetProperties();
            if (!_mTriggered)
                DisplayPropertyUI();
            else
                TransferProperty();
        }
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
        PauseGame.Pause(false);
        _mTriggered = true;
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
    private void TransferProperty()
    {
        PauseGame.Resume();
        CheckRemovals();

        foreach (Property p in _mPropertyToTransfer)
            Target.GetComponent<PropertyHolder>().TransferProperty(p, Player.GetComponent<PropertyHolder>());
        foreach (Property p in _mPropertiesToKeep)
            Player.GetComponent<PropertyHolder>().TransferProperty(p, Target.GetComponent<PropertyHolder>());

        int i = 0;
        foreach (Ability a in _mSelected)
        {
            Player.GetComponent<AbilityControl>().AbsorbAbility(a, i);
            i++;
        }

        _mTriggered = false;
        Target = null;
    }
}
