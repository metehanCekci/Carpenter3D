using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public GameObject[] gameObjectsToFollow; // Eleman sayısı belli olmayan dizi
    public int count = 0;
    public DoorScript[] doorScripts;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            
                foreach (DoorScript obj in doorScripts)
                {
                    obj.doorLocked = true;
                }
            

            

            foreach (GameObject obj in gameObjectsToFollow)
            {
                FollowScript followScript = obj.GetComponent<FollowScript>();
                if (followScript != null)
                {
                    obj.GetComponent<Animator>().SetTrigger("Awake");
                    followScript.isFollowing = true;
                }
            }
        }
    }
    private void Update()
    {

        foreach (GameObject obj in gameObjectsToFollow)
        {
            if (obj.activeInHierarchy)
            {
                break;
            }
            else
            {
                count++;

            }
        }
        if (count >= gameObjectsToFollow.Length)
        {
            this.gameObject.GetComponent<musicSwapper>().phaseToCombat = false;
            this.gameObject.GetComponent<musicSwapper>().phaseToCalm = true;
            
            
                foreach (DoorScript obj in doorScripts)
                {
                    obj.doorLocked = false;
                }
            
            
            

            this.enabled = false;
        }

    }
}

