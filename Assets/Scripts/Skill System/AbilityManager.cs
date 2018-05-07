using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    public List<GameObject> AbilityObjects;
    public List<string> StateNames;

    public static AbilityTree abilityTree;

    public const int LOOT = 0;
    public const int INFECT = 1;

	// Use this for initialization
	void Start () {
        Ability.Manager = this;
        abilityTree = new AbilityTree();
	}
	
    public GameObject GetObject(int i)
    {
        return AbilityObjects[i];
    }
    public string GetStateName(int i)
    {
        return StateNames[i];
    }
}
