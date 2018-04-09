using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxMaker : MonoBehaviour
{
	public GameObject HitboxClass;
	public GameObject LineHBClass;

	public List<string> hitTypes;
	public FactionType Faction;
	PhysicsSS m_physics;
	Fighter m_fighter;

	void Awake () {
		m_physics = GetComponent<PhysicsSS>();
		m_fighter = GetComponent<Fighter>();

	}

	void Start() {
		if (GetComponent<Attackable> ()) {
			Faction = GetComponent<Attackable> ().Faction;
		}
	}
	public LineHitbox createLineHB(float range, Vector2 aimPoint, Vector2 offset,float damage, float stun, float hitboxDuration, Vector2 knockback, bool followObj = true) {
		Vector3 newPos = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);
		GameObject go = Instantiate(LineHBClass,newPos,Quaternion.identity) as GameObject; 
		LineHitbox line = go.GetComponent<LineHitbox> ();
		line.setRange (range);
		line.Damage = damage;
		line.setAimPoint (aimPoint);
		line.Duration = hitboxDuration;
		line.Knockback = m_physics.OrientVectorToDirection(knockback);
		line.IsFixedKnockback = true;
		line.Creator = gameObject;
		line.Faction = Faction;
		//line.reflect = hitboxReflect;
		line.Stun = stun;
		//line.mAttr = mAttrs;
		line.Init();
		//Debug.Log ("Creating Line HB");
		return line;
	}

	public Hitbox CreateHitbox(Vector2 hitboxScale, Vector2 offset, float damage, float stun, float hitboxDuration, Vector2 knockback, bool fixedKnockback = true, bool followObj = true)
	{
		Vector2 cOff = m_physics.OrientVectorToDirection(offset);
		Vector3 newPos = transform.position + (Vector3)cOff;
		var go = GameObject.Instantiate(HitboxClass, newPos, Quaternion.identity);
		if (followObj)
			go.transform.SetParent(gameObject.transform);

		Hitbox newBox = go.GetComponent<Hitbox>();
		newBox.SetScale(hitboxScale);
		newBox.Damage = damage;
		newBox.Duration = hitboxDuration;
		newBox.Knockback = m_physics.OrientVectorToDirection(knockback);
		newBox.IsFixedKnockback = fixedKnockback;
		newBox.Stun = stun;
		newBox.HitTypes = hitTypes;
		newBox.Creator = gameObject;
		newBox.Faction = Faction;
		if (followObj)
			newBox.SetFollow (gameObject,offset);

		newBox.Init();
		return newBox;
	}

	public void ClearHitboxes()
	{
		foreach (Hitbox hb in GetComponentsInChildren<Hitbox>())
			Destroy(hb.gameObject);
	}

	public void RegisterHit(GameObject otherObj, Hitbox hb, HitResult hr)
	{
		//Debug.Log ("Registering Hit: " + otherObj);
		if (m_fighter)
			m_fighter.RegisterHit (otherObj,hb,hr);
	}

	public void AddHitType(string hitType)
	{
		hitTypes.Add (hitType);
	}

	public void ClearHitTypes()
	{
		hitTypes = new List<string> ();
	}

}
