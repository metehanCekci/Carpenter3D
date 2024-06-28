using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public GameObject slashEffect;
    public GameObject slashEffect2;

    public float firstAttackTiming;
    public float secondAttackTiming;
  

    private bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAttack(GameObject obj)
    {
        if(!isAttacking)
        {
        this.gameObject.GetComponent<Animator>().SetTrigger("isAttacking");
        StartCoroutine(swing());
        }

    }

    IEnumerator swing()
    {
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;

        isAttacking = true;
        yield return new WaitForSeconds(firstAttackTiming);
        slashEffect.SetActive(true);
        this.gameObject.GetComponent<NavMeshAgent>().speed = 20;
        yield return new WaitForSeconds(0.1f);
        slashEffect.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitForSeconds(0.2f);
        slashEffect.SetActive(false);
        yield return new WaitForSeconds(secondAttackTiming);
        slashEffect2.SetActive(true);
       this.gameObject.GetComponent<NavMeshAgent>().speed = 20;
        yield return new WaitForSeconds(0.1f);
        slashEffect2.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitForSeconds(0.2f);
        slashEffect2.SetActive(false);

        //Collider[] hitEnemies = Physics.OverlapSphere(attackPos.transform.position, radius);
        slashEffect.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        slashEffect2.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        isAttacking = false;
        this.gameObject.GetComponent<NavMeshAgent>().speed = 3;

    }

}
