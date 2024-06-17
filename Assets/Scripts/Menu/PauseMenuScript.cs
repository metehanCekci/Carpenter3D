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
    [SerializeField] AudioSource Combat;
    float combSoundLevel;
    [SerializeField] AudioSource Calm;
    float calmSoundLevel;
    PlayerInputActions inputActions;
    bool isPaused;
    void Awake()
    {
        inputActions = new PlayerInputActions();
        isPaused = false;
    }

    void ResumePauseGame(InputAction.CallbackContext context)
    {

        if (!isPaused)
        {
            //bunu daha iyi bi şekilde yapabilirim ama üşendim
            //bir babayiğit güzelleştirsin bu boktan düzeneği
            combSoundLevel = Combat.volume;
            calmSoundLevel = Calm.volume;
            Combat.volume = 0;
            Calm.volume = 0;
            // Cursor.lockState = CursorLockMode.None;
            pauseMenuUI.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Combat.volume = combSoundLevel;
            Calm.volume = calmSoundLevel;
            pauseMenuUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            isPaused = false;
        }

    }

    //BUTONA EYLEMİ TANITABİLMEK İÇİN GEREKLİ ;(((((((
    public void ResumeButton(){
            pauseMenuUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            isPaused = false;
    }

    public void quitHomeButton(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
