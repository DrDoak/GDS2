using UnityEngine;
using System.Collections.Generic; //REMEMBER! In order to use lists! Make sure it is System.Collections.Generic instead of System.Collections
// Example of an object that can affect the physics of another object.
public class HitboxDoT : Hitbox {
	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		Tick ();
	}
	protected void Tick() {
		if (!m_hasDuration || Duration > 0.0f) {
			foreach(Attackable a in m_overlappingControl) {
				a.TakeHit (this);
			}
			Duration = Duration - Time.deltaTime;
		} else if (m_hasDuration) {
			GameObject.Destroy (gameObject);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (transform.position, transform.lossyScale);
	}

	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Attackable>() && !m_overlappingControl.Contains(other.gameObject.GetComponent<Attackable> ())) {
			m_overlappingControl.Add (other.gameObject.GetComponent<Attackable> ()); 
		}
	} 
	internal void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.GetComponent<Attackable> () && m_overlappingControl.Contains(other.gameObject.GetComponent<Attackable> ())) {
			m_overlappingControl.Remove (other.gameObject.GetComponent<Attackable> ()); //Removes the object from the list
		}
	}
}
