using UnityEngine;
using System.Collections.Generic;

public class DontDestroyOnLoadManager : MonoBehaviour
{
    private static DontDestroyOnLoadManager instance;
    private List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

    public static DontDestroyOnLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject managerObject = new GameObject("DontDestroyOnLoadManager");
                instance = managerObject.AddComponent<DontDestroyOnLoadManager>();
                DontDestroyOnLoad(managerObject);
            }
            return instance;
        }
    }

    public void Register(GameObject obj)
    {
        if (!dontDestroyOnLoadObjects.Contains(obj))
        {
            dontDestroyOnLoadObjects.Add(obj);
            DontDestroyOnLoad(obj);
        }
    }

    public void ResetAll()
    {
        foreach (GameObject obj in dontDestroyOnLoadObjects)
        {
            Destroy(obj);
        }
        dontDestroyOnLoadObjects.Clear();
    }
}
