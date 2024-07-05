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
    private bool closingDistance = false;
    private bool isWaiting = false;

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
    }

    void Update()
    {
        // Add any necessary updates here
    }

    public IEnumerator ForeHeadSlam()
    {
        this.GetComponent<Animator>().SetTrigger("ForeHeadSlam");
        agent.speed = 0;

        yield return new WaitForSeconds(fhsDelay1);
        upSlash1.SetActive(true);
        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash1.SetActive(false);

        yield return new WaitForSeconds(fhsDelay2);
        upSlash2.SetActive(true);
        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash2.SetActive(false);

        yield return new WaitForSeconds(fhsDelay3);
        upSlash1.SetActive(true);
        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash1.SetActive(false);

        yield return new WaitForSeconds(fhsDelay2);
        upSlash2.SetActive(true);
        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash2.SetActive(false);

        yield return new WaitForSeconds(fhsDelay2);
        upSlash1.SetActive(true);
        agent.speed = (baseSpeed * 9);
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash1.SetActive(false);


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
        this.GetComponent<Animator>().SetTrigger("JumpAttack");

        yield return new WaitForSeconds(JumpAttackDelay1);
        jumpAttack.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        jumpAttack.SetActive(false);

        yield return new WaitForSeconds(JumpAttackDelay2);
        jumpAttack.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        jumpAttack.SetActive(false);
        
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
