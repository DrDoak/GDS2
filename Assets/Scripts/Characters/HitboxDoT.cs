using UnityEngine;
using System.Collections.Generic; //REMEMBER! In order to use lists! Make sure it is System.Collections.Generic instead of System.Collections
// Example of an object that can affect the physics of another object.
public class HitboxDoT : Hitbox {
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		Tick ();
	}
	protected void Tick() {
		if (Duration > 0.0f) {
			foreach(Attackable a in m_overlappingControl) {
				a.DamageObj (Damage * Time.deltaTime);
				if (IsFixedKnockback) {
					//cont.addToVelocity (knockback * Time.deltaTime);
				} else {
					Vector3 otherPos = a.gameObject.transform.position;
					float angle = Mathf.Atan2 (transform.position.y - otherPos.y, transform.position.x - otherPos.x); //*180.0f / Mathf.PI;
					float magnitude = Knockback.magnitude;
					float forceX = Mathf.Cos (angle) * magnitude;
					float forceY = Mathf.Sin (angle) * magnitude;
					Vector2 force = new Vector2 (-forceX, -forceY);
					a.GetComponent<PhysicsSS>().AddSelfForce (force,Time.deltaTime);
				}
			}
			Duration = Duration - Time.deltaTime;
		} else {
			GameObject.Destroy (gameObject);
		}
		base.Tick ();
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}

	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Attackable>()) {
			m_overlappingControl.Add (other.gameObject.GetComponent<Attackable> ()); 
		}
	} 
	internal void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<Attackable> ()) {
			m_overlappingControl.Remove (other.gameObject.GetComponent<Attackable> ()); //Removes the object from the list
		}
	}
}
