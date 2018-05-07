using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_ElecDoor : PR_Mechanical {

	protected override void OnActive() {
		Debug.Log ("On door active?");
		if (GetComponent<Door> () != null) {
			GetComponent<Door> ().SetOpen (true);
		}
	}
	protected override void OnDisable() {
		Debug.Log ("On door inactive?");
		if (GetComponent<Door> () != null) {
			GetComponent<Door> ().SetOpen (false);
		}
	}
}

