using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleLightReaction : LightDetection{

	// Use this for initialization
	public override void React()
    {
        //Light basic reaction
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public override void StopReact() {
        //Light basic reaction
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
