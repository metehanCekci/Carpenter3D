using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackingJumpPad : MonoBehaviour
{
    public PlayerMovement pm;
    [SerializeField] float baseJumpForce = 25f; // Base force applied for a normal jump
    [SerializeField] float slamMultiplier = 1.5f; // Multiplier for the slam jump
    [SerializeField] float multiplyScale = 2f;
    [SerializeField] float resetTime = 5f; // Time in seconds to reset the slam effect
    bool isSlammed = false;
    float lastSlamTime = 0f;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            if (pm.isSlamming)
            {
                isSlammed = true;
                lastSlamTime = Time.time;
                rb.AddForce(Vector3.up * baseJumpForce * slamMultiplier, ForceMode.Impulse);
                if(slamMultiplier<10)
                slamMultiplier*=multiplyScale;
                pm.SlamImpact();
            }
            

            
        }
    }


    void Update()
    {
        Debug.Log(pm.isGrounded);   
        if (isSlammed && Time.time - lastSlamTime > resetTime)
        {
            isSlammed = false;
        }
    }

    
}
