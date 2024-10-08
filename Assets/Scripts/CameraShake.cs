using UnityEngine;
using System.Collections;


public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance { get; private set; }

        void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;


        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-0.1f, 0.2f) * magnitude;

            transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}
