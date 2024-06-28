using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class damagePlayerScript : MonoBehaviour
{
    public float damageAmt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            SfxScript.Instance.playHurt();
            PlayerHpBar.Instance.playerHp -= damageAmt;
            
        }
    }
}
