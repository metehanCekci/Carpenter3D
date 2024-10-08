using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{


    [SerializeField] Canvas pauseMenuUI;
    [SerializeField] Canvas settingsMenuUI;
    [SerializeField] GameObject pauseBack;
    [SerializeField] Canvas playerUI;
    [SerializeField] AudioSource Combat;
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement pm;
    [SerializeField] AudioSource Calm;
    PlayerInputActions inputActions;
    [HideInInspector] public bool isPaused;
    Camera cam;
    Scene currentScene;
    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        cam = Camera.main;
        inputActions = new PlayerInputActions();
        isPaused = false;
    }

    public void ResumePauseGame(InputAction.CallbackContext context)
    {
        if (!isPaused && !pm.isDead)
        {

            // Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            pauseMenuUI.gameObject.SetActive(true);
            settingsMenuUI.gameObject.SetActive(false);
            pauseBack.gameObject.SetActive(true);
            playerUI.gameObject.SetActive(false);
            cam.GetComponent<CameraController>().enabled = false;
            pm.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            isPaused = true;
        }
        else if (isPaused)
        {
            Time .timeScale = 1;
            pauseBack.gameObject.SetActive(false);
            pauseMenuUI.gameObject.SetActive(false);
            settingsMenuUI.gameObject.SetActive(false);
            playerUI.gameObject.SetActive(true);
            cam.GetComponent<CameraController>().enabled = true;
            pm.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
        }
        else return;





    }

    //BUTONA EYLEMİ TANITABİLMEK İÇİN GEREKLİ ;(((((((
    public void ResumeButton()
    {
        pauseMenuUI.gameObject.SetActive(false);
        settingsMenuUI.gameObject.SetActive(false);
        pauseBack.gameObject.SetActive(false);
        playerUI.gameObject.SetActive(true);
        cam.GetComponent<CameraController>().enabled = true;
        pm.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time .timeScale = 1;
    }

    public void quitHomeButton()
    {
        
        Destroy(GameObject.FindGameObjectWithTag("AudioDontDestroy"));
        SceneManager.LoadScene(0);
    }

    //
    // #TODO: fix checkpoint system   IMPORTANT!!!!
    //

    public void reloadScene()
    {

        pm.ResetPlayerInput();
        pm.enabled = true;
        Time.timeScale = 1;
        cam.GetComponent<CameraController>().enabled = true;
        pauseMenuUI.gameObject.SetActive(false);
        settingsMenuUI.gameObject.SetActive(false);
        pauseBack.gameObject.SetActive(false);
        playerUI.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(currentScene.buildIndex);
        isPaused = false;
        pm.isDead = false;
        
    }
    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }



    void OnEnable()
    {
        inputActions.Player.Menu.performed += ResumePauseGame;
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Menu.performed -= ResumePauseGame;
        inputActions.Player.Disable();
    }
}
