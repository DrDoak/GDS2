using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class Property : MonoBehaviour, ICustomMessageTarget
{

    public virtual void OnCreation() { }
	public virtual void OnHit(Hitbox hb, GameObject attacker) { }
	public virtual void OnHitConfirm(Hitbox myHitbox, GameObject objectHit, HitResult hr) { }
	public virtual void OnSight(Observable observedObj) { }
    public virtual void OnDeath() { }
    public virtual void OnUpdate() { }
    public virtual void OnCollision() { }

	public virtual void OnAttack() { }
	public virtual void OnHitboxCreate (Hitbox hitboxCreated) {}

	public virtual void OnAddProperty() { }
	public virtual void OnRemoveProperty() {}

	public virtual void OnWaterEnter(WaterHitbox waterCollided) { }
	public virtual void OnWaterExit(WaterHitbox waterCollided) {}

	public bool Stealable = true;
	public bool Viewable = true;
	public bool Stackable = false;
	public float value = 0f;

	public string PropertyName = "None";
	[TextArea(3,5)]
	public string Description = "No description provided.";
	public Sprite icon;

	public void CopyPropInfo(Property p) {
		Stealable = p.Stealable;
		Viewable = p.Viewable;
		Stackable = p.Stackable;
		if (PropertyName == "None")
			PropertyName = p.PropertyName;
		if (Description == "No description provided.")
			Description = p.Description;
		if (value == 0f)
			value = p.value;
		icon = p.icon;;
	}
}