using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class damagePlayerScript : MonoBehaviour
{
    public PlayerMovement PM;
    public PlayerHpBar PHB;
    public float damageAmt;
    private float timer;
    [HideInInspector] public bool waitForParry = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitForParry)
        {
            timer += Time.deltaTime;

            if(PM.parrySuccessful)
            {
                YesParry();
                timer = 0;
            }

            else if(timer > 0.1)
            {
                NoParry();
                timer = 0;
            }

        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!PM.parrySuccessful)
            {
                waitForParry = true;
            }
            else
            {
                 YesParry();             
            }
        }
    }

    public void YesParry()
    {
        Debug.Log("succesfull parry");
        SfxScript.Instance.playParry();
        PM.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
        Invoke("setFalse",0.2f);
        PM.parrySuccessful = false;
        PM.startIFrames();
        waitForParry = false;
    }

    public void NoParry()
    {
            SfxScript.Instance.playHurt();
            PHB.playerHp -= damageAmt;
            PM.startIFrames();
            waitForParry = false;
    }

    public void turnOfSlowMotion()
    {

    }

    public void setFalse()
    {
                PM.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
    }
}
