using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SyringeScript : MonoBehaviour
{
    public float fillAmt = 0.1f;
    public float healAmt;
    public float brightnessDuration = 0.5f;  // Duration for the brightness effect
    public int startSyringe = 1;
    public TMP_Text syringeCounterText; // Reference to the TMP_Text component for syringe count

    private Image image;
    private Color originalColor;
    private Coroutine currentCoroutine;
    public GlobalHpBar hpBar;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        syringeCounterText.text = startSyringe.ToString();; // Start with an empty string
    }

    void Update()
    {
        
    }

    public void fillSyringe()
    {
        image.fillAmount += fillAmt;
        if (image.fillAmount >= 1)
        {
            IncrementSyringeCounter();
            image.fillAmount = 0; // Reset syringe fill amount to 0
        }
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(BrightnessEffect());
    }

    public void useSyringe()
    {
        if(!string.IsNullOrEmpty(syringeCounterText.text))
        {
            hpBar.hp += healAmt;
            if(hpBar.hp > hpBar.maxHp)
            hpBar.hp = hpBar.maxHp;
            DecrementSyringeCounter();
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(BrightnessEffect());
        }
        
    }

    private void IncrementSyringeCounter()
    {
        int currentCount = string.IsNullOrEmpty(syringeCounterText.text) ? 0 : int.Parse(syringeCounterText.text);
        currentCount++;
        syringeCounterText.text = currentCount > 0 ? currentCount.ToString() : "";
    }

    private void DecrementSyringeCounter()
    {
        int currentCount = string.IsNullOrEmpty(syringeCounterText.text) ? 0 : int.Parse(syringeCounterText.text);
        currentCount--;
        syringeCounterText.text = currentCount > 0 ? currentCount.ToString() : "";
    }

    private IEnumerator BrightnessEffect()
    {
        float elapsedTime = 0f;
        float maxBrightness = 10f;  // Increase this value to make it a lot brighter

        // Instantly set to bright color
        Color brightColor = originalColor * maxBrightness;
        image.color = brightColor;

        // Smoothly fade back to original color
        while (elapsedTime < brightnessDuration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(brightColor, originalColor, elapsedTime / brightnessDuration);
            yield return null;
        }

        image.color = originalColor;
    }
}
