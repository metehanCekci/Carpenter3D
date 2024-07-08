using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfarTrigger : MonoBehaviour
{
    public CloseAttackTrigger cat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerStay(Collider other) {

        if(!cat.isClose)
        

        if(other.CompareTag("Player") && !this.transform.parent.GetComponent<KaruiAi>().isBusy)
        
        this.transform.parent.GetComponent<KaruiAi>().longDistance();
    }
}
