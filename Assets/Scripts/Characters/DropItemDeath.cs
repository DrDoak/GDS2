using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemDeath : MonoBehaviour {
	public GameObject ItemDropped;
	// Use this for initialization
	void Update() {
		if (ItemDropped != null && !GetComponent<Attackable>().Alive) {
			Instantiate (ItemDropped, transform.position, Quaternion.identity);
			ItemDropped = null;
		}
	}
}