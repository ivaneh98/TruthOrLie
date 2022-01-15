using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialFill : MonoBehaviour
{

    // Public UI References
    public Image fillImage;
    public TMP_Text displayText;
    public int time = 30;
    public int currentTime;
    private bool isActive = false;
    // Trackers for min/max values
    protected float maxValue = 1f, minValue = 0f;

    // Create a property to handle the slider's value
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
        currentTime = time;
        CurrentValue = 1f;
        displayText.text = currentTime.ToString();
        
    }
    private void Update()
    {
        if(currentTime < 0)
        {
            currentTime = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isActive)
            CurrentValue -= 1.0f / time * Time.fixedDeltaTime;
        else
            displayText.text = "";
    }
    IEnumerator Timer()
    {
        while (currentTime>0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            displayText.text = currentTime.ToString();
        }
    }
    public void Restart()
    {
        currentTime = time;
        StopCoroutine("Timer");
        CurrentValue = 1f;
        isActive = false;
    }
    public void Unpause()
    {
        isActive = true;
        displayText.text = currentTime.ToString();
        StartCoroutine("Timer");
    }


}