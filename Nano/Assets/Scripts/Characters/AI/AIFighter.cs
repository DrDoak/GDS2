using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighter {
	public Fighter Fighter { get; set; }
	public BasicMovement BasicMove { get; set; }
	public Attackable Attackable { get; set; }
	public FighterRoutine Routine { get; set; }
}