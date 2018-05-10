using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Combat Control manages the keyed abilities of the Player
/// </summary>
public class CombatControl : MonoBehaviour
{

    public List<KeyCode> keys;
    public Dictionary<KeyCode, Ability> SlottedAbilities;
    private KeyCode KeyPressed;
    public KeyCode TransferKey = KeyCode.Return;
    public KeyCode CombatKey;
    public KeyCode EnvironmentalKey;

    // Use this for initialization
    void Start()
    {
        SlottedAbilities = new Dictionary<KeyCode, Ability>();
        foreach (KeyCode k in keys)
        {
            SlottedAbilities.Add(k, null);
        }
        CombatKey = keys[0];
        SlotAbility(CombatKey, AbilityManager.abilityTree.GetAbility(Branch.LEFT, 1, AbilityType.COMBAT));
       // SlotAbility(keys[1], ScriptableObject.CreateInstance<Ab_Forcepush>());
        SlotAbility(TransferKey, AbilityManager.abilityTree.root.ability);

        Ability.Player = gameObject;
        AbilityTree.Player = gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!PauseGame.isPaused && Input.anyKeyDown)
            CheckKey();
    }

    void CheckKey()
    {
        foreach (KeyCode k in SlottedAbilities.Keys)
        {
            if (Input.GetKeyDown(k))
            {
                KeyPressed = k;
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
        if (KeyPressed == KeyCode.None)
            return;
        //Debug.Log("Using ability: " + KeyPressed);
        Ability a = SlottedAbilities[KeyPressed];
		if (a != null) {
			a.UseAbility ();
		}
    }

    /// <summary>
    /// Slots the passed ability into the Player's designated keyslot
    /// </summary>
    /// <param name="k"></param>
    /// <param name="a"></param>
    public void SlotAbility(KeyCode k, Ability a)
    {
        try
        {
            SlottedAbilities.Add(k, a);
            keys.Add(k);
        }
        catch (ArgumentException)
        {
            SlottedAbilities[k] = a;
        }
        SlottedAbilities[k].Creator = gameObject;
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
