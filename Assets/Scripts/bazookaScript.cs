using UnityEngine;
using System.Collections;


public class bazookaScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPos;
    public float speed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
    }


}