using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence  {
	public int numChars = 0;
	public string rawText = "";
	public List<DialogueUnit> allDUnits;
	public DialogueSequence parentSequence = null;
	public GameObject Speaker;

	public void advanceSequence() {
		DialogueUnit currDialogueUnit = allDUnits [0];
		currDialogueUnit.Speaker = Speaker;
		currDialogueUnit.startSequence ();
		allDUnits.Remove (currDialogueUnit);
	}

}
