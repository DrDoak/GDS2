using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceHolder : MonoBehaviour {
	public int Experience;
	public void AddExperience(int value) {
		Experience += value;
	}
}
