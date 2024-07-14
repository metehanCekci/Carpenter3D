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
  

    public bool isAttacking = false;
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
        GameObject clone = Instantiate(slashEffect);
        Destroy(clone , 1);
        clone.transform.position = slashEffect.transform.position;
        clone.transform.rotation = slashEffect.transform.rotation;
        clone.transform.SetParent(slashEffect.transform.parent);
        clone.SetActive(true);
        this.gameObject.GetComponent<NavMeshAgent>().speed = 30;
        yield return new WaitForSeconds(0.1f);
        clone.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(secondAttackTiming);
        GameObject clone2 = Instantiate(slashEffect2);
        Destroy(clone2 , 1);
        clone2.transform.position = slashEffect2.transform.position;
        clone2.transform.rotation = slashEffect2.transform.rotation;
        clone2.transform.SetParent(slashEffect2.transform.parent);
        clone2.SetActive(true);
       this.gameObject.GetComponent<NavMeshAgent>().speed = 30;
        yield return new WaitForSeconds(0.1f);
        clone2.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitForSeconds(0.2f);

        
        


        isAttacking = false;
        this.gameObject.GetComponent<NavMeshAgent>().speed = 3;



    }

}
