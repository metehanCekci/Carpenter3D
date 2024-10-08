using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public int sceneIndex;

    private void Start()
    {
        // Ensure the fadeImage is initially white and fully opaque
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        fadeImage.gameObject.SetActive(true);

        // Start the fade-in effect
        StartCoroutine(FadeFromWhite());
    }

    public IEnumerator FadeToWhite()
    {
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1);
        GameObject[] group = GameObject.FindGameObjectsWithTag("AudioDontDestroy");
        foreach (GameObject obj in group)
        {
            Destroy(obj);
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public IEnumerator FadeFromWhite()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0);
        fadeImage.gameObject.SetActive(false); // Optionally disable the image after fading in
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeToWhite());
        }
    }
}
