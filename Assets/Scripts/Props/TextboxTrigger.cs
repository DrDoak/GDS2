using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxTrigger : Interactable {
	


	public bool typeText = true;
	public bool autoTrigger = true;
	public bool FloatingHitbox = true;
	float interval = 2.0f;
	float currentInterval = 0.0f;

	// Update is called once per frame
	void Update () {
		mUpdate ();
	}
	protected void mUpdate() {
		if (currentInterval > 0.0f) {
			currentInterval -= Time.deltaTime;
		}
		destroyAfterUse ();
	}
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 1, .5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
	internal void OnTriggerEnter2D(Collider2D other) {
		if (autoTrigger && other.gameObject.GetComponent<BasicMovement> () && currentInterval <= 0.0f) {
			triggerText ();
		}
	}
	protected virtual void triggerText() {
		TriggerUsed = true;
		currentInterval = interval;
		if (FloatingHitbox)
			TextboxManager.StartSequence (value,gameObject);
		else
			TextboxManager.StartSequence (value);
	}
	/* public override void onInteract(BasicMovement interactor) {
		if (!autoTrigger) {
			triggerText ();
		}
	} */
}
