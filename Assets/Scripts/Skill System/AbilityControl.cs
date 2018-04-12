using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControl : MonoBehaviour {

    private CombatControl _mCombatControl;
    private BasicAbilityControl _mBasicControl;

	// Use this for initialization
	void Start () {
        _mCombatControl = GetComponent<CombatControl>();
        _mBasicControl = GetComponent<BasicAbilityControl>();
	}
	
    
    public void AbsorbAbility(Ability a)
    {
        _mCombatControl.SlotAbility(ChooseKeySlot(), a);
    }

    public KeyCode ChooseKeySlot()
    {
        Debug.Log("Choose keyslot");
        return KeyCode.J;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PropertyHolder>())
        {
            foreach (Ability a in _mBasicControl.Abilities)
                a.SetTarget(collision.gameObject);
        }
    }

}
