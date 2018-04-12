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
    
    public void AbsorbAbility(Ability a)
    {
        _mCombatControl.SlotAbility(ChooseKeySlot(), a);
    }

    public KeyCode ChooseKeySlot()
    {

        return KeyCode.Space;
    }
}
