using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkDash : AttackInfo
{
	public Vector2 StartUpDash = new Vector2 (0.0f, 0f);
	public float StartUpDuration = 0.0f;
	public Vector2 AttackDash = new Vector2 (0.0f, 0f);
	public float AttackDashDuration = 0.0f;
	public Vector2 ConclusionDash = new Vector2 (0.0f, 0f);
	public float ConclusionDuration = 0.0f;

	protected override void OnStartUp()
	{
		base.OnStartUp();
		m_physics = GetComponent<PhysicsSS>();
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(StartUpDash), StartUpDuration);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(AttackDash), AttackDashDuration);
	}

	protected override void OnConclude()
	{
		base.OnConclude();
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(ConclusionDash), ConclusionDuration);
	}
}
