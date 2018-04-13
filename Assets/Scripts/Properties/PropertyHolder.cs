using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyHolder : MonoBehaviour {

	List<Property> m_properties;
	bool m_currentPlayer;
	// Use this for initialization
	void Awake () {
		m_properties = new List<Property> ();
	}

	void Start() {
		Property[] prList = GetComponents<Property> ();
		foreach (Property p in prList) {
			m_properties.Add (p);
		}
		m_currentPlayer = (GetComponent<BasicMovement> () && GetComponent<BasicMovement> ().IsCurrentPlayer);
	}
	// Update is called once per frame
	void Update () {}

	public List<Property> GetStealableProperties() {
		List<Property> lp = new List<Property> ();
		foreach (Property p in m_properties) {
			if (p.Stealable) {
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
		p.OnAddProperty ();
		m_properties.Add (p);
		if (m_currentPlayer) {
			GameManager.Instance.AddPropertyText (p, transform.position);
			GameManager.Instance.AddPropIcon (p);
		}
	}

	public void RemoveProperty(Property p) {
		if (HasProperty(p)) {
			RemoveProperty (p.GetType().Name);
			if (m_currentPlayer) {
				GameManager.Instance.RemovePropIcon (p);
			}
		}
	}

	public void RemoveProperty(string pName) {
		if (HasProperty (pName)) {
			Type pType = Type.GetType (pName);
			Property mp = (Property)gameObject.GetComponent(pType);
			m_properties.Remove (mp);
			mp.OnRemoveProperty ();
			Destroy(mp);
		}

	}

	public bool HasProperty(Property p) {
		return (HasProperty(p.GetType().Name));
	}

	public bool HasProperty(string pName) {
		foreach (Property p in m_properties) {
			if (p.GetType ().Name == pName)
				return true;
		}
		return false;
	}

	public void TransferProperty( Property p, PropertyHolder other) {
		RemoveProperty (p);
		other.AddProperty (p);
		GameObject go = Instantiate (GameManager.Instance.PropertyPrefab,transform.position,Quaternion.identity);
		go.GetComponent<ChaseTarget> ().Target = other.GetComponent<BasicMovement> ();
	}
}
