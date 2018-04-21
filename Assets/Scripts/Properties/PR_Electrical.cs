using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Electrical : Property {

	GameObject fx;

	public override void OnHitboxCreate (Hitbox hitboxCreated) {
		hitboxCreated.AddElement( ElementType.LIGHTNING );
		hitboxCreated.Damage = hitboxCreated.Damage * 1.2f;
	}
	public override void OnAddProperty()
	{
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXLightning);
		//   GetComponent<PhysicsSS>().SetGravityScale(-2.0f);
	}

	public override void OnRemoveProperty()
	{
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
		//GetComponent<PhysicsSS>().SetGravityScale(-1.0f);
	}
}
