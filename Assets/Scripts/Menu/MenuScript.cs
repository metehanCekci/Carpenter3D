using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{
    
    [SerializeField] Canvas canv;
    

    public void startGame(){
        canv.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }
    
    public void quitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }


}
