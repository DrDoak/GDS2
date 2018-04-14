using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoEffect : MonoBehaviour {

	// Use this for initialization
	void OnDestroy() {
		if (GetComponent<ChaseTarget>().Target != null)
			Instantiate (GameManager.Instance.FXPropertyGetPrefab, GetComponent<ChaseTarget>().Target.transform.position, Quaternion.identity);
	}
}
