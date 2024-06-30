using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            this.transform.parent.GetComponent<EnemyAttack>().startAttack(other.gameObject);
        }
    }
}
