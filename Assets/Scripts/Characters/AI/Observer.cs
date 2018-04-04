using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour {

	PhysicsSS m;
	public float detectionRange = 15.0f;

	List<Observable> visibleObjs = new List<Observable>();
	float sinceLastScan;
	float scanInterval = 0.5f;
	float postLineVisibleTime = 3.0f;

	// Use this for initialization
	void Start () {
		m = GetComponent<PhysicsSS> ();
		sinceLastScan = UnityEngine.Random.Range (0.0f, scanInterval);
	}

	void Update() {
		if (sinceLastScan > scanInterval) {
			scanForEnemies ();
		}
		sinceLastScan += Time.deltaTime;
	}

	void scanForEnemies() {
		//Debug.Log (gameObject + " is scanning, found " + allObs.Length);
		Observable[] allObs = FindObjectsOfType<Observable> ();
		float lts = Time.realtimeSinceStartup;
		foreach (Observable o in allObs) {
			Vector3 otherPos = o.transform.position;
			Vector3 myPos = transform.position;
			if (o.gameObject != gameObject && otherPos.x < myPos.x && m.FacingLeft ||
				otherPos.x > myPos.x && !m.FacingLeft) {
				float cDist = Vector3.Distance (otherPos, myPos);
				if (cDist < detectionRange) {
					RaycastHit2D[] hits = Physics2D.RaycastAll (myPos, otherPos - myPos, cDist);
					Debug.DrawRay (myPos, otherPos - myPos, Color.green);
					float minDist = float.MaxValue;
					foreach (RaycastHit2D h in hits) {
						GameObject oObj = h.collider.gameObject;
						if (oObj != gameObject ) {
							minDist = Mathf.Min(minDist,Vector3.Distance (transform.position,h.point));
						}
					}
					float diff = Mathf.Abs (cDist - minDist);
					if (diff < 1.0f) {
						if (!visibleObjs.Contains (o)) {
							//onSight (o);
							o.addObserver (this);
							visibleObjs.Add (o);
						}
					}
				}
			}
		}
		if (visibleObjs.Count > 0) {
			for (int i= visibleObjs.Count - 1; i >= 0; i --) {
				Observable o = visibleObjs [i];
				if (o == null) { // c.gameObject == null) {
					visibleObjs.RemoveAt (i);
				} /* else if (o.c) {
					if (lts - r.lastTimeSeen > postLineVisibleTime) {
						o.removeObserver (this);
						//outOfSight (o, true);
						visibleObjs.RemoveAt (i);
					} else if (Mathf.Abs(lts - r.lastTimeSeen) > 0.05f 
								&& r.canSee == true){
						outOfSight (o, false);
					}	
				} */
			}
		}
		sinceLastScan = 0f;
	}

	void OnDestroy() {
		foreach (Observable o in visibleObjs) {
			o.removeObserver (this);	
		}
	}
}