using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Property : MonoBehaviour, ICustomMessageTarget
{

    public virtual void OnCreation() { }
    public virtual void OnHit() { }
    public virtual void OnHitConfirm() { }
    public virtual void OnDeath() { }
    public virtual void OnUpdate() { }
    public virtual void OnCollision() { }
    public virtual void OnAttack() { }

	public virtual void OnAddProperty() { }

	public virtual void OnRemoveProperty() {}
}