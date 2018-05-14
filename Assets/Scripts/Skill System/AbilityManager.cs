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
        PopulateTree();
	}
	
    public GameObject GetObject(int i)
    {
        return AbilityObjects[i];
    }
    public string GetStateName(int i)
    {
        return StateNames[i];
    }

    private void PopulateTree()
    {
        //Transfer Root
        abilityTree.AddRoot(ScriptableObject.CreateInstance<Ab_Transfer>());

        //Melee Abilities
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Melee>(), Branch.LEFT);
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_NanoSword>(), Branch.LEFT, AbilityType.COMBAT);
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Passive_AttackRate>(), Branch.LEFT, AbilityType.COMBAT);

        abilityTree.Add(ScriptableObject.CreateInstance<Ab_CriticalStrike>(), Branch.RIGHT, AbilityType.COMBAT);
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Passive_AttackDamage>(), Branch.RIGHT, AbilityType.COMBAT);


        //Transfer Abilities


        //Environmental Abilities
        abilityTree.PrintTree();

    }
}
