using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowScript : MonoBehaviour
{
    public bool isFollowing = false;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && isFollowing)
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(target.position);
    }


}
