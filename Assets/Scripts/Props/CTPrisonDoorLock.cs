using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTPrisonDoorLock : MonoBehaviour {

	bool HasElec = false;
	void Status() {
		
	}
	// Update is called once per frame
	void Update () {
		if (!HasElec && GetComponent<PropertyHolder> ().HasProperty ("Electrical")) {
		} else if (HasElec && !GetComponent<PropertyHolder> ().HasProperty ("Electrical")) {
		}
	}

	void UpdateStatus() {
		bool hasPower = GetComponent<PropertyHolder> ().HasProperty ("Electrical");
		PropertyHolder[] pList = FindObjectsOfType<PropertyHolder> ();
		if (hasPower) {
			foreach (PropertyHolder h in pList) {
				if (h.HasProperty ("Electric_Door") && !h.HasProperty("Electrical")) {
					h.AddProperty ("Electrical");
				}
			}
		} else {
			foreach (PropertyHolder h in pList) {
				if (h.HasProperty ("Electric_Door")) {
					h.RequestRemoveProperty ("Electric_Door");
				}
			}
		}
		HasElec = hasPower;

	}
}
