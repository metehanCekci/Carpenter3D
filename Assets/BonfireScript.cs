using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BonfireScript : MonoBehaviour
{
    private PlayerInputActions inputActions;
    [SerializeField] GameObject Player;

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
        ReadJson.Instance.saveFile.LastBonfireID = int.Parse(this.gameObject.name);
        ReadJson.Instance.saveQuick.HP = ReadJson.Instance.saveFile.maxHP;
        ReadJson.Instance.saveQuick.syringeCount = ReadJson.Instance.saveFile.maxSyringe;
        ReadJson.Instance.WriteSaveFile();
    
        }

    }

    public void BonfireManager(GameObject Player)
    {
        ReadJson.Instance.ReadSaveFile();

        if (ReadJson.Instance.saveFile == null)
        {
            return;
        }

        int ID = ReadJson.Instance.saveFile.LastBonfireID;

        if (ReadJson.Instance.saveFile.bonfires == null || ReadJson.Instance.saveFile.bonfires.Count == 0)
        {
            return;
        }

        if (ID < 0 || ID >= ReadJson.Instance.saveFile.bonfires.Count)
        {
            return;
        }

        // Bonfire pozisyonunu loglayalým
        Debug.Log("Bonfire Pos: " + ReadJson.Instance.saveFile.bonfires[ID].BonfirePos);

    }
    public void Update()
    {
        BonfireManager(Player);
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
