using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Melee : Ability {

	public new AbilityType AbilityClassification = AbilityType.COMBAT;

	public override void UseAbility()
	{
		Player.GetComponent<Fighter> ().TryAttack ("melee");
	}
}
