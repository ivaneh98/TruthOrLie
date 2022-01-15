using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWrong : MonoBehaviour
{
    public Image fillImage;
    public int time;
    protected float maxValue = 1f, minValue = 0f;
    private float currentValue = 0f;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            // Ensure the passed value falls within min/max range
            currentValue = Mathf.Clamp(value, minValue, maxValue);

            // Calculate the current fill percentage and display it
            float fillPercentage = currentValue / maxValue;
            fillImage.fillAmount = fillPercentage;
            //displayText.text =;
        }
    }
    void Start()
    {
        CurrentValue = 0f;
    }

    private void FixedUpdate()
    {
        CurrentValue += 1.0f / time * Time.fixedDeltaTime;

    }
    private void OnEnable()
    {
        CurrentValue = 0f;
    }
}
