using UnityEngine;

public class DeleteObjects : MonoBehaviour
{
    void Start()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("DestroyOnStart");
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }
}
