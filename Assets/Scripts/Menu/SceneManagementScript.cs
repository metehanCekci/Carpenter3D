using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagementScript : MonoBehaviour
{
    int currentScene;
    int sceneCount;
    // Start is called before the first frame update
    void Start()
    {   
        currentScene = SceneManager.GetActiveScene().buildIndex;
        sceneCount = SceneManager.sceneCount;
    }

    public void loadScene()
    {

        if (currentScene != sceneCount)
        {
            currentScene++;
            SceneManager.LoadScene(currentScene);
        }

        
    }

    
}
