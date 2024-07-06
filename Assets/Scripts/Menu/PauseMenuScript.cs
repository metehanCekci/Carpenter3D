using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    void Awake()
    {
        cam = Camera.main;
        inputActions = new PlayerInputActions();
        isPaused = false;
    }

    public void ResumePauseGame(InputAction.CallbackContext context)
    {
        if (!isPaused && !pm.isDead)
        {

            // Cursor.lockState = CursorLockMode.None;
            pauseMenuUI.gameObject.SetActive(true);
            settingsMenuUI.gameObject.SetActive(false);
            pauseBack.gameObject.SetActive(true);
            playerUI.gameObject.SetActive(false);
            cam.GetComponent<CameraController>().enabled = false;
            pm.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            isPaused = true;
        }
        else if (isPaused)
        {
            pauseBack.gameObject.SetActive(false);
            pauseMenuUI.gameObject.SetActive(false);
            settingsMenuUI.gameObject.SetActive(false);
            playerUI.gameObject.SetActive(true);
            cam.GetComponent<CameraController>().enabled = true;
            pm.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
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
        Time.timeScale = 1;
        isPaused = false;
    }

    public void quitHomeButton()
    {
        
        Time.timeScale = 1;
        Destroy(GameObject.FindGameObjectWithTag("AudioDontDestroy"));
        SceneManager.LoadScene(0);
    }

    public void reloadScene()
    {
        pm.isDead = false;
        Time.timeScale = 1;
        cam.GetComponent<CameraController>().enabled = true;
        Destroy(GameObject.FindGameObjectWithTag("AudioDontDestroy"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
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
