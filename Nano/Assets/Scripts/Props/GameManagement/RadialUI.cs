using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialUI : MonoBehaviour
{
    private float fillAmount;

    [SerializeField] private Image content;
    [SerializeField] private float fillRate;
    [SerializeField] private float emptyRate;

    void Start()
    {
        content.fillAmount = 0;
    }

    void Update()
    {
        //Temporary testing code for the radial UI things
        if (Input.GetKey(KeyCode.P))
        {
            if(fillAmount < 100)
                fillAmount += fillRate;

            content.fillAmount = fillAmount/100;
        }
        else
        {
            if (fillAmount > 0)
                fillAmount -= emptyRate;

            content.fillAmount = fillAmount/100;
        }
    }

    //What should actually be used to modify the circles
    public void CircleHandler(float min, float max, float currPercentage)
    {
        fillAmount = Mathf.Lerp(min, max, currPercentage);
        content.fillAmount = fillAmount;
    }


}

