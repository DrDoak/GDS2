using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    public static List<GameObject> AbilityObjects;
    public static List<string> StateNames;

	// Use this for initialization
	void Start () {
        Ability.Manager = gameObject;
	}
	
    public static GameObject GetObject(int i)
    {
        return AbilityObjects[i];
    }
    public static string GetStateName(int i)
    {
        return StateNames[i];
    }
}
