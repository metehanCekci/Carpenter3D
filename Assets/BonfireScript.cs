using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BonfireScript : MonoBehaviour
{

        private PlayerInputActions inputActions;

        void Awake() {
                    inputActions = new PlayerInputActions();
        }

        void OnEnable()
    {
        inputActions.Player.Interact.performed += Interact;

        inputActions.Player.Enable();
    }


    public GameObject text;

    private bool mouseOn = false;

    public void Interact(InputAction.CallbackContext context)
    {
        if(mouseOn)
        {
        Debug.Log("rest");
        ReadJson.Instance.WriteSaveFile();

        SceneReloader.Instance.ResetToCheckpoint();
        }

    }


    public void showText()
    {
        text.SetActive(true);
        mouseOn = true;
    }

    public void leaveText()
    {
        text.SetActive(false); 
        mouseOn = false;       
    }
}
