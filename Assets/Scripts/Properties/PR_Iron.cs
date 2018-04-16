using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Iron : Property {

    Resistence physResist;
    Resistence fireResist;
    Resistence lightningResist;
	GameObject fx;

    public override void OnAddProperty()
    {
        physResist = GetComponent<Attackable>().AddResistence(ElementType.PHYSICAL, 25.0f, false, false, 0.0f, 70.0f, 70.0f);
        fireResist = GetComponent<Attackable>().AddResistence(ElementType.FIRE, 50.0f, false, false);
        lightningResist = GetComponent<Attackable>().AddResistence(ElementType.LIGHTNING, -25.0f, false, false);
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().SetMoveSpeed (GetComponent<BasicMovement> ().MoveSpeed / 2.0f);
			GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight / 2.0f, GetComponent<BasicMovement> ().TimeToJumpApex);
		}
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXIron);
     //   GetComponent<PhysicsSS>().SetGravityScale(-2.0f);
    }

    public override void OnRemoveProperty()
    {
        GetComponent<Attackable>().RemoveResistence(physResist);
        GetComponent<Attackable>().RemoveResistence(fireResist);
        GetComponent<Attackable>().RemoveResistence(lightningResist);
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().SetMoveSpeed (GetComponent<BasicMovement> ().MoveSpeed * 2.0f);
			GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight * 2.0f, GetComponent<BasicMovement> ().TimeToJumpApex);
		}
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
        //GetComponent<PhysicsSS>().SetGravityScale(-1.0f);
    }

}
