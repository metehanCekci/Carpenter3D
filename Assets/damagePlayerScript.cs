using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class damagePlayerScript : MonoBehaviour
{
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

            if(PlayerMovement.Instance.parrySuccessful)
            {
                YesParry();

            }

            else if(timer > 0.3)
            {
                NoParry();
                timer = 0;
            }

        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!PlayerMovement.Instance.parrySuccessful)
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
        SfxScript.Instance.playParry();
        PlayerMovement.Instance.parrySuccessful = false;
        PlayerMovement.Instance.startIFrames();
        waitForParry = false;
    }

    public void NoParry()
    {
            SfxScript.Instance.playHurt();
            PlayerHpBar.Instance.playerHp -= damageAmt;
            PlayerMovement.Instance.startIFrames();
            waitForParry = false;
    }

    public void turnOfSlowMotion()
    {

    }
}
