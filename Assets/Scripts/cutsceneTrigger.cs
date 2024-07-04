using UnityEngine;
using System.Collections;

public class cutsceneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(Collider.gameObject.CompareTag("Player"))
        {
            other.GetComponent<animator>().setTrigger("Intro");
        }
    }

}