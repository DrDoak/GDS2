using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (Attackable))]
public class BasicMovement : MonoBehaviour
{
	private const float MAX_OFFSET_TOLERANCE = 0.1f;
	private const float SMOOTH_TIME = .1f;


	public bool IsCurrentPlayer = false;
	public float JumpHeight = 4.0f;
	public float TimeToJumpApex = .4f;

	// Physics helpers / configurables
	public float MoveSpeed = 8.0f;
	private PhysicsSS m_physics;
	private Vector2 m_velocity;
	private float m_accelerationX = 0;
	private float m_accelerationY = 0;
	float gravity;
	float jumpVelocity;
	Vector2 velocity;
	Vector2 jumpVector;

	// Movement tracking
	public Vector2 m_inputMove;
	private Vector3 m_targetPoint;
	private bool m_targetSet = false;
	private bool m_targetObj = false;

	public float m_minDistance = 1.0f;
	public float m_abandonDistance = 10.0f;
	private PhysicsSS m_followObj;
	private bool m_autonomy = true;

	internal void Awake()
	{
		m_physics = GetComponent<PhysicsSS>();
		gravity = -(2 * JumpHeight) / Mathf.Pow (TimeToJumpApex, 2);
		m_physics.SetGravityScale (gravity * (1.0f/60f));
		jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;
		jumpVector = new Vector2 (0f, jumpVelocity);
	}

	internal void Update()
	{
		if (IsCurrentPlayer && Input.GetButton("Fire1"))
			GetComponent<Fighter>().TryAttack("default");

		if (!m_physics.CanMove)
			return;
		
		if (IsCurrentPlayer && m_autonomy)
			PlayerMovement();
		else if (m_targetSet)
			NpcMovement();

		MoveSmoothly();
	}

	internal void PlayerMovement() {
		m_inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		JumpMovement ();
		SetDirectionFromInput();
	}
	internal void JumpMovement() {
		if (Input.GetButtonDown("Jump")) {
			if (m_inputMove.y < -0f) {
				m_physics.setDropTime(0.05f);
			}
			else if (m_physics.OnGround) {
				m_physics.AddSelfForce (jumpVector, 0f);
			}
		}
	}
	private void NpcMovement()
	{
		if (m_targetObj)
		{
			if (m_followObj == null)
			{
				EndTarget ();
				return;
			}
			m_targetPoint = m_followObj.transform.position;
		}
		MoveToPoint(m_targetPoint);
	}


	private void MoveSmoothly()
	{
		Vector2 targetVel = new Vector2(m_inputMove.x * MoveSpeed, m_inputMove.y * MoveSpeed);
		m_velocity.x = Mathf.SmoothDamp(m_velocity.x, targetVel.x, ref m_accelerationX, SMOOTH_TIME);
		//m_velocity.y = Mathf.SmoothDamp(m_velocity.y, targetVel.y, ref m_accelerationY, SMOOTH_TIME);
		m_physics.Move(m_velocity, m_inputMove);
	}
		

	// Priority goes to UP and DOWN directions
	private void SetDirectionFromInput()
	{
		if ( m_inputMove.x != 0f )
			m_physics.SetDirection ( m_inputMove.x < 0.0f );
	}

	private void SetInputAndDirectionFromOffset(Vector2 offset)
	{
		if (Mathf.Abs(offset.x) > MAX_OFFSET_TOLERANCE)
			m_inputMove.x = offset.x < 0 ? -1.0f : 1.0f;
		if (Mathf.Abs(offset.y) > MAX_OFFSET_TOLERANCE)
			m_inputMove.y = offset.y < 0 ? -1.0f : 1.0f;
		SetDirectionFromInput ();
	}

	public void MoveToPoint(Vector3 point)
	{
		m_inputMove = new Vector2(0,0);
		float dist = Vector3.Distance(transform.position, point);

		if (dist > m_abandonDistance || dist < m_minDistance)
		{
			EndTarget();
			return;
		}

		if (m_physics.CanMove && dist > m_minDistance)
			SetInputAndDirectionFromOffset(point - transform.position);
	}

	public void SetTargetPoint(Vector3 point, float proximity, float max = float.MaxValue)
	{
		m_targetPoint = point;
		m_minDistance = proximity;
		m_abandonDistance = max;
		m_targetSet = true;
	}

	private void SetTarget(PhysicsSS target)
	{
		m_targetObj = true;
		m_targetSet = true;
		m_followObj = target;
	}

	public void EndTarget()
	{
		m_targetSet = false;
		m_targetObj = false;
		m_followObj = null;
		m_minDistance = 0.2f;
	}
}