using System.Collections;
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
        _mPlayerProps = Player.GetComponent<PropertyHolder>().GetVisibleProperties();
    }

    private void GetTargetProperties()
    {
        _mEnemyProps = Target.GetComponent<PropertyHolder>().GetVisibleProperties();
    }


    private void DisplayPropertyUI()
    {
        GUIHandler.SetAbility(this);
		PauseGame.Pause (false);
		_mTriggered = true;
    }

    private void TransferProperty()
    {
        GUIHandler.ClosePropertyLists();
		PauseGame.Resume ();
        if(_mPropertyToTransfer)
			Player.GetComponent<PropertyHolder>().TransferProperty(_mPropertyToTransfer,Target.GetComponent<PropertyHolder>());
		_mTriggered = false;
        Target = null;
    }
}
