﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyProxy : PropertyHolder {

	public PropertyHolder PropHolder;
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () { 
		if (PropHolder == null)
			Destroy (this);
	}

	public override List<Property> GetVisibleProperties() {
		Debug.Log ("Override");
		return PropHolder.GetVisibleProperties();
	}

	public override void AddProperty(Property originalP) {
		PropHolder.AddProperty (originalP);
	}

	public override void AddProperty(string pName) {
		PropHolder.AddProperty (pName);
	}

	public override void ClearProperties() {
		PropHolder.ClearProperties ();
	}

	public override void RequestRemoveProperty(string pName) {
		PropHolder.RequestRemoveProperty (pName);
	}

	public override void RemoveProperty(Property p) {
		PropHolder.RemoveProperty (p);
	}

	public override bool HasProperty(Property p) {
		return PropHolder.HasProperty (p);
	}

	public override bool HasProperty(string pName) {
		return PropHolder.HasProperty (pName);
	}

	public override void TransferProperty( Property p, PropertyHolder other) {
		PropHolder.TransferProperty (p,other);
	}

	public override GameObject AddBodyEffect(GameObject go) {
		return PropHolder.AddBodyEffect (go);
	}

	public override void RemoveBodyEffect(GameObject go) {
		Destroy (go);
	}

	public override Vector3 BodyScale() {
		return PropHolder.BodyScale ();
	}
}
