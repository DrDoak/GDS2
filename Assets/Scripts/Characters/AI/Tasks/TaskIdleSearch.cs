using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIdleSearch : FighterTask {

	void Init() {
		//Stealable = false;
	}

	override public void Advance() 
	{}

	/*override public void OnSight(Observable o) {
		if (o.GetComponent<Attackable> () && GetComponent<Attackable> ().CanAttack (o.GetComponent<Attackable> ().Faction)) {
			GetComponent<AIFighter> ().CurrentTarget = o.GetComponent<Attackable> ();
			Active = false;
		}
	}*/
}
