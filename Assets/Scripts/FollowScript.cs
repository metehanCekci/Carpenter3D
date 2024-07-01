using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FollowScript : MonoBehaviour
{
    public bool isFollowing = false;
    public Transform target;
    bool isTouching = false;
    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && isFollowing && !isTouching)
        {
            navMeshAgent.SetDestination(target.position);
            this.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            }
        else if(isTouching){
            navMeshAgent.SetDestination(transform.position);
            this.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }

        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) 
        {
            navMeshAgent.updateRotation = false;
            //insert your rotation code herec
        }
        else {
            navMeshAgent.updateRotation = true;
        }
            }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isTouching = true;

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isTouching = false;
    }


}
