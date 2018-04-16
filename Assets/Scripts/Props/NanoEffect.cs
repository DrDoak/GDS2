using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoEffect : MonoBehaviour {

	// Use this for initialization
	void OnDestroy() {
		Debug.Log (GetComponent<ChaseTarget> ().Target);
		if (GetComponent<ChaseTarget> ().Target != null) {
			GameObject go = Instantiate (GameManager.Instance.FXPropertyGetPrefab, GetComponent<ChaseTarget> ().Target.transform.position, Quaternion.identity);
			go.GetComponent<Follow> ().followObj = GetComponent<ChaseTarget> ().Target.gameObject;
		}
	}
}
