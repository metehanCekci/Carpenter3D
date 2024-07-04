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

    //values that will be set in the Inspector
	
	public float RotationSpeed = 1;

	//values for internal use
	private Quaternion _lookRotation;
	private Vector3 _direction;
    private Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<NavMeshAgent>().speed!=0)
        {
            //find the vector pointing from our position to the target
		_direction = (Target.position - transform.position).normalized;

		//create the rotation we need to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);

		//rotate us over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }

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
