/*
 * Data in SessionPersistentData persists between scenes.
 */
using UnityEngine;


public class SessionPersistentData {

	//Members to hold data for scene transitions
	public string LastScene { get; set; }
	public Vector2 ToCoords { get; set; }
}

