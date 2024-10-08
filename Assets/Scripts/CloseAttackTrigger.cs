using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttackTrigger : MonoBehaviour
{
    public bool isClose = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player") && !this.transform.parent.GetComponent<KaruiAi>().isBusy)
        
        this.transform.parent.GetComponent<KaruiAi>().randomAttack();
    }

    private void OnTriggerEnter(Collider other) {
        isClose = true;
    }

    private void OnTriggerExit(Collider other) {
        isClose = false;
    }
}