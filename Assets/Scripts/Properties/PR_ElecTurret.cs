using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_ElecTurret : PR_Mechanical {

	protected override void OnActive() {
		if (GetComponent<Turret> () != null) {
			GetComponent<Turret> ().SetTarget (GameManager.Instance.CurrentPlayer);
		}
	}
	protected override void OnDisable() {
		if (GetComponent<Turret> () != null) {
			GetComponent<Turret> ().SetTarget (null);
		}
	}
}
