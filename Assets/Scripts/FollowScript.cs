using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowScript : MonoBehaviour
{
    public bool isFollowing = false;
    public Transform target;
    private bool isTouching = false;
    public bool isRotating = true;
    private NavMeshAgent navMeshAgent;

    // Values that will be set in the Inspector
    public float RotationSpeed = 1;

    // Values for internal use
    private Quaternion lookRotation;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (target == null)
        {
            Debug.LogError("Target not set and no GameObject with tag 'Player' found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent == null || target == null)
        {
            return;
        }

        if (this.enabled)
        {

            if (isRotating)
            {
                // Find the vector pointing from our position to the target
                direction = (target.position - transform.position).normalized;

                // Create the rotation we need to be in to look at the target
                lookRotation = Quaternion.LookRotation(direction);

                // Rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
            }


            if (isFollowing && !isTouching)
            {
                navMeshAgent.SetDestination(target.position);
            }
            else if (isTouching)
            {
                navMeshAgent.SetDestination(transform.position);
            }

            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                navMeshAgent.updateRotation = false;
                // Insert your rotation code here
            }
            else
            {
                navMeshAgent.updateRotation = true;
            }
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
