using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveller : MonoBehaviour {

    public static Leveller Instance;

    public int Level;
    public int Scaler = 2;
    public int DataRequirement = 800;
    public int PointAdditions = 2;
    public float HealthAddition = 50f;
    
    private ExperienceHolder exp;

	void Start()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Level = 1;
		for (int i = 1; i < 10; i++) { 
			Debug.Log (Instance.DataRequirement * Instance.Scaler * i);
		}
	}

    /// <summary>
    /// Takes in an Experience Holder for correct exp updates, levels up if required
    /// </summary>
    /// <param name="obj"></param>
    public static void UpdateExperience(ExperienceHolder obj)
    {
		if (Instance.exp == null || Instance.exp != obj)
			Instance.exp = obj;
		//Debug.Log ("Requirement: " + Instance.DataRequirement * Instance.Scaler * Instance.Level);
		//Debug.Log ("Current: " + Instance.exp.Experience);
		if (Instance.exp.Experience >= Instance.DataRequirement * Instance.Scaler * Instance.Level) {
			EventManager.TriggerEvent (3);
			Debug.Log ("Event Triggered");
		}
    }

    void IncreaseHealth()
    {
        exp.gameObject.GetComponent<Attackable>().MaxHealth += HealthAddition * Level;
    }

    void DisplayLevelUp()
    {
		TextboxManager.StartSequence ("~RANK UP! LEVEL " + (Instance.Level + 1));
		Instance.Level++;
    }

    void AddAbilityPoints()
    {
        AbilityTree.PointsToSpend += PointAdditions;
    }

    void AddTransferSlots()
    {
        //Add slots at levels 3, 6, 9
        if(Level % 3 == 0)
            exp.gameObject.GetComponent<PropertyHolder>().MaxSlots += 1;
    }

	void AddTransfers()
	{
		//Add slots at levels 2,3, 6, 9 TEMPORARY
		if(Level == 2 || Level % 3 == 0)
			exp.gameObject.GetComponent<PropertyHolder>().NumTransfers += 1;
	}

    void OnEnable()
    {
		Debug.Log ("on enable");
        EventManager.LevelUpEvent += IncreaseHealth;
        EventManager.LevelUpEvent += AddAbilityPoints;
        EventManager.LevelUpEvent += DisplayLevelUp;
        EventManager.LevelUpEvent += AddTransferSlots;
		EventManager.LevelUpEvent += AddTransfers;
    }

    void OnDisable()
    {
		Debug.Log ("on disable");
        EventManager.LevelUpEvent -= IncreaseHealth;
        EventManager.LevelUpEvent -= AddAbilityPoints;
        EventManager.LevelUpEvent -= DisplayLevelUp;
        EventManager.LevelUpEvent -= AddTransferSlots;
		EventManager.LevelUpEvent -= AddTransfers;
    }
}
