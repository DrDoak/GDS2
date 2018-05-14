using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveller : MonoBehaviour {

    public static Leveller instance;

    public int Level;
    public int Scaler = 2;
    public int DataRequirement = 800;
    public int PointAdditions = 2;
    public float HealthAddition = 50f;
    
    private ExperienceHolder exp;

	// Use this for initialization
	void Start () {
        Level = 1;
        instance = this;
	}
    /// <summary>
    /// Takes in an Experience Holder for correct exp updates, levels up if required
    /// </summary>
    /// <param name="obj"></param>
    public static void UpdateExperience(ExperienceHolder obj)
    {
        if (instance.exp == null || instance.exp != obj)
            instance.exp = obj;

        if (instance.exp.Experience >= instance.DataRequirement * instance.Scaler* instance.Level)
            EventManager.TriggerEvent();
    }

    void IncreaseHealth()
    {
        exp.gameObject.GetComponent<Attackable>().MaxHealth += HealthAddition * Level;
    }

    void DisplayLevelUp()
    {
        Debug.Log("congrats");
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

    void OnEnable()
    {
        EventManager.LevelUpEvent += IncreaseHealth;
        EventManager.LevelUpEvent += AddAbilityPoints;
        EventManager.LevelUpEvent += DisplayLevelUp;
        EventManager.LevelUpEvent += AddTransferSlots;
    }

    void OnDisable()
    {
        EventManager.LevelUpEvent -= IncreaseHealth;
        EventManager.LevelUpEvent -= AddAbilityPoints;
        EventManager.LevelUpEvent -= DisplayLevelUp;
        EventManager.LevelUpEvent -= AddTransferSlots;
    }
}
