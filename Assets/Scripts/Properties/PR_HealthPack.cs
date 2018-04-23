using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_HealthPack : Property {

	public override void OnAddProperty()
	{
		if (GetComponent<ExperienceHolder>() != null) {
			m_giveHealth ();
		}
	}
		

	void m_giveHealth() {
		GetComponent<Attackable> ().DamageObj (-value);
		GetComponent<PropertyHolder> ().RequestRemoveProperty ("HealthPack");
	}
}