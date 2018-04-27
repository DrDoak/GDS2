using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CharData {
	public string regID = "Not Assigned";
	public string name;
	public Vector3 pos;
	public float health = 100f;
	public float energy = 100f;
	public string prefabPath;
	public string targetID;
	public string [] propertyList;
	public string [] propertyDescriptions;
	public float[] propertyValues;
	public RoomDirection targetDir;
	public bool IsCurrentCharacter;
	public bool IsFacingLeft;
	public bool IsSheathed;
	public int Experience;
}