using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hitbox {
	public float ProjectileSpeed;
	public Vector2 AimPoint = new Vector2();
	public int PenetrativePower = 0;
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

}