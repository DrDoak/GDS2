using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (Attackable))]
public class BasicMovement : MonoBehaviour
{
	private const float MAX_OFFSET_TOLERANCE = 0.1f;
	private const float SMOOTH_TIME = .1f;
	private const float MIN_JUMP_INTERVAL = .2f;
	private const float NPC_STUCK_JUMP_TIME = .4f;
	private const float NPC_X_DIFF_MOVEMENT_THREASHOLD = 0.4f;
	private const float PIT_JUMP_VERTICAL_THREASHOLD = -0.3f;


	public bool IsCurrentPlayer = false;
	public float JumpHeight = 4.0f;
	public float TimeToJumpApex = .4f;

	// Physics helpers / configurables
	public float MoveSpeed = 8.0f;
	private PhysicsSS m_physics;
	private Vector2 m_velocity;
	private float m_accelerationTimeAirborne = .2f;
	private float m_accelerationTimeGrounded = .1f;
	private float m_velocityXSmoothing;
	float gravity;
	float jumpVelocity;
	Vector2 velocity;
	public Vector2 jumpVector;

	// Movement tracking
	public Vector2 m_inputMove;
	private Vector3 m_targetPoint;
	private bool m_targetSet = false;
	private bool m_targetObj = false;

	public float m_minDistance = 1.0f;
	public float m_abandonDistance = 10.0f;
	private float m_lastJump = 0.0f;
	public float m_stuckTime = 0.0f;
	private float m_verticalStuckTime = 0.0f; 

	private PhysicsSS m_followObj;
	private bool m_autonomy = true;

	internal void Awake()
	{
		m_physics = GetComponent<PhysicsSS>();
		SetJumpData (JumpHeight,TimeToJumpApex);
		/*gravity = -(2 * JumpHeight) / Mathf.Pow (TimeToJumpApex, 2);
		m_physics.SetGravityScale (gravity * (1.0f/60f));
		jumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;
		jumpVector = new Vector2 (0f, jumpVelocity);*/
	}
		
		
	internal void Update()
	{
		if (IsCurrentPlayer && Input.GetButton ("Fire1")) {
			GetComponent<Fighter> ().TryAttack ("default");
		}
		if (IsCurrentPlayer && Input.GetButton ("Fire2")) {
			GetComponent<Fighter> ().TryAttack ("steal");
		}
		if (IsCurrentPlayer && Input.GetButton ("Fire3")) {
			GetComponent<Fighter> ().TryAttack ("give");
		}
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
			else {
				AttemptJump ();
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
		m_velocity.x = Mathf.SmoothDamp(m_velocity.x, targetVel.x, ref m_accelerationTimeGrounded, SMOOTH_TIME);
		//velocity.x = Mathf.SmoothDamp (velocity.x, targetVel.x, ref (m_physics.OnGround)?m_accelerationTimeGrounded:m_accelerationTimeAirborne,SMOOTH_TIME);
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

	private void AttemptJump() {
		float dt = (Time.timeSinceLevelLoad - m_lastJump);
		//Debug.Log ("Attempting Jump ground: " + m_physics.OnGround + " time: " + dt);
		if (!m_physics.OnGround || (Time.timeSinceLevelLoad - m_lastJump) < MIN_JUMP_INTERVAL) {
			return;
		}
		Vector2 jv = new Vector2 (jumpVector.x, jumpVector.y - Mathf.Max (0, m_physics.TrueVelocity.y/Time.deltaTime));
		m_physics.AddSelfForce (jv, 0f);
	
		//Debug.Log ("Jumped");
		m_lastJump = Time.timeSinceLevelLoad;
	}

	public void MoveToPoint(Vector3 point)
	{
		m_inputMove = new Vector2(0,0);

		float dist = Vector3.Distance (transform.position, point);
		if (dist > m_abandonDistance || ( dist < m_minDistance && 
			((m_physics.FacingLeft && point.x < transform.position.x) &&
				(!m_physics.FacingLeft && point.x > transform.position.x)))){
			EndTarget ();
		} else {
			if (m_physics.CanMove) {
				if (Mathf.Abs (transform.position.x - point.x) > NPC_X_DIFF_MOVEMENT_THREASHOLD) {
					if (point.x > transform.position.x) {
						if (dist > m_minDistance)
							m_inputMove.x = 1.0f;
						m_physics.SetDirection (false);
					} else {
						if (dist > m_minDistance)
							m_inputMove.x = -1.0f;
						m_physics.SetDirection (true);
					}
				}
			}
		}
		float targetVelocityX = m_inputMove.x * MoveSpeed;

		if (Mathf.Abs (m_inputMove.x) >= 0.9f && (m_physics.FallDir == FallDirection.LEFT || m_physics.FallDir == FallDirection.RIGHT ) &&
			(point.y - transform.position.y) > PIT_JUMP_VERTICAL_THREASHOLD) {
			AttemptJump ();
		}
		JumpOverObstacle (point);
		JumpVerticalObstacles (point);
	}

	private void JumpOverObstacle(Vector2 point) {
		if (Mathf.Abs (m_inputMove.x) >= 0.9f && Mathf.Abs (m_physics.TrueVelocity.x) < 0.05f ) {
			m_stuckTime += Time.deltaTime;
			if (m_stuckTime > NPC_STUCK_JUMP_TIME) {
				m_stuckTime = 0f;
				AttemptJump ();
			}
		} else {
			m_stuckTime = 0f;
		}
	}
	private void JumpVerticalObstacles(Vector2 point) {
		if ( Mathf.Abs (transform.position.x - point.x) < NPC_X_DIFF_MOVEMENT_THREASHOLD && 
			(point.y - transform.position.y) > -PIT_JUMP_VERTICAL_THREASHOLD && 
			(point.y - transform.position.y) < JumpHeight * 1.5f) {
			m_stuckTime += Time.deltaTime;
			if (m_verticalStuckTime > NPC_STUCK_JUMP_TIME) {
				m_verticalStuckTime = 0f;
				AttemptJump ();
			}
		} else if ((point.y - transform.position.y) < PIT_JUMP_VERTICAL_THREASHOLD) {
			m_verticalStuckTime += Time.deltaTime;
			if (m_verticalStuckTime > NPC_STUCK_JUMP_TIME) {
				m_verticalStuckTime = 0f;
				m_physics.setDropTime(0.05f);
			}
		} else {
			m_verticalStuckTime = 0f;
		}
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

	public void SetMoveSpeed(float moveSpeed) {
		MoveSpeed = moveSpeed;
	}

	public void SetJumpData(float jumpHeight, float timeToJumpApex) {
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		m_physics.SetGravityScale (gravity * (1.0f/60f));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		jumpVector = new Vector2 (0f, jumpVelocity);
	}
}