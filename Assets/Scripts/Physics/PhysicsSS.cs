using UnityEngine;
using System.Collections.Generic;

public enum FallDirection { NONE, LEFT, RIGHT}

[RequireComponent (typeof (BoxCollider2D))]
public class PhysicsSS : MonoBehaviour
{
	private const float VELOCITY_MINIMUM_THRESHOLD = 0.3f;
		
	public FallDirection FallDir;
	public LayerMask CollisionMask;

	// Used to decrease / scale velocity vector
	public float DecelerationRatio = 1.0f;

	// Collision detection to adjust velocity vector 
	private BoxCollider2D m_boxCollider;
	private RaycastOrigins m_raycastOrigins;
	private CollisionInfo m_collisions;
	private const float m_skinWidth = .015f;
	private int m_horizontalRayCount = 4;
	private int m_verticalRayCount = 4;
	private float m_horizontalRaySpacing;
	private float m_verticalRaySpacing;
	private float dropThruTime = 0.0f;
	public bool OnGround = true;
	public float MaxClimbAngle = 80;

	// Tracking movement
	private List<ForceSS> m_forces;
	private Vector2 m_accumulatedVelocity = Vector2.zero;
	private bool m_canMove = true;
	public bool CanMove { get { return m_canMove; } set { m_canMove = value; } }
	private Vector2 m_velocity;
	public Vector2 Velocity { get { return m_velocity; } }

	private Vector3 m_trueVelocity;
	public Vector3 TrueVelocity { get { return m_trueVelocity; } }
	private Vector3 m_lastPosition;

	// Tracking inputed movement
	private ForceSS m_inputedForce;
	public Vector2 m_inputedMove = Vector2.zero;
	public Vector2 InputedMove { get { return m_inputedMove; } }
	public float TerminalVelocity = -5f;
	private float m_gravityScale = -1.0f;

	public bool FacingLeft = false;

	// Tracking m_sprite orientation (flipping if left)...
	SpriteRenderer m_sprite;

	internal void Awake()
	{
		m_forces = new List<ForceSS>();
		m_inputedForce = new ForceSS();
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_boxCollider.offset = new Vector2(m_boxCollider.offset.x, m_boxCollider.offset.y + m_skinWidth);
		m_sprite = GetComponent<SpriteRenderer>();
		m_canMove = true;
		m_lastPosition = transform.position;
	}

	internal void Start()
	{
		CalculateRaySpacing();
		SetDirection(FacingLeft);
	}

	internal void FixedUpdate()
	{
		dropThruTime = Mathf.Max(0f,dropThruTime - Time.fixedDeltaTime);
		DecelerateAutomatically(VELOCITY_MINIMUM_THRESHOLD);
		ProcessMovement();
		UpdateTrueVelocity ();
	}

	private void UpdateTrueVelocity() {
		m_trueVelocity = transform.position - m_lastPosition;
		m_lastPosition = transform.position;
	}
	private void DecelerateAutomatically(float threshold)
	{
		if (m_accumulatedVelocity.sqrMagnitude > threshold)
			m_accumulatedVelocity.x *= (1.0f - Time.fixedDeltaTime * DecelerationRatio * 3.0f);
		else
			m_accumulatedVelocity = Vector2.zero;
	}

	private void CheckCanMove()
	{
		if (m_canMove)
			return;
		//m_inputedMove = Vector2.zero;
		m_inputedForce.Force = new Vector2(0, 0);
	}

	private void ApplyForcesToVelocity()
	{
		m_inputedForce.Force *= Time.fixedDeltaTime;
		m_velocity.x = m_inputedForce.Force.x;
		bool Yf = false;

		List<ForceSS> forcesToRemove = new List<ForceSS>();
		foreach (ForceSS force in m_forces)
		{
			m_velocity += force.Force * Time.fixedDeltaTime;
			force.Duration -= Time.fixedDeltaTime;
			if (force.Duration < 0f)
				forcesToRemove.Add(force);
		}

		foreach (ForceSS force in forcesToRemove)
			m_forces.Remove(force);

		if (m_velocity.y > TerminalVelocity){
			if (Yf || !m_collisions.below) {
				m_velocity.y += m_gravityScale * Time.fixedDeltaTime;
			} else if (m_collisions.below) { //force player to stick to slopes
				m_velocity.y += m_gravityScale * Time.fixedDeltaTime * 6f;
			}
		}
		m_velocity.x += m_accumulatedVelocity.x * Time.fixedDeltaTime;
	}

