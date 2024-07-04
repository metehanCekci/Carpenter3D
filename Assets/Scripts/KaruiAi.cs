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

    public GameObject comboSlash1;
    public GameObject comboSlash2;
    public GameObject combo2Slash1;
    public GameObject combo2Slash2;
    public GameObject bazooka1;
    public GameObject bazooka2;

    public GameObject Player;
    private NavMeshAgent agent;
    private float baseSpeed;

    private bool isBusy = false;
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

        yield return new WaitForSeconds(fhsDelay1);
        upSlash1.SetActive(true);
        agent.speed = baseSpeed * 3;
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash1.SetActive(false);

        yield return new WaitForSeconds(fhsDelay2);
        upSlash2.SetActive(true);
        agent.speed = baseSpeed * 3;
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash2.SetActive(false);

        yield return new WaitForSeconds(fhsDelay3);
        upSlash1.SetActive(true);
        agent.speed = baseSpeed * 3;
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash1.SetActive(false);

        yield return new WaitForSeconds(fhsDelay2);
        upSlash2.SetActive(true);
        agent.speed = baseSpeed * 3;
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash2.SetActive(false);

        yield return new WaitForSeconds(fhsDelay1);
        upSlash1.SetActive(true);
        agent.speed = baseSpeed * 3;
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;
        upSlash1.SetActive(false);

        yield return new WaitForSeconds(fhsDelay3);
        GameObject clone = Instantiate(groundCircle);
        clone.SetActive(true);

        attackEnder();
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

        attackEnder();
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

        attackEnder();
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

        attackEnder();
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

        attackEnder();
    }

    public IEnumerator JumpAttack()
    {
        this.GetComponent<Animator>().SetTrigger("JumpAttack");

        attackEnder();

        yield return new WaitForSeconds(0);
    }

    public void randomAttack()
    {
        if (!isBusy)
        {
            agent.speed = baseSpeed;
            closingDistance = false;
            agent.speed = 0;

            int attackNumber = Random.Range(1, 100);

            if (attackNumber < 30)
                StartCoroutine(ForeHeadSlam());
            else if (attackNumber < 40)
                StartCoroutine(JetPackAttack());
            else if (attackNumber < 65)
                StartCoroutine(SlashCombo());
            else if (attackNumber < 90)
                StartCoroutine(SlashCombo2());
            else if (attackNumber < 100)
                StartCoroutine(JumpAttack());

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
            int longDis = Random.Range(1, 5);

            if (longDis == 1)
                closeDistance();
            else if (longDis == 2)
                randomRanged();
            else
                isWaiting = true;

            Invoke("notWaiting", 2);
        }
    }

    public void closeDistance()
    {
        closingDistance = true;
        agent.speed *= 3;
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
