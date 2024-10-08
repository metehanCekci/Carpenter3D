using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleScript : MonoBehaviour
{
    // Bir TMP_Text nesnesi referansı (TextMeshPro)
    public TMP_Text subtitleText;

    // Altyazıları ve bekleme sürelerini tutan yapı
    [System.Serializable]
    public struct Subtitle
    {
        public float waitTime;
        public float subtitleDuration;
        public string message;
    }

    public List<Subtitle> subtitles;

    public void playSub()
    {
        // Coroutine başlat
        StartCoroutine(DisplaySubtitles());
    }

    IEnumerator DisplaySubtitles()
    {
        foreach (Subtitle subtitle in subtitles)
        {
            yield return new WaitForSeconds(subtitle.waitTime);
            yield return StartCoroutine(FadeInText(subtitle.message, 0.5f)); // 1 saniyede fade in
            yield return new WaitForSeconds(subtitle.subtitleDuration-1);
            yield return StartCoroutine(FadeOutText(0.5f)); // 1 saniyede fade out
        }
    }

    IEnumerator FadeInText(string message, float duration)
    {
        subtitleText.text = message;
        Color color = subtitleText.color;
        color.a = 0f;
        subtitleText.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration);
            subtitleText.color = color;
            yield return null;
        }

        color.a = 1f;
        subtitleText.color = color;
    }

    IEnumerator FadeOutText(float duration)
    {
        Color color = subtitleText.color;
        color.a = 1f;
        subtitleText.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / duration));
            subtitleText.color = color;
            yield return null;
        }

        color.a = 0f;
        subtitleText.color = color;
    }
}
