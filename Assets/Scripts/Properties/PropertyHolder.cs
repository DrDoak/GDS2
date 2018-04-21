using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyHolder : MonoBehaviour {

	List<Property> m_properties;
	bool m_currentPlayer;
	public int MaxSlots = 4;
	public int NumTransfers = 2;
	public List<string> m_toRemove;
	// Use this for initialization
	void Awake () {
		m_properties = new List<Property> ();
		m_toRemove = new List<string> ();
	}

	void Start() {
		Property[] prList = GetComponents<Property> ();
		foreach (Property p in prList) {
			Property mp = GameManager.Instance.GetPropInfo(p);
			p.PropertyName = mp.PropertyName;
			m_properties.Add (p);
			p.OnAddProperty ();
		}
		m_currentPlayer = (GetComponent<BasicMovement> () && GetComponent<BasicMovement> ().IsCurrentPlayer);
		//AddBodyEffect (GameManager.Instance.FXBodyTest);
		if (m_currentPlayer) {
			foreach (Property p in m_properties) {
				GameManager.Instance.AddPropIcon (p);
			}
			//GUIHandler.CreatePropertyList(m_properties, "Test List", Vector3.zero);
		}
	}

	void LateUpdate() {
		foreach (string s in m_toRemove ) {
			Property toRemove = null;
			foreach (Property p in m_properties) {
				if (p.PropertyName == s) {
					toRemove = p;
					break;
				}
			}
			if (toRemove != null) {
				RemoveProperty (toRemove);
			}
		}
		m_toRemove.Clear ();
	}

	public List<Property> GetVisibleProperties() {
		List<Property> lp = new List<Property> ();
		foreach (Property p in m_properties) {
			Property mp = GameManager.Instance.GetPropInfo (p);
			if (mp.Viewable) {
				lp.Add (p);
			}
		}
		return lp;
	}

	public void AddProperty(Property p) {
		AddProperty (p.GetType().Name);
	}

	public void AddProperty(string pName) {
		if (Type.GetType (pName) == null)
			return;
		//Property p = (Property)(System.Activator.CreateInstance (Type.GetType (pName)));
		Type t = Type.GetType (pName);
		gameObject.AddComponent (t);
		Property p = (Property)gameObject.GetComponent (t);
		Property mp = GameManager.Instance.GetPropInfo(p);
		p.Viewable = mp.Viewable;
		p.PropertyName = mp.PropertyName;
		p.Stealable = mp.Stealable;
		p.OnAddProperty ();
		m_properties.Add (p);
		if (m_currentPlayer) {
			GameManager.Instance.AddPropIcon (p);
		}
	}
	public void ClearProperties() {
		foreach (Property p in m_properties) {
			p.OnRemoveProperty();
			Destroy (p);
			if (m_currentPlayer) {
				GameManager.Instance.RemovePropIcon (p);
			}
		}
		m_properties.Clear();
	}
	public void RequestRemoveProperty(string pName) {
		m_toRemove.Add (pName);
	}

	public void RemoveProperty(Property p) {
		if (HasProperty(p)) {
			Type pType = p.GetType();
			Property mp = (Property)gameObject.GetComponent(pType);
			m_properties.Remove (mp);
			mp.OnRemoveProperty ();
			Destroy(mp);

			if (m_currentPlayer) {
				GameManager.Instance.RemovePropIcon (p);
			}
		}
	}

	public bool HasProperty(Property p) {
		foreach (Property mp in m_properties) {
			if (mp.GetType() == p.GetType())
				return true;
		}
		return false;
	}

	public bool HasProperty(string pName) {
		foreach (Property p in m_properties) {
			if (GameManager.Instance.GetPropInfo(p).PropertyName == pName)
				return true;
		}
		return false;
	}

	public void TransferProperty( Property p, PropertyHolder other) {
		RemoveProperty (p);
		other.AddProperty (p);
		GameObject go = Instantiate (GameManager.Instance.FXPropertyPrefab,transform.position,Quaternion.identity);
		go.GetComponent<ChaseTarget> ().Target = other.GetComponent<PhysicsSS> ();
	}

	public GameObject AddBodyEffect(GameObject go) {
		GameObject newGO = Instantiate (go, transform.position, Quaternion.identity);
		newGO.transform.parent = transform;
		Vector3 newS = BodyScale ();
		//newGO.transform.localScale = newS;
		float z =  transform.rotation.eulerAngles.z;
		for (int i = 0; i < newGO.transform.childCount; i++) {
			ParticleSystem.ShapeModule s = newGO.transform.GetChild (i).GetComponent<ParticleSystem>().shape;
			s.scale = newS;
			Vector3 v = newGO.transform.GetChild (i).rotation.eulerAngles;
			newGO.transform.GetChild (i).rotation = Quaternion.Euler(new Vector3(v.x,v.y, z));
		}
		return newGO;
	}
	public void RemoveBodyEffect(GameObject go) {
		Destroy (go);
	}
	public Vector3 BodyScale() {
		Vector3 sc = transform.localScale;
		Vector2 v2 =  GetComponent<BoxCollider2D> ().size;
		return new Vector3(sc.x * v2.x,sc.y *v2.y,1f);
	}
}
