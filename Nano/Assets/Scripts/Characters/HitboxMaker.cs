using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxMaker : MonoBehaviour
{
	public GameObject hitboxClass;
	public List<string> hitTypes;
	PhysicsSS m_physics;
	Fighter m_fighter;

	void Awake () {
		m_physics = GetComponent<PhysicsSS>();
		m_fighter = GetComponent<Fighter>();
	}

	public Hitbox CreateHitbox(Vector2 hitboxScale, Vector2 offset, float damage, float stun, float hitboxDuration, Vector2 knockback, bool fixedKnockback, bool followObj)
	{
		Vector2 cOff = m_physics.OrientVectorToDirection(offset);
		Vector3 newPos = transform.position + (Vector3)cOff;
		var go = GameObject.Instantiate(hitboxClass, newPos, Quaternion.identity);
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

	public void RegisterHit(GameObject otherObj)
	{
		if (m_fighter)
			m_fighter.RegisterHit (otherObj);
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