	private void ProcessMovement()
	{
		CheckCanMove();
		ApplyForcesToVelocity();
		UpdateRaycastOrigins();
		m_collisions.Reset();

		if (m_velocity.x != 0 || m_inputedMove.x != 0)
			HorizontalCollisions(ref m_velocity);
		if (m_velocity.y != 0 || m_inputedMove.y != 0)
			VerticalCollisions(ref m_velocity);
		transform.Translate (m_velocity);
	}

	public void AddToVelocity(Vector2 veloc)
	{
		m_accumulatedVelocity += veloc;
	}

	public void AddSelfForce(Vector2 force, float duration)
	{
		m_forces.Add(new ForceSS(force, duration));
	}

	public bool IsAttemptingMovement()
	{
		return m_inputedMove.x != 0.0f;
	}

	public void Move(Vector2 veloc, Vector2 input)
	{
		m_inputedMove = input;
		m_inputedForce.Force = veloc;
	}

	private bool ValidCollision(RaycastHit2D hit)
	{
		return hit && !hit.collider.isTrigger && hit.collider.gameObject != gameObject;
	}

	bool handleStairs(RaycastHit2D hit,Vector2 vel) {
		if ( JumpThruTag(hit.collider.gameObject)) {
			if (dropThruTime > 0f)
				return true;
			if (hit.collider.gameObject.GetComponent<EdgeCollider2D> ()) {
				EdgeCollider2D ec = hit.collider.gameObject.GetComponent<EdgeCollider2D> ();
				Vector2[] points = ec.points;
				Vector2 leftPoint = Vector2.zero;
				bool foundLeft = false;
				Vector2 rightPoint = Vector2.zero;
				bool foundRight = false;
				foreach (Vector2 p in points) {
					float xDiff = (ec.transform.position.x + p.x) - transform.position.x;
					if (xDiff < -0.01f) {
						if (foundRight) {
							float yDiff = p.y - rightPoint.y;
							if (yDiff > 0.01f) {
								if (vel.x > 0f) {
									return true;
								} else {
									return false;
								}
							} else if (yDiff < -0.01f) {
								if (vel.x > 0f) {
									return false;
								} else {
									return true;
								}
							}
						} else {
							if (Vector2.Equals(Vector2.zero,leftPoint)) {
								leftPoint = p;
								foundLeft = true;
							}
						}
					} else if (xDiff > 0.01f) {
						if (foundLeft) {
							float yDiff = p.y - leftPoint.y;
							if (yDiff > 0.01f) {
								if (vel.x < 0f) {
									return true;
								} else {
									return false;
								}
							} else if (yDiff < -0.01f) {
								if (vel.x < 0f) {
									return false;
								} else {
									return true;
								}
							}
						} else {
							if (Vector2.Equals(Vector2.zero,rightPoint)) {
								rightPoint = p;
								foundRight = true;
							}
						}
					}
				}
			} else {
				return true;
			}
		}
		return false;
	}

