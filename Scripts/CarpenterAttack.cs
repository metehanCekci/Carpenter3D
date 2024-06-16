
using UnityEngine;

public class CarpenterAttack : MonoBehaviour
{
    public Animator animator;
    public string attackTriggerName = "Attack";

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Assuming left mouse button is used for attack
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger(attackTriggerName);
        // Add melee attack logic here
    }
}
