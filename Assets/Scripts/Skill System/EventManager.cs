using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public const int USE_ABILITY = 0;

    public delegate void VoidDelegate();

    public static event VoidDelegate UseAbilityEvent;

    public static void TriggerEvent(int index)
    {
        UseAbilityEvent();
    }

    
}
