using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeReaction : LightDetection {

    public Sprite SpriteInDarkness;
    public Sprite SpriteInLight;

    public override void React()
    {
        GetComponent<SpriteRenderer>().sprite = SpriteInLight;
    }

    public override void StopReact()
    {
        GetComponent<SpriteRenderer>().sprite = SpriteInDarkness;
    }
}
