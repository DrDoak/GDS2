using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Luminosity.IO;

/// <summary>
/// Combat Control manages the keyed abilities of the Player
/// </summary>
public class CombatControl : MonoBehaviour
{

    public List<string> keys;
    public Dictionary<string, Ability> SlottedAbilities;
    private string ButtonPressed;

    // Use this for initialization
    void Start()
    {
        SlottedAbilities = new Dictionary<string, Ability>();
        foreach (string s in keys)
        {
            SlottedAbilities.Add(s, null);
        }

        SlotAbility("Ability1", ScriptableObject.CreateInstance<Ab_Melee>());
       // SlotAbility(keys[1], ScriptableObject.CreateInstance<Ab_Forcepush>());
        SlotAbility("Transfer", ScriptableObject.CreateInstance<Ab_Transfer>());

        Ability.Player = gameObject;
        AbilityTree.Player = gameObject;

        AbilityManager.abilityTree.AddRoot(SlottedAbilities["Transfer"]);
    }

    // Update is called once per frame
    void Update()
    {
		if (!PauseGame.isPaused && Input.anyKeyDown)
            CheckKey();
    }

    void CheckKey()
    {
		foreach (string s in SlottedAbilities.Keys)
        {
			if (InputManager.GetButtonDown(s))
            {
				ButtonPressed = s;
                EventManager.TriggerEvent(EventManager.USE_ABILITY);
                break;
            }
        }
    }

    /// <summary>
    /// Calls the queued ability using the abstract methods in Ability
    /// </summary>
    public void UseAbility()
    {
        //Debug.Log("Using ability: " + KeyPressed);
		Ability a = SlottedAbilities[ButtonPressed];
		if (a != null) {
			a.UseAbility ();
		}
    }

    /// <summary>
    /// Slots the passed ability into the Player's designated keyslot
    /// </summary>
    /// <param name="k"></param>
    /// <param name="a"></param>
	public void SlotAbility(String s, Ability a)
    {
        try
        {
            SlottedAbilities.Add(s, a);
            keys.Add(s);
        }
        catch (ArgumentException)
        {
            SlottedAbilities[s] = a;
        }
        SlottedAbilities[s].Creator = gameObject;
        UpdateBasicAbilityControl();
    }

    private void UpdateBasicAbilityControl()
    {
        GetComponent<BasicAbilityControl>().Abilities = SlottedAbilities.Values.OfType<Ability>().ToList();
    }

    void OnEnable()
    {
        EventManager.UseAbilityEvent += UseAbility;
    }

    void OnDisable()
    {
        EventManager.UseAbilityEvent -= UseAbility;
    }
}
