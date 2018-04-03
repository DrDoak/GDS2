using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterTask : MonoBehaviour {
	[HideInInspector]
	public Fighter Player;

	[HideInInspector]
	public AIFighter Fighter;

	public bool Active { get; private set; }

	public void Init(Fighter player, AIFighter fighter) {
		this.Player = player;
		this.Fighter = fighter;
	}

	public void Activate() {
		Active = true;
	}

	virtual public void Advance() {}
}
