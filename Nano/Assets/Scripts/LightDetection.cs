using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightDetection : MonoBehaviour {

    SpriteRenderer _spr;
    GameObject player;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");
    }

    public void OnTriggerEnter2D(Collider2D c)
    {
        //Calls function with specific reaction
        React();
    }

    public void OnTriggerExit2D(Collider2D c)
    {
        //Calls function to stop specific reaction
        StopReact();
    }

    public abstract void React();

    public abstract void StopReact();

    private void Update()
    {
        if(player.GetComponent<SpriteRenderer>().sortingOrder < transform.localScale.y*100f + _spr.sortingOrder )//&& (player.transform.position.x < transform.position.x))
        {
            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 8;
        }
    }
}
