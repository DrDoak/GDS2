using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hitbox {
	public float ProjectileSpeed;
	public Vector2 AimPoint = new Vector2();
	public int PenetrativePower = 0;
	public bool TravelThroughWalls = false;
	public bool OrientToSpeed = true;
	int m_numPenetrated = 0;

	virtual internal void Update()
	{
		Tick();
	}

	protected void Tick()
	{
		base.Tick ();
		Vector3 movement = new Vector3 (ProjectileSpeed * Time.deltaTime * AimPoint.normalized.x,
			ProjectileSpeed * Time.deltaTime * AimPoint.normalized.y, 0f);
		transform.Translate (movement, Space.World);
		if (OrientToSpeed)
			orientToSpeed (new Vector2(movement.x,movement.y));
	}
	protected override HitResult OnAttackable(Attackable atkObj)
	{
		if (canAttack (atkObj))
			incrementPenetration ();
		return base.OnAttackable (atkObj);
	}

	void incrementPenetration() {
		m_numPenetrated++;
		if (m_numPenetrated > PenetrativePower)
			Duration = 0f;
	}
	protected override void OnHitObject(Collider2D other) {
		if (TravelThroughWalls)
			return;
		Debug.Log ("Hit this: " + other.gameObject + " : " + Creator);
		if (other.gameObject != Creator && !other.isTrigger && other.GetComponent<Attackable> () == null)
			Duration = 0f;
	}
	void orientToSpeed(Vector2 speed) {
		transform.rotation = Quaternion.Euler (new Vector3(0f,0f,Mathf.Rad2Deg * Mathf.Atan2 (speed.y, speed.x)));
	}
}