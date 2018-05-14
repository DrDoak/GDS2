using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Luminosity.IO;

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
	private const float DOUBLE_TAP_DROP_INTERVAL = 0.2f;


	public bool IsCurrentPlayer = false;
	public float JumpHeight = 4.0f;
	public float TimeToJumpApex = .4f;
	public bool VariableJumpHeight = false;
	public bool variableJumpApplied = false;

	// Physics helpers / configurables
	public float MoveSpeed = 8.0f;
	private PhysicsSS m_physics;
	private Vector2 m_velocity;
	//private float m_accelerationTimeAirborne = .2f;
	private float m_accelerationTimeGrounded = .1f;
	private float m_velocityXSmoothing;
	float gravity;
	float jumpVelocity;
	Vector2 velocity;
	public Vector2 jumpVector;

	// Movement tracking
	public Vector2 m_inputMove;
	protected bool m_jumpDown;
	protected bool m_jumpHold;
	private Vector3 m_targetPoint;
	private bool m_targetSet = false;
	private bool m_targetObj = false;

	public float m_minDistance = 1.0f;
	public float m_abandonDistance = 10.0f;
	public bool Submerged = false;
	private float m_lastJump = 0.0f;
	public float m_stuckTime = 0.0f;
	private float m_verticalStuckTime = 0.0f; 

	private PhysicsSS m_followObj;
	private bool m_autonomy = true;

	public bool playFootsteps = false;
	public float footstepInterval = 0.75f;
	float m_sinceStep = 0f;

	private float m_lastDownTime = 0f;

	internal void Awake()
	{
		m_physics = GetComponent<PhysicsSS>();
		SetJumpData (JumpHeight,TimeToJumpApex);
	}
		
		
	internal void Update()
	{
		if (!m_physics.CanMove)
			return;
		
		if (IsCurrentPlayer && m_autonomy)
			PlayerMovement();
		else if (m_targetSet)
			NpcMovement();
		if (playFootsteps)
			PlayStepSounds ();
		MoveSmoothly();
	}

	private void PlayStepSounds() {
		if (m_inputMove.x != 0f && m_physics.OnGround) {
			m_sinceStep += Time.deltaTime;
			if (m_sinceStep > footstepInterval) {
				m_sinceStep = 0f;
				FindObjectOfType<AudioManager> ().PlayClipAtPos (FXBody.Instance.SFXFootstep,transform.position,0.2f,0f,0.25f);
			}
		} else {
			m_sinceStep = footstepInterval;
		}
	}
	protected virtual void PlayerMovement() {
		m_inputMove = new Vector2(0f, 0f);
		if (InputManager.GetButton("Left"))
			m_inputMove.x -= 1f;
		if (InputManager.GetButton("Right"))
			m_inputMove.x += 1f;
		if (InputManager.GetButton("Up"))
			m_inputMove.y += 1f;
		if (InputManager.GetButton ("Down")) {
			m_inputMove.y -= 1f;
			if (InputManager.GetButtonDown ("Down")) {
				//Debug.Log (Time.timeSinceLevelLoad + " : " + m_lastDownTime);
				if (Time.timeSinceLevelLoad - m_lastDownTime < DOUBLE_TAP_DROP_INTERVAL)
					m_physics.setDropTime (0.2f);
				m_lastDownTime = Time.timeSinceLevelLoad;
			}
		}
		m_jumpDown = InputManager.GetButtonDown ("Jump");
		m_jumpHold = InputManager.GetButton ("Jump");
		JumpMovement ();
		SetDirectionFromInput();
	}

	protected void JumpMovement() {
		if (m_jumpDown) {
			if (m_inputMove.y < -0f) {
				m_physics.setDropTime(0.2f);
			}
			else {
				AttemptJump ();
			}
		}
		float dt = (Time.timeSinceLevelLoad - m_lastJump);
		if (VariableJumpHeight && dt > 0.1f && dt < 0.2f && m_jumpHold && !variableJumpApplied) {
			variableJumpApplied = true;
			applyJumpVector (new Vector2(1f, 0.35f));
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
		
	protected void SetDirectionFromInput()
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
		if ((Time.timeSinceLevelLoad - m_lastJump) < MIN_JUMP_INTERVAL)
			return;

		if (!Submerged && !m_physics.OnGround)
			return;

		if (Submerged)
			applyJumpVector (new Vector2 (1f, 0.6f));
		else if (VariableJumpHeight)
			applyJumpVector (new Vector2 (1f, 0.8f));
		else
			applyJumpVector (new Vector2 (1f, 1f));

		FindObjectOfType<AudioManager> ().PlayClipAtPos (FXBody.Instance.SFXJump,transform.position,0.3f,0f,0.25f);
		m_lastJump = Time.timeSinceLevelLoad;
		variableJumpApplied = false;
	}
	
	private void applyJumpVector(Vector2 scale) {
		float y = jumpVector.y;
		Vector2 jv = new Vector2 (jumpVector.x, y ); //- Mathf.Max (0, m_physics.TrueVelocity.y / Time.deltaTime));
		jv.x *= scale.x;
		jv.y *= scale.y;
		m_physics.AddSelfForce (jv, 0f);
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
				m_physics.setDropTime(0.2f);
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
		JumpHeight = jumpHeight;
		TimeToJumpApex = timeToJumpApex;
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		m_physics.SetGravityScale (gravity * (1.0f/60f));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		jumpVector = new Vector2 (0f, jumpVelocity);
	}
}