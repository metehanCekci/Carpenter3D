using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAlert : MonoBehaviour
{
    public GameObject[] gameObjectsToFollow;
    public int count = 0;
    public DoorScript[] doorScripts;
    public musicSwapper ms;
    public bool Encounter = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            
                ms.isCombat = true;

                foreach (DoorScript obj in doorScripts)
                {
                    obj.doorLocked = true;
                }

                foreach (GameObject obj in gameObjectsToFollow)
                {
                    FollowScript followScript = obj.GetComponent<FollowScript>();
                    if (followScript != null)
                    {
                        Animator animator = obj.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.SetTrigger("Awake");
                        }
                        followScript.isFollowing = true;
                    }
                }
            
        }
    }

    public void resetTrigger()
    {
        Encounter = false;

        foreach (GameObject obj in gameObjectsToFollow)
        {
            obj.SetActive(true);

            EnemyHealthScript healthScript = obj.GetComponent<EnemyHealthScript>();
            if (healthScript != null)
            {
                healthScript.hp = healthScript.maxhp;
            }

            NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
            FollowScript followScript = obj.GetComponent<FollowScript>();
            if (agent != null && followScript != null)
            {
                agent.speed = followScript.maxSpeed;
            }

            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Rebind(); // Reset the animator
                animator.Update(0f); // Ensure the animator updates to the default state immediately
            }
        }

        // Debug log to confirm resetTrigger was called
    }

    private void Update()
    {
        if(Encounter)
        return;
            count = 0; // Reset count at the beginning of each frame

            foreach (GameObject obj in gameObjectsToFollow)
            {
                if (obj.activeInHierarchy)
                {
                    // If any object is active, break out of the loop
                    count = 0;
                    break;
                }
                else if(!obj.activeInHierarchy)
                {
                    count++;
                }
            }

            if (count >= gameObjectsToFollow.Length)
            {
                ms.isCombat = false;

                foreach (DoorScript obj in doorScripts)
                {
                    obj.doorLocked = false;
                }

                // Debug log to confirm encounter is finished
                Debug.Log("All enemies defeated. Doors unlocked.");
                count = 0;

                Encounter = true;
            }
        
    }
}
