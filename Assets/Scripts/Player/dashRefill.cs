using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashRefill : MonoBehaviour
{
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
            other.gameObject.GetComponent<PlayerMovement>().canDash = true;
            other.gameObject.GetComponent<PlayerMovement>().hasAirDashed = false;
            Destroy(this.gameObject);
        }
    }
}
