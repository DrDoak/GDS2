using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_PropGive : Ability {

	public new AbilityType AbilityClassification = AbilityType.COMBAT;

	private List<Property> _mPlayerProps;
	private List<Property> _mEnemyProps;
	private bool _mTriggered = false;

	void Awake() {
		Debug.Log ("awake");
		UseAttackHitbox = true;
	}

	public override void UseAbility()
	{

		if (Target == null) {
			AtkAbilityHitTrigger at = (AtkAbilityHitTrigger)Player.GetComponent<Fighter> ().TryAttack ("give");
			if (at != null) 
				at.mAbility = this;
		} else {
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
		_mPlayerProps = Player.GetComponent<PropertyHolder>().GetStealableProperties();
	}

	private void GetTargetProperties()
	{
		_mEnemyProps = Target.GetComponent<PropertyHolder>().GetStealableProperties();
	}


	private void DisplayPropertyUI()
	{
		GUIHandler.SetAbility(this);
		GUIHandler.CreatePropertyList(_mPlayerProps, "Item to Give", new Vector3(100f,-200f,0f), true);
		GUIHandler.CreatePropertyList(_mEnemyProps, "Target", new Vector3(400f,-200f,0f), false);
		PauseGame.Pause (false);
		_mTriggered = true;
	}

	private void TransferProperty()
	{
		GUIHandler.ClosePropertyLists();
		PauseGame.Resume ();
		foreach(Property p in _mPropertyToTransfer)
			Player.GetComponent<PropertyHolder>().TransferProperty(p,Target.GetComponent<PropertyHolder>());
		_mTriggered = false;
		Target = null;
        ClearLists();
	}
}
