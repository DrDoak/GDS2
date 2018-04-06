using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControl : MonoBehaviour {

    private CombatControl _mCombatControl;

	// Use this for initialization
	void Start () {
        _mCombatControl = GetComponent<CombatControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
