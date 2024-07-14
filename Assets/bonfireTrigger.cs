using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonfireTrigger : MonoBehaviour
{
    // Start is called before the first frame update
private void OnTriggerEnter(Collider other) {
    if(other.CompareTag("Bonfire"))
    {
        other.gameObject.GetComponent<BonfireScript>().showText();
    }
}

private void OnTriggerExit(Collider other) {
    if(other.CompareTag("Bonfire"))
    {
        other.gameObject.GetComponent<BonfireScript>().leaveText();
    }
}
}
