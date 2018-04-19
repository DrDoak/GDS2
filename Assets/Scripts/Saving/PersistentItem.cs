using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if (data.regID == "") {
			data.regID = "Not Assigned";
		}
		if (SaveObjManager.CheckRegistered(gameObject)) {
			Debug.Log ("Object Already registered, deleting duplicate");
			Destroy(gameObject);
		}
		m_registryChecked = true;
	}
	void Update () {
		if (!m_registryChecked) {
			registryCheck ();
		}
	}

	public void StoreData() {
		Debug.Log ("storing data: player is " + GetComponent<BasicMovement> ().IsCurrentPlayer);
		data.name = gameObject.name;
		data.pos = transform.position;
		data.health = GetComponent<Attackable>().Health;
		data.IsCurrentCharacter = GetComponent<BasicMovement> ().IsCurrentPlayer;
		data.IsFacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
		string properName = "";
		foreach (char c in gameObject.name) {
			if (!c.Equals ('(')) {
				properName += c;
			} else {
				break;
			}
		}
		Debug.Log("ID: " + properName);
		data.prefabPath = properName; //gameObject.name;*/
	}

	public void LoadData() {
		name = data.name;
		transform.position = data.pos;
		//GetComponent<Attackable>().Health = data.health;
	}

	public void ApplyData() {
		SaveObjManager.AddCharData(data);
	}

	void OnEnable() {
		SaveObjManager.OnLoaded += LoadData;
		SaveObjManager.OnBeforeSave += StoreData;
		SaveObjManager.OnBeforeSave += ApplyData;
	}

	void OnDisable() {
		SaveObjManager.OnLoaded -= LoadData;
		SaveObjManager.OnBeforeSave -= StoreData;
		SaveObjManager.OnBeforeSave -= ApplyData;
	}
}