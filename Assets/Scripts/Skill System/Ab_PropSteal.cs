using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_PropSteal : Ability {

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
			AtkAbilityHitTrigger at = (AtkAbilityHitTrigger)Player.GetComponent<Fighter> ().TryAttack ("take");
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
		GUIHandler.CreatePropertyList(_mPlayerProps, "You", new Vector3(100f,-200f,0f), false);
		GUIHandler.CreatePropertyList(_mEnemyProps, "Item to Steal", new Vector3(400f,-200f,0f), true);
		PauseGame.Pause (false);
		_mTriggered = true;
	}

	private void TransferProperty()
	{
		GUIHandler.ClosePropertyLists();
		PauseGame.Resume ();
		if(_mPropertyToTransfer)
			Target.GetComponent<PropertyHolder>().TransferProperty(_mPropertyToTransfer,Player.GetComponent<PropertyHolder>());
		_mTriggered = false;
		Target = null;
	}
}
