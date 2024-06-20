using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float speed;
    public float xAmt;
    public float yAmt;
    public float zAmt;

    private bool doorOpening = false;
    private bool doorClosing = false;
    private bool doorIsClosed = true;
    private bool closeInQueue = false;

    private Vector3 initialPositionLeft;
    private Vector3 initialPositionRight;

    // Start is called before the first frame update
    void Start()
    {
        initialPositionLeft = transform.GetChild(0).gameObject.transform.position;
        initialPositionRight = transform.GetChild(1).gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpening)
        {
            transform.GetChild(0).gameObject.transform.position = Vector3.MoveTowards(transform.GetChild(0).gameObject.transform.position, initialPositionLeft + (new Vector3(xAmt, yAmt, zAmt)), speed * Time.deltaTime);
            transform.GetChild(1).gameObject.transform.position = Vector3.MoveTowards(transform.GetChild(1).gameObject.transform.position, initialPositionRight - (new Vector3(xAmt, yAmt, zAmt)), speed * Time.deltaTime);
        }
        else
        {
            transform.GetChild(0).gameObject.transform.position = Vector3.MoveTowards(transform.GetChild(0).gameObject.transform.position, initialPositionLeft, speed * Time.deltaTime);
            transform.GetChild(1).gameObject.transform.position = Vector3.MoveTowards(transform.GetChild(1).gameObject.transform.position, initialPositionRight, speed * Time.deltaTime);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

                doorOpening = true;
            
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            doorOpening = false;
        }
            
        
    }





}
