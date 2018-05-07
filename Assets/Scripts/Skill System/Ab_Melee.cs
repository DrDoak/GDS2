using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Melee : Ability {

    private int upgradeIndex;
    public float Damage { get; protected set; }
    public float Speed { get; protected set; }

    private float damageUpgrade = 15f;
    private float speedUpgrade = 1f;

	new public void Awake()
	{
		ClearLists();
		AbilityClassification = AbilityType.COMBAT;
        upgradeIndex = 0;
        Damage = Player.GetComponent<AttackInfo>().m_HitboxInfo.Damage;
        Speed = Player.GetComponent<AttackInfo>().m_AttackAnimInfo.AnimSpeed;
	}

	public override void UseAbility()
	{
		Player.GetComponent<Fighter> ().TryAttack ("melee");
	}

    public override void Upgrade()
    {
        switch (upgradeIndex)
        {
            case 0:
            case 2:
            case 4:
                Damage += damageUpgrade;
                UpdateFighter();
                break;
            case 1:
            case 3:
            case 5:
                Speed += speedUpgrade;
                UpdateFighter();
                break;
        }
        upgradeIndex++;
    }

    private void UpdateFighter()
    {
        AttackInfo[] attacks = Player.GetComponents<AttackInfo>();
        foreach(AttackInfo a in attacks)
        {
            a.m_HitboxInfo.Damage = Damage;
            a.m_AttackAnimInfo.AnimSpeed = Speed;
        }
    }
}
