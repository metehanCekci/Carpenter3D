using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class KaruiAi : MonoBehaviour
{
    public GameObject upSlash1;
    public GameObject upSlash2;
    public GameObject groundCircle;
    public GameObject jetpackSlash;

    public float fhsDelay1;
    public float fhsDelay2;
    public float fhsDelay3;
    
    public float jetpackDelay;
    public float slashComboDelay;
    public float slashComboDelay2;
    public float slashCombo2Delay;
    public float slashCombo2Delay2;
    public float rangedDelay;
    public float rangedDelay2;
    public float JumpAttackDelay1;
    public float JumpAttackDelay2;

    public GameObject comboSlash1;
    public GameObject comboSlash2;
    public GameObject combo2Slash1;
    public GameObject combo2Slash2;
    public GameObject bazooka1;
    public GameObject bazooka2;
    public GameObject jumpAttack;

    public GameObject Player;
    private NavMeshAgent agent;
    private float baseSpeed;

    public bool isBusy = false;

    private Vector3 jumpAttackTrans;
    private Vector3 jumpAttackTrans2;
    private bool closingDistance = false;
    public bool jumpAttackUp = false;
    private bool jumpAttackDown = false;
    private bool isWaiting = false;

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
    }

    void FixedUpdate()
    {
        if(jumpAttackUp)
        {
            Debug.Log("Jump Attack Up is true. Current position: " + transform.position + " Target position: " + jumpAttackTrans);

            float step = baseSpeed * Time.deltaTime; // Calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, jumpAttackTrans, step);

            if(Vector3.Distance(transform.position, jumpAttackTrans) < 0.1f)
            {
                Debug.Log("Reached target position.");
                jumpAttackUp = false;
            }
        }
        else if(jumpAttackDown)
        {
            float step = baseSpeed * Time.deltaTime * 15; // Calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, jumpAttackTrans2, step);

            if(Vector3.Distance(transform.position, jumpAttackTrans2) < 0.1f)
            {
                Debug.Log("Reached target position.");
                jumpAttackDown = false;
                agent.enabled = true;
            }  
        }
    }

    public IEnumerator ForeHeadSlam()
    {
        this.GetComponent<Animator>().SetTrigger("ForeHeadSlam");
        agent.speed = 0;

        yield return new WaitForSeconds(fhsDelay1);
        GameObject clone = Instantiate(upSlash1);
        clone.SetActive(true);

        clone.transform.SetParent(upSlash1.transform.parent);
        clone.transform.position = upSlash1.transform.position;
        clone.transform.rotation = upSlash1.transform.rotation;

        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        clone.GetComponent<BoxCollider>().enabled = false;
        Destroy(clone, 1);

        yield return new WaitForSeconds(fhsDelay2);
        GameObject clone1 = Instantiate(upSlash2);
        clone1.SetActive(true);

        clone1.transform.SetParent(upSlash2.transform.parent);
        clone1.transform.position = upSlash2.transform.position;
        clone1.transform.rotation = upSlash2.transform.rotation;

        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        clone1.GetComponent<BoxCollider>().enabled = false;
        Destroy(clone1, 1);

        yield return new WaitForSeconds(fhsDelay3);
        GameObject clone2 = Instantiate(upSlash1);
        clone2.SetActive(true);

        clone2.transform.SetParent(upSlash1.transform.parent);
        clone2.transform.position = upSlash1.transform.position;
        clone2.transform.rotation = upSlash1.transform.rotation;

        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        clone2.GetComponent<BoxCollider>().enabled = false;
        Destroy(clone2, 1);

        yield return new WaitForSeconds(fhsDelay2);
        GameObject clone3 = Instantiate(upSlash2);
        clone3.SetActive(true);

        clone3.transform.SetParent(upSlash2.transform.parent);
        clone3.transform.position = upSlash2.transform.position;
        clone3.transform.rotation = upSlash2.transform.rotation;

        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        clone3.GetComponent<BoxCollider>().enabled = false;
        Destroy(clone3, 1);

        yield return new WaitForSeconds(fhsDelay2);
        GameObject clone4 = Instantiate(upSlash1);
        clone4.SetActive(true);

        clone4.transform.SetParent(upSlash1.transform.parent);
        clone4.transform.position = upSlash1.transform.position;
        clone4.transform.rotation = upSlash1.transform.rotation;

        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        clone4.GetComponent<BoxCollider>().enabled = false;
        Destroy(clone4, 1);

        Invoke("attackEnder", 2);
    }

    public IEnumerator JetPackAttack()
    {
        this.GetComponent<Animator>().SetTrigger("JetPackAttack");

        yield return new WaitForSeconds(jetpackDelay);
        agent.speed *= 10;
        yield return new WaitForSeconds(0.5f);
        agent.speed = baseSpeed;
        jetpackSlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        jetpackSlash.SetActive(false);

        Invoke("attackEnder", 1);
    }

    public IEnumerator SlashCombo()
    {
        this.GetComponent<Animator>().SetTrigger("SlashCombo");

        yield return new WaitForSeconds(slashComboDelay);
        comboSlash1.SetActive(true);
        agent.speed *= 3;
        yield return new WaitForSeconds(0.1f);
        comboSlash1.SetActive(false);
        agent.speed = 0;

        yield return new WaitForSeconds(slashComboDelay2);
        comboSlash2.SetActive(true);
        agent.speed *= 3;
        yield return new WaitForSeconds(0.1f);
        comboSlash2.SetActive(false);

        Invoke("attackEnder", 1);
    }

    public IEnumerator SlashCombo2()
    {
        this.GetComponent<Animator>().SetTrigger("SlashCombo2");

        yield return new WaitForSeconds(slashCombo2Delay);
        combo2Slash1.SetActive(true);
        agent.speed *= 3;
        yield return new WaitForSeconds(0.1f);
        combo2Slash1.SetActive(false);
        agent.speed = 0;

        yield return new WaitForSeconds(slashCombo2Delay2);
        combo2Slash2.SetActive(true);
        agent.speed *= 3;
        yield return new WaitForSeconds(0.1f);
        combo2Slash2.SetActive(false);

        Invoke("attackEnder", 1);
    }

    public IEnumerator RangedAttack()
    {
        this.GetComponent<Animator>().SetTrigger("RangedAttack");

        yield return new WaitForSeconds(rangedDelay);
        GameObject clone = Instantiate(bazooka1);
        clone.SetActive(true);
        agent.speed *= -10;
        yield return new WaitForSeconds(0.05f);
        agent.speed = baseSpeed;
        yield return new WaitForSeconds(rangedDelay2);
        GameObject clone1 = Instantiate(bazooka2);
        clone1.SetActive(true);

        Invoke("attackEnder", 1);
    }

    public IEnumerator JumpAttack()
    {
        jumpAttackTrans = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z); // For testing
        jumpAttackUp = true;

        this.GetComponent<Animator>().SetTrigger("JumpAttack");
        agent.enabled = false; // Disable NavMeshAgent

        yield return new WaitForSeconds(JumpAttackDelay1 - 0.5f);



        jumpAttackTrans2 = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z); // For testing
        jumpAttackDown = true;

        yield return new WaitForSeconds(0.5f);

        GameObject clone = Instantiate(jumpAttack);
        clone.SetActive(true);
        clone.transform.position = jumpAttack.transform.position;
        clone.transform.rotation = jumpAttack.transform.rotation;
        clone.transform.SetParent(jumpAttack.transform.parent);
        yield return new WaitForSeconds(0.1f);
        clone.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(JumpAttackDelay2);
        GameObject clone1 = Instantiate(jumpAttack);
        clone1.SetActive(true);
        clone1.transform.position = jumpAttack.transform.position;
        clone1.transform.rotation = jumpAttack.transform.rotation;
        clone1.transform.SetParent(jumpAttack.transform.parent);
        yield return new WaitForSeconds(0.1f);
        clone1.GetComponent<BoxCollider>().enabled = false;

        Destroy(clone, 1);
        Destroy(clone1, 1);


        Invoke("attackEnder", 2);
    }

    public void randomAttack()
    {
        if (!isBusy)
        {
            agent.speed = baseSpeed;
            this.GetComponent<Animator>().speed = 1;
            closingDistance = false;
            agent.speed = 0;

            int attackNumber = Random.Range(1, 40);

            if (attackNumber < 30)
                StartCoroutine(ForeHeadSlam());
            else if (attackNumber < 40)
                StartCoroutine(JumpAttack());
            else if (attackNumber < 65)
                StartCoroutine(SlashCombo());
            else if (attackNumber < 90)
                StartCoroutine(SlashCombo2());
            else if (attackNumber < 100)
                StartCoroutine(JetPackAttack());

            isBusy = true;
        }
    }

    public void randomRanged()
    {
        int randomNumber = Random.Range(1, 3);

        if (randomNumber == 1)
            StartCoroutine(RangedAttack());
        else if (randomNumber == 2)
            StartCoroutine(JumpAttack());
    }

    public void longDistance()
    {
        if (!isBusy && !closingDistance && !isWaiting)
        {
            int longDis = Random.Range(1, 2);

            if (longDis == 1 || longDis == 2)
                closeDistance();
            else if (longDis == 3)
                randomRanged();
            else
                isWaiting = true;

            Invoke("notWaiting", 2);
        }
    }

    public void closeDistance()
    {
        closingDistance = true;
        agent.speed *= 2;
        this.GetComponent<Animator>().speed = 2;
    }

    public void attackEnder()
    {
        isBusy = false;
        agent.speed = baseSpeed;
    }

    public void notWaiting()
    {
        isWaiting = false;
    }
}
