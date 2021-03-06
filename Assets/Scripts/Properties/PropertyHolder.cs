﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropertyHolder : MonoBehaviour {

	public List<Property> m_properties;
	public string HolderName = "SameAsObject";
	bool m_currentPlayer;
	public int MaxSlots = 4;
	public int NumTransfers = 99;
	public List<string> m_toRemove;
	public WaterHitbox SubmergedHitbox = null;
	public Color EffectColor = Color.white;

	// Use this for initialization
	void Awake () {
		m_properties = new List<Property> ();
		m_toRemove = new List<string> ();
	}

	void Start() {
		Property[] prList = GetComponents<Property> ();
		foreach (Property p in prList) {
			if (p != null) {
				if (GameManager.Instance != null) {
					Property mp = GameManager.Instance.GetPropInfo (p);
					p.CopyPropInfo (mp);
				}
				m_properties.Add (p);
				p.OnAddProperty ();
				if (SubmergedHitbox != null)
					p.OnWaterEnter (SubmergedHitbox);
			}
		}
		m_currentPlayer = (GetComponent<BasicMovement> () && GetComponent<BasicMovement> ().IsCurrentPlayer);
		//AddBodyEffect (GameManager.Instance.FXBodyTest);
		if (m_currentPlayer) {
			foreach (Property p in m_properties) {
				GUIHandler.Instance.AddPropIcon (p);
			}
			//GUIHandler.CreatePropertyList(m_properties, "Test List", Vector3.zero);
		}
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnCreation ());
	}

	void Update() {
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnUpdate ());
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

	public virtual List<Property> GetVisibleProperties() {
		List<Property> lp = new List<Property> ();
		foreach (Property p in m_properties) {
			if (p != null && p.Viewable && !m_toRemove.Contains(p.PropertyName)) {
				lp.Add (p);
			}
		}
		return lp;
	}

	public virtual void AddProperty(Property originalP) {
		if (originalP.GetType() == null)
			return;
		//Property p = (Property)(System.Activator.CreateInstance (Type.GetType (pName)));
		Type t = originalP.GetType();
		Property p = (Property)gameObject.AddComponent (t);

		p.CopyPropInfo (originalP);
		p.CopyPropInfo (GameManager.Instance.GetPropInfo (p));
		m_properties.Add (p);
		p.OnAddProperty ();
		if (SubmergedHitbox != null)
			p.OnWaterEnter (SubmergedHitbox);
		if (m_currentPlayer)
			GUIHandler.Instance.AddPropIcon (p);
	}

	public virtual void AddProperty(string pName) {
		if (Type.GetType (pName) == null)
			return;
		//Property p = (Property)(System.Activator.CreateInstance (Type.GetType (pName)));
		Type t = Type.GetType (pName);
		Property p = (Property)gameObject.AddComponent (t);
		if (GameManager.Instance != null) {
			p.CopyPropInfo (GameManager.Instance.GetPropInfo (p));
		}
		m_properties.Add (p);
		p.OnAddProperty ();
		if (SubmergedHitbox != null)
			p.OnWaterEnter (SubmergedHitbox);
		if (m_currentPlayer) {
			GUIHandler.Instance.AddPropIcon (p);
		}
	}
	public virtual void ClearProperties() {
		Property[] prList = GetComponents<Property> ();
		foreach (Property p in prList) {
			if (m_properties.Contains (p)) {
				m_properties.Remove (p);
			}
			p.OnRemoveProperty();
			if (SubmergedHitbox != null) {
				p.OnWaterExit (SubmergedHitbox);
			}
			Destroy (p);
			if (m_currentPlayer) {
				GUIHandler.Instance.RemovePropIcon (p);
			}
		}
		m_properties.Clear();
	}
	public virtual void RequestRemoveProperty(string pName) {
		m_toRemove.Add (pName);
	}

	public virtual void RemoveProperty(Property p) {
		if (m_properties.Contains(p)) {
			m_properties.Remove (p);
			p.OnRemoveProperty ();
			if (SubmergedHitbox != null)
				p.OnWaterExit (SubmergedHitbox);
			Destroy(p);
			if (m_currentPlayer) {
				GUIHandler.Instance.RemovePropIcon (p);
			}
		} else if (HasProperty(p)) {
			Type pType = p.GetType();
			Property mp = (Property)gameObject.GetComponent(pType);
			m_properties.Remove (mp);
			mp.OnRemoveProperty ();
			if (SubmergedHitbox != null)
				mp.OnWaterExit (SubmergedHitbox);
			Destroy(mp);

			if (m_currentPlayer) {
				GUIHandler.Instance.RemovePropIcon (p);
			}
		}
	}

	public virtual bool HasProperty(Property p) {
		foreach (Property mp in m_properties) {
			if (mp != null && mp.GetType() == p.GetType())
				return true;
		}
		return false;
	}

	public virtual bool HasProperty(string pName) {
		foreach (Property p in m_properties) {
			if (p != null && p.PropertyName == pName)
				return true;
		}
		return false;
	}

	public virtual void TransferProperty( Property p, PropertyHolder other) {
		RemoveProperty (p);
		other.AddProperty (p);
		//GameObject go = Instantiate (GameManager.Instance.FXPropertyPrefab,transform.position,Quaternion.identity);
		//go.GetComponent<ChaseTarget> ().Target = other.GetComponent<PhysicsSS> ();
	}

	public virtual GameObject AddBodyEffect(GameObject go) {
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
	public virtual void RemoveBodyEffect(GameObject go) {
		Destroy (go);
	}
	public virtual Vector3 BodyScale() {
		Vector3 sc = transform.localScale;
		Vector2 v2 =  GetComponent<BoxCollider2D> ().size;
		return new Vector3(sc.x * v2.x,sc.y *v2.y,1f);
	}

	public void AddAmbient(AudioClip ac) {
		AmbientNoise am = GetComponent<AmbientNoise> ();
		if (am == null)
			am = gameObject.AddComponent<AmbientNoise> ();
		am.AddSound (ac);
	}
	public void RemoveAmbient(AudioClip ac) {
		AmbientNoise am = GetComponent<AmbientNoise> ();
		if (am == null)
			return;
		am.RemoveSound (ac);
	}
}
