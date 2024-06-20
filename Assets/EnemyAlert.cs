using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public GameObject[] gameObjectsToFollow; // Eleman sayısı belli olmayan dizi
    public int count = 0;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in gameObjectsToFollow)
            {
                FollowScript followScript = obj.GetComponent<FollowScript>();
                if (followScript != null)
                {
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
            if (this.gameObject.GetComponent<musicSwapper>().phaseToCalm)
            {
                this.gameObject.GetComponent<musicSwapper>().phaseToCombat = true;
                this.gameObject.GetComponent<musicSwapper>().phaseToCalm = false;
            }
            else
            {
                this.gameObject.GetComponent<musicSwapper>().phaseToCalm = true;
                this.gameObject.GetComponent<musicSwapper>().phaseToCombat = false;
            }
            this.enabled = false;
        }

    }
}

