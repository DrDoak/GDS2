/*
 * GameProgress class is used a data store for game progression.
 * The game manager implements a private instance of this.
 * The game manager also serializes it to disk when closing the game.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress {
	public enum HubWorldDoorStatus{
		locked,
		unlocked,
		completed
	}

	//List representing the states of all doors in the HubWorld
	private List<HubWorldDoorStatus> doorStates;

	public HubWorldDoorStatus GetDoorState(int d){
		if (d < doorStates.Count && d >= 0)
		{
			return doorStates [d];
		}
		else
		{
			Debug.Log("Bad Call to GetDoorState: out of list index");
			return HubWorldDoorStatus.unlocked;
		}
	}

	public void SaveToDisk(){
		//TODO: Save logic
	}

	public void LoadFromDisk(){
		//TODO: Load logic
	}

	public GameProgress(){
		//LoadFromDisk()
		doorStates = new List<HubWorldDoorStatus>();
		doorStates.Add (HubWorldDoorStatus.unlocked);
		doorStates.Add (HubWorldDoorStatus.completed);
	}
		
	~GameProgress(){
		//SaveToDisk() ?
	}
}
