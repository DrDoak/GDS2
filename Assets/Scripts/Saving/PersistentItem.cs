using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (Attackable))]
public class PersistentItem : MonoBehaviour {
	public string saveID = "autoID";
	public string prefabName = "none";
	public Vector3 pos = Vector3.zero;
	public string targetID = "none";
	public RoomDirection targetDir = RoomDirection.NEUTRAL;
	public float healthVal = 0;
	public CharData data = new CharData();
	public bool recreated = false;

	protected bool m_registryChecked = false;

	public void registryCheck() {
		m_registryChecked = true;
		if (data.regID == "") {
			data.regID = "Not Assigned";
		}
		if (SaveObjManager.CheckRegistered(gameObject)) {
			Debug.Log ("Object Already registered, deleting duplicate");
			Destroy(gameObject);
		}
	}
	void Update () {
		if (!m_registryChecked) {
			registryCheck ();
		}
	}

	public void StoreData() {
		data.name = gameObject.name;
		data.pos = transform.position;
		data.health = GetComponent<Attackable>().Health;
		if (GetComponent<BasicMovement>())
			data.IsCurrentCharacter = GetComponent<BasicMovement> ().IsCurrentPlayer;
		data.IsFacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
		if (GetComponent<PropertyHolder> ()) {
			Property[] pL = GetComponents<Property> ();
			string[] allPs = new string[pL.Length];
			for (int i = 0; i < pL.Length; i++) {
				allPs [i] = pL [i].GetType ().ToString ();
			}
			data.propertyList = allPs;
		}
		string properName = "";
		foreach (char c in gameObject.name) {
			if (!c.Equals ('(')) {
				properName += c;
			} else {
				break;
			}
		}
		if (GetComponent<ExperienceHolder> ())
			data.Experience = GetComponent<ExperienceHolder> ().Experience;
		//Debug.Log("ID: " + properName);
		data.prefabPath = properName; //gameObject.name;*/

	}

	public void LoadData() {
		Debug.Log ("Loading data");
		GetComponent<Attackable>().SetHealth( data.health);
		GetComponent<PhysicsSS> ().SetDirection (data.IsFacingLeft);
		if (GetComponent<PropertyHolder> ()) {
			GetComponent<PropertyHolder> ().ClearProperties ();
			for (int i = 0; i < data.propertyList.Length; i++) {
				Type t = Type.GetType (data.propertyList [i]);
				gameObject.AddComponent (t);
			}
		}
		if (GetComponent<ExperienceHolder> ()) {
			GetComponent<ExperienceHolder> ().Experience = data.Experience;
		}
	}

	/*public void ApplyData() {
		Debug.Log ("applying data");
		SaveObjManager.AddCharData(data);
	}*/

	void OnEnable() {
		SaveObjManager.OnLoaded += LoadData;
		SaveObjManager.OnBeforeSave += StoreData;
		//SaveObjManager.OnBeforeSave += ApplyData;
	}

	void OnDisable() {
		SaveObjManager.OnLoaded -= LoadData;
		SaveObjManager.OnBeforeSave -= StoreData;
		//SaveObjManager.OnBeforeSave -= ApplyData;
	}
}