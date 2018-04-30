using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	void Start() {
		if (data.regID == "") {
			data.regID = SaveObjManager.Instance.GenerateID (gameObject);
		}
		saveID = data.regID;
	}
	public void registryCheck() {
		m_registryChecked = true;
		if (data.regID == "") {
			data.regID = "Not Assigned";
		}
		if (SaveObjManager.CheckRegistered(gameObject)) {
			Debug.Log (gameObject + " Already registered, deleting duplicate");
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
		data.targetID = "";
		if (GetComponent<Interactable>()) {
			data.TriggerUsed = GetComponent<Interactable> ().TriggerUsed;
			data.triggerString = GetComponent<Interactable> ().value;
		}
		if (GetComponent<Attackable>())
			data.health = GetComponent<Attackable>().Health;
		if (GetComponent<BasicMovement>())
			data.IsCurrentCharacter = GetComponent<BasicMovement> ().IsCurrentPlayer;
		if (GetComponent<PhysicsSS>())
			data.IsFacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
		if (GetComponent<PropertyHolder> ()) {
			Property[] pL = GetComponents<Property> ();
			string[] allPs = new string[pL.Length];
			string[] allDs = new string[pL.Length];
			float[] allVs = new float[pL.Length];
			for (int i = 0; i < pL.Length; i++) {
				allPs [i] = pL [i].GetType ().ToString ();
				allDs [i] = pL [i].Description;
				allVs [i] = pL [i].value;
			}
			data.propertyList = allPs;
			data.propertyDescriptions = allDs;
			data.propertyValues = allVs;
		}
		string properName = "";
		foreach (char c in gameObject.name) {
			if (!c.Equals ('(') && !c.Equals(' ')) {
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
		if (GetComponent<Attackable>())
			GetComponent<Attackable>().SetHealth( data.health);
		if (GetComponent<PhysicsSS>())
			GetComponent<PhysicsSS> ().SetDirection (data.IsFacingLeft);
		if (GetComponent<PropertyHolder> ()) {
			GetComponent<PropertyHolder> ().ClearProperties ();
			for (int i = 0; i < data.propertyList.Length; i++) {
				Type t = Type.GetType (data.propertyList [i]);
				Property p = (Property)gameObject.AddComponent (t);
				p.Description = data.propertyDescriptions [i];
				p.value = data.propertyValues [i];
			}
		}
		if (GetComponent<ExperienceHolder> ()) {
			GetComponent<ExperienceHolder> ().Experience = data.Experience;
		}
		if (GetComponent<Interactable>()) {
			GetComponent<Interactable> ().TriggerUsed = data.TriggerUsed;
			GetComponent<Interactable> ().value = data.triggerString;
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