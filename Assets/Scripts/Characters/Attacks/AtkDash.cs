using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDashInfo {
	public Vector2 StartUpDash = new Vector2 (0.0f, 0f);
	public float StartUpDuration = 0.0f;
	public Vector2 AttackDash = new Vector2 (0.0f, 0f);
	public float AttackDashDuration = 0.0f;
	public Vector2 ConclusionDash = new Vector2 (0.0f, 0f);
	public float ConclusionDuration = 0.0f;
}
public class AtkDash : AttackInfo
{
	public AttackDashInfo m_dashInfo;

	protected override void OnStartUp()
	{
		base.OnStartUp();
		m_physics = GetComponent<PhysicsSS>();
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_dashInfo.StartUpDash), m_dashInfo.StartUpDuration);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_dashInfo.AttackDash), m_dashInfo.AttackDashDuration);
	}

	protected override void OnConclude()
	{
		base.OnConclude();
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_dashInfo.ConclusionDash), m_dashInfo.ConclusionDuration);
	}
}