	void HorizontalCollisions(ref Vector2 velocity) {
		float directionX;
		//Debug.Log ("Horizontal Collisions:" + SelfInput + " : " + velocity);
		if (velocity.x == 0) {
			directionX = Mathf.Sign (m_inputedMove.x);
		} else {
			directionX = Mathf.Sign (velocity.x);
		}
		float rayLength = Mathf.Max(0.05f,Mathf.Abs (velocity.x) + m_skinWidth);

		for (int i = 0; i < m_horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?m_raycastOrigins.bottomLeft:m_raycastOrigins.bottomRight;
			if (i == m_horizontalRayCount - 1) {
				rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);
			} else {
				rayOrigin += Vector2.up * (m_horizontalRaySpacing/2f * i);
			}
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);
			if (hit) {
				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);
			} else {
				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.green);
			}

			if (hit && !hit.collider.isTrigger) {

				if (handleStairs(hit,velocity) ){

				} else {
					float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
					if (i == 0 && slopeAngle <= MaxClimbAngle) {
						float distanceToSlopeStart = 0;
						if (slopeAngle != m_collisions.slopeAngleOld) {
							distanceToSlopeStart = hit.distance - m_skinWidth;
							//velocity.x -= distanceToSlopeStart * directionX;
							velocity.x = (Mathf.Abs(velocity.x) + distanceToSlopeStart) * directionX;
						}
						ClimbSlope (ref velocity, slopeAngle);
						//velocity.x += distanceToSlopeStart * directionX;
						velocity.x = (Mathf.Abs(velocity.x) + distanceToSlopeStart) * directionX;
					}

					if (!m_collisions.climbingSlope || slopeAngle > MaxClimbAngle) {
						velocity.x = (hit.distance - m_skinWidth) * directionX;
						rayLength = hit.distance;

						if (m_collisions.climbingSlope) {
							velocity.y = Mathf.Tan (m_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);
						}

						m_collisions.left = directionX == -1;
						m_collisions.right = directionX == 1;
					}
				}
			}
		}
	}

	void VerticalCollisions(ref Vector2 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + m_skinWidth;

		for (int i = 0; i < m_verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1)?m_raycastOrigins.bottomLeft:m_raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (m_verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);
			if (hit && !hit.collider.isTrigger && hit.collider.gameObject != gameObject) {
				if ( JumpThruTag(hit.collider.gameObject) && ( velocity.y > 0 || dropThruTime > 0f)){ //|| handleStairs(hit,velocity))){
				} else {
					velocity.y = (hit.distance - m_skinWidth) * directionY;
					rayLength = hit.distance;
					if (m_collisions.climbingSlope) {
						velocity.x = velocity.y / Mathf.Tan (m_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
					}

					m_collisions.below = directionY == -1;
					m_collisions.above = directionY == 1;
				}
			}
		}
		FallDir = FallDirection.NONE;
		OnGround = false;
		rayLength = rayLength + 0.1f;
		if (true) {
			bool collide = false;
			bool started = false;
			rayLength = 0.3f;
			for (int i = 0; i < m_verticalRayCount; i++) {
				Vector2 rayOrigin = m_raycastOrigins.bottomLeft; //true ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (m_verticalRaySpacing * i + velocity.x);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * -1f, rayLength, CollisionMask);
				if (hit && JumpThruTag(hit.collider.gameObject) && (dropThruTime > 0f )) {
				} else {
					if (hit && !hit.collider.isTrigger && hit.collider.gameObject != gameObject) {
						////Debug.Log (hit.collider.gameObject);
						Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);
						OnGround = true;
						if ( started && !collide) {
							FallDir = FallDirection.LEFT;
						}
						collide = true;
					}  else {
						Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.green);
						if (started && collide) {
							FallDir = FallDirection.RIGHT;
						}
					}
				}
				started = true;
			}
		}
	}
	public void setDropTime(float tm) {
		dropThruTime = tm;
	}
	void ClimbSlope(ref Vector2 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			m_collisions.below = true;
			m_collisions.climbingSlope = true;
			m_collisions.slopeAngle = slopeAngle;
		}
	}

	private void UpdateRaycastOrigins()
	{
		Bounds bounds = m_boxCollider.bounds;
		bounds.Expand(m_skinWidth);
		m_raycastOrigins.bottomLeft = new Vector2 (bounds.min.x , bounds.min.y);
		m_raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		m_raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		m_raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	private void CalculateRaySpacing()
	{
		Bounds bounds = m_boxCollider.bounds;
		bounds.Expand(m_skinWidth);
		m_horizontalRayCount = Mathf.Clamp (m_horizontalRayCount, 2, int.MaxValue);
		m_verticalRayCount = Mathf.Clamp (m_verticalRayCount, 2, int.MaxValue);
		m_horizontalRaySpacing = bounds.size.y / (m_horizontalRayCount - 1);
		m_verticalRaySpacing = bounds.size.x / (m_verticalRayCount - 1);
	}

	public void SetDirection(bool left) {
		FacingLeft = left;
		if (m_sprite.sprite) {
			if (FacingLeft) {
				m_sprite.flipX = true;
			} else {
				m_sprite.flipX = false;
			}
		}
	}

	public Vector2 OrientVectorToDirection(Vector2 v) {
		if (FacingLeft)
			return new Vector2 (-v.x, v.y);
		return v;
	}

	public void SetGravityScale(float gravScale) {
		m_gravityScale = gravScale;
	}

	private bool JumpThruTag( GameObject obj ) {
		return (obj.CompareTag ("JumpThru") || (obj.transform.parent != null &&
		obj.transform.parent.CompareTag ("JumpThru")));
	}
}