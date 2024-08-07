using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public float slashComboDelay3;
    public float slashCombo2Delay;
    public float slashCombo2Delay2;
    public float rangedDelay;
    public float JumpAttackDelay1;
    public float JumpAttackDelay2;

    public GameObject comboSlash1;
    public GameObject comboSlash2;
    public GameObject comboSlash3;
    public GameObject combo2Slash1;
    public GameObject combo2Slash2;

    public GameObject RangedSlash;
    public GameObject jumpAttack;

    public GameObject Player;
    private NavMeshAgent agent;
    public float baseSpeed;

    public bool isBusy = false;

    private Vector3 jumpAttackTrans;
    private Vector3 jumpAttackTrans2;
    private bool closingDistance = false;
    public bool jumpAttackUp = false;
    private bool jumpAttackDown = false;
    private bool isWaiting = false;

    private int lastAttackIndex = -1; // Son seçilen saldırının indeksini saklar

    private List<Coroutine> runningCoroutines = new List<Coroutine>();

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

    }

    void FixedUpdate()
    {
        if (jumpAttackUp)
        {
            Debug.Log("Jump Attack Up is true. Current position: " + transform.position + " Target position: " + jumpAttackTrans);

            float step = baseSpeed * Time.deltaTime; // Calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, jumpAttackTrans, step * 2);

            if (Vector3.Distance(transform.position, jumpAttackTrans) < 0.1f)
            {
                jumpAttackUp = false;
            }
        }
        else if (jumpAttackDown)
        {
            float step = baseSpeed * Time.deltaTime; // Calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, jumpAttackTrans2, step * 4);

            if (Vector3.Distance(transform.position, jumpAttackTrans2) < 0.1f)
            {
                jumpAttackDown = false;
                agent.enabled = true;
            }
        }
    }

    void OnDisable()
    {
        // Stop all running coroutines
        foreach (var coroutine in runningCoroutines)
        {
            StopCoroutine(coroutine);
        }
        runningCoroutines.Clear();

        // Reset any ongoing actions
        isBusy = false;
        closingDistance = false;
        jumpAttackUp = false;
        jumpAttackDown = false;
        isWaiting = false;

        // Reset agent speed and re-enable it if needed


        // Reset animator
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.ResetTrigger("ForeHeadSlam");
            animator.ResetTrigger("JetPackAttack");
            animator.ResetTrigger("SpinSlashAttack");
            animator.ResetTrigger("TwoSlash");
            animator.ResetTrigger("RangedSlash");
            animator.ResetTrigger("JumpAttack");
            animator.SetBool("isWalking", false);
        }
    }

    void OnEnable() {
             
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

        Invoke("attackEnder", 2 - 0.1f);
    }

    public IEnumerator JetPackAttack()
    {
        agent.speed = 0;
        this.GetComponent<Animator>().SetTrigger("JetPackAttack");

        yield return new WaitForSeconds(1.3f);
        SfxScript.Instance.playWoosh();
        agent.speed = baseSpeed * 12;
        yield return new WaitForSeconds(jetpackDelay);
        agent.speed = 0;
        GameObject clone = Instantiate(jetpackSlash);
        clone.SetActive(true);

        clone.transform.SetParent(jetpackSlash.transform.parent);
        clone.transform.position = jetpackSlash.transform.position;
        clone.transform.rotation = jetpackSlash.transform.rotation;

        yield return new WaitForSeconds(0.1f);
        clone.GetComponent<BoxCollider>().enabled = false;

        Invoke("attackEnder", 2f);
    }

    public IEnumerator SlashCombo()
    {
        this.GetComponent<Animator>().SetTrigger("SpinSlashAttack");

        yield return new WaitForSeconds(slashComboDelay);
        GameObject clone = Instantiate(comboSlash1);
        clone.SetActive(true);

        clone.transform.SetParent(comboSlash1.transform.parent);
        clone.transform.position = comboSlash1.transform.position;
        clone.transform.rotation = comboSlash1.transform.rotation;

        agent.speed = baseSpeed * 10;
        yield return new WaitForSeconds(0.1f);
        clone.GetComponent<BoxCollider>().enabled = false;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);

        GameObject clone00 = Instantiate(comboSlash3);
        clone00.SetActive(true);

        clone00.transform.SetParent(comboSlash3.transform.parent);
        clone00.transform.position = comboSlash3.transform.position;
        clone00.transform.rotation = comboSlash3.transform.rotation;

        yield return new WaitForSeconds(0.1f);
        clone00.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(slashComboDelay2 - 0.2f);

        GameObject clone2 = Instantiate(comboSlash2);
        clone2.SetActive(true);

        clone2.transform.SetParent(comboSlash2.transform.parent);
        clone2.transform.position = comboSlash2.transform.position;
        clone2.transform.rotation = comboSlash2.transform.rotation;

        agent.speed = baseSpeed * 10;
        yield return new WaitForSeconds(0.1f);
        clone2.GetComponent<BoxCollider>().enabled = false;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);

        GameObject clone01 = Instantiate(comboSlash1);
        clone01.SetActive(true);

        clone01.transform.SetParent(comboSlash1.transform.parent);
        clone01.transform.position = comboSlash1.transform.position;
        clone01.transform.rotation = comboSlash1.transform.rotation;

        yield return new WaitForSeconds(0.1f);
        clone01.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(slashComboDelay3 - 0.2f);

        GameObject clone3 = Instantiate(comboSlash3);
        clone3.SetActive(true);

        clone3.transform.SetParent(comboSlash3.transform.parent);
        clone3.transform.position = comboSlash3.transform.position;
        clone3.transform.rotation = comboSlash3.transform.rotation;

        agent.speed = baseSpeed * 10;
        yield return new WaitForSeconds(0.1f);
        clone3.GetComponent<BoxCollider>().enabled = false;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);

        GameObject clone02 = Instantiate(comboSlash1);
        clone02.SetActive(true);

        clone02.transform.SetParent(comboSlash1.transform.parent);
        clone02.transform.position = comboSlash1.transform.position;
        clone02.transform.rotation = comboSlash1.transform.rotation;

        yield return new WaitForSeconds(0.1f);
        clone02.GetComponent<BoxCollider>().enabled = false;

        Invoke("attackEnder", 2);
    }

    public IEnumerator SlashCombo2()
    {
        this.GetComponent<Animator>().SetTrigger("TwoSlash");

        yield return new WaitForSeconds(slashCombo2Delay);
        GameObject clone = Instantiate(combo2Slash1);
        clone.SetActive(true);

        clone.transform.SetParent(combo2Slash1.transform.parent);
        clone.transform.position = combo2Slash1.transform.position;
        clone.transform.rotation = combo2Slash1.transform.rotation;
        agent.speed = baseSpeed * 20;
        yield return new WaitForSeconds(0.1f);
        clone.GetComponent<BoxCollider>().enabled = false;
        agent.speed = 0;

        yield return new WaitForSeconds(slashCombo2Delay2);
        GameObject clone1 = Instantiate(combo2Slash2);
        clone1.SetActive(true);

        clone1.transform.SetParent(combo2Slash2.transform.parent);
        clone1.transform.position = combo2Slash2.transform.position;
        clone1.transform.rotation = combo2Slash2.transform.rotation;
        agent.speed = baseSpeed * 20;
        yield return new WaitForSeconds(0.1f);
        clone1.GetComponent<BoxCollider>().enabled = false;
        agent.speed = 0;

        Destroy(clone, 1);
        Destroy(clone1, 1);

        Invoke("attackEnder", 2 - 0.1f);
    }

    public IEnumerator RangedAttack()
    {
        this.GetComponent<Animator>().SetTrigger("RangedSlash");

        agent.speed = 0;
        yield return new WaitForSeconds(rangedDelay);
        GameObject clone = Instantiate(RangedSlash);

        clone.transform.position = RangedSlash.transform.position;
        clone.transform.rotation = RangedSlash.transform.rotation;
        clone.SetActive(true);

        agent.speed = baseSpeed * -10;
        yield return new WaitForSeconds(0.1f);
        agent.speed = 0;

        Invoke("attackEnder", 2);
    }

    public IEnumerator JumpAttack()
    {
        this.GetComponent<FollowScript>().isRotating = false;
        this.GetComponent<FollowScript>().isFollowing = false;
        jumpAttackTrans = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z); // For testing
        jumpAttackUp = true;

        this.GetComponent<Animator>().SetTrigger("JumpAttack");
        agent.enabled = false; // Disable NavMeshAgent

        yield return new WaitForSeconds(JumpAttackDelay1 - 0.5f);

        jumpAttackTrans2 = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z); // For testing


        yield return new WaitForSeconds(0.3f);
        jumpAttackDown = true;
        yield return new WaitForSeconds(0.2f);

        GameObject clone = Instantiate(jumpAttack);
        SfxScript.Instance.playCrush();
        clone.SetActive(true);
        clone.transform.position = jumpAttack.transform.position;
        clone.transform.rotation = jumpAttack.transform.rotation;
        clone.transform.SetParent(jumpAttack.transform.parent);
        yield return new WaitForSeconds(0.1f);
        clone.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(JumpAttackDelay2);
        GameObject clone1 = Instantiate(jumpAttack);
        SfxScript.Instance.playCrush();
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
        if (this.enabled)
            if (!isBusy)
            {
                this.GetComponent<Animator>().SetBool("isWalking", false);
                agent.speed = baseSpeed;
                this.GetComponent<Animator>().speed = 1;
                closingDistance = false;
                agent.speed = 0;



                // Yeni bir saldırı seçmek için döngü
                int newAttackIndex = -1;
                while (newAttackIndex == -1 || newAttackIndex == lastAttackIndex)
                {
                    newAttackIndex = Random.Range(1, 6);
                }
                lastAttackIndex = newAttackIndex;

                switch (newAttackIndex)
                {
                    case 1:
                        runningCoroutines.Add(StartCoroutine(ForeHeadSlam()));
                        break;
                    case 2:
                        runningCoroutines.Add(StartCoroutine(JumpAttack()));
                        break;
                    case 3:
                        runningCoroutines.Add(StartCoroutine(SlashCombo()));
                        break;
                    case 4:
                        runningCoroutines.Add(StartCoroutine(JetPackAttack()));
                        break;
                    case 5:
                        runningCoroutines.Add(StartCoroutine(SlashCombo2()));
                        break;
                    default:
                        break;
                }

                isBusy = true;
            }
    }

    public void randomRanged()
    {

        this.GetComponent<Animator>().SetBool("isWalking", false);
        this.GetComponent<Animator>().speed = 1;
        int randomNumber = Random.Range(1, 4);

        if (randomNumber == 1 || randomNumber == 2)
            runningCoroutines.Add(StartCoroutine(RangedAttack()));
        else if (randomNumber == 3)
            runningCoroutines.Add(StartCoroutine(JetPackAttack()));

        isBusy = true;
    }

    public void longDistance()
    {
        if (this.enabled)
        {
            if (!isBusy && !closingDistance && !isWaiting)
            {
                int longDis = Random.Range(1, 5);

                if (longDis == 1 || longDis == 2)
                    closeDistance();
                else if (longDis == 3 || longDis == 4)
                    randomRanged();
                else
                    isWaiting = true;

                Invoke("notWaiting", 2);
            }
        }
    }

    public void closeDistance()
    {
        closingDistance = true;
        agent.speed = baseSpeed * 3;
        this.GetComponent<Animator>().speed = 2;
    }

    public void attackEnder()
    {
        isBusy = false;
        if (this.enabled)
        {
            agent.speed = baseSpeed;
            this.GetComponent<FollowScript>().isFollowing = true;
            this.GetComponent<FollowScript>().isRotating = true;
            this.GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            agent.speed = 0;
        }
    }

    public void notWaiting()
    {
        isWaiting = false;
    }
}
