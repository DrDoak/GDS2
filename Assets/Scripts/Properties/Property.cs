﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class Property : MonoBehaviour, ICustomMessageTarget
{

    public virtual void OnCreation() { }
    public virtual void OnHit() { }
    public virtual void OnHitConfirm() { }
	public virtual void OnSight(Observable observedObj) { }
    public virtual void OnDeath() { }
    public virtual void OnUpdate() { }
    public virtual void OnCollision() { }
    public virtual void OnAttack() { }

	public virtual void OnAddProperty() { }
	public virtual void OnRemoveProperty() {}

	public bool Stealable = true;

	public string PropertyName = "None";
	public string Description = "No description provided.";
	public Sprite icon;
}