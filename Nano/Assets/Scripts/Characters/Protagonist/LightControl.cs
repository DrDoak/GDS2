using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {

	private enum holdingLight
	{
		f,
		l,
		none
	}

    public GameObject lantern, flashlight;
    public float lanternCharge, flashlightCharge;
    public KeyCode lanternKey, flashlightKey;
    private bool lanternOn, flashlightOn;
	private holdingLight equippedLight;

    private float negateLantern = -1f, negateFlash = -1f;
    public float lanternFullCharge = 15f, flashlightFullCharge = 10f;

	private Fighter fighterSiblingScript;

    void Start () {
		fighterSiblingScript = GetComponent<Fighter>();

        lanternCharge = lanternFullCharge;
        flashlightCharge = flashlightFullCharge;

		changeEquippedLight(holdingLight.f);
		turnOnFlashlight();
		turnOffLantern();
	}

	void Update () {

		//TODO: Possible bugged behavior if player presses lanternkey and flashlightkey on same frame
		//      This might be fixed by putting it in a else if or a switch case.
        if (Input.GetKeyDown(lanternKey))
        {
			changeEquippedLight(holdingLight.l);
			togglePlayerLightSource("lantern");
        }

        if (Input.GetKeyDown(flashlightKey))
        {
			changeEquippedLight(holdingLight.f);
			togglePlayerLightSource("flashlight");
        }
			
		//Running out of lantern charge
        if (lanternCharge >= lanternFullCharge && !lanternOn)
            lanternCharge = lanternFullCharge;
        else if (lanternCharge <= 0f && lanternOn)
        {
			turnOffLantern();
        }
        else
            lanternCharge += Time.deltaTime * negateLantern;
        
		//Running out of flashlight charge
        if (flashlightCharge >= flashlightFullCharge && !flashlightOn)
            flashlightCharge = flashlightFullCharge;
        else if (flashlightCharge <= 0f && flashlightOn)
        {
			turnOffFlashlight();
        }
        else
            flashlightCharge += Time.deltaTime * negateFlash;
        
    }
		
	//This function only handles the act of turning a light on and off
	private void togglePlayerLightSource(string playerLight)
	{
		switch (playerLight)
		{
		case "lantern":
			turnOffFlashlight();
			if (lanternOn)
			{
				turnOffLantern();
			} else {
				turnOnLantern();
			}
			break;

		case "flashlight":
			turnOffLantern();
			if (flashlightOn)
			{
				turnOffFlashlight();
			} else {
				turnOnFlashlight();
			}
			break;

		default:
			Debug.Log ("togglePlayerLightSource was called with an invalid playerLight: " + playerLight);
			break;
		}
	}

	private void turnOnFlashlight()
	{
		flashlight.SetActive(true);
		flashlightOn = true;
		negateFlash = -1f;
	}

	private void turnOffFlashlight()
	{
		flashlight.SetActive(false);
		flashlightOn = false;
		negateFlash = 1f;
	}

	private void turnOnLantern()
	{
		lantern.SetActive(true);
		lanternOn = true;
		negateLantern = -1f;
	}

	private void turnOffLantern()
	{
		lantern.SetActive(false);
		lanternOn = false;
		negateLantern = 1f;
	}

	//This function handles switching the character sprite and any game manager hooks for equipment
	private void changeEquippedLight(holdingLight e)
	{
		if (e != equippedLight)
		{
			equippedLight = e;
			switch (e)
			{
			case holdingLight.f:
				fighterSiblingScript.IdleAnimation = "f_idle";
				fighterSiblingScript.WalkAnimation = "f_walk";
				break;
			case holdingLight.l:
				fighterSiblingScript.IdleAnimation = "l_idle";
				fighterSiblingScript.WalkAnimation = "l_walk";
				break;
			default:
				break;
			}
		}
	}
}
