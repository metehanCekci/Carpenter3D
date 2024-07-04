using UnityEngine;
using System.Collections;

public GameObject upSlash1;
public GameObject upSlash2;
public GameObject groundCircle;
public GameObject jetpackSlash;

public float fhsDelay1;
public float fhsDelay2;
public float fhsDelay3;

public GameObject Player;
private NavMeshAgent agent;
private float baseSpeed;

public class KaruiAi : MonoBehaviour
{
    void Start
    {
        agent = this.gameobject.GetComponent<NavMeshAgent>();
        
        baseSpeed = agent.speed;
    }

    void update()
    {

    }

    public IEnumerator ForeHeadSlam()
    {

        yield return new waitforseconds(fhsDelay1);
        upSlash1.setActive(true);
        agent.speed = baseSpeed * 3;
        yield return new waitforseconds(0.1f);
        agent.speed = 0;
        upSlash1.setActive(false);

        yield return new waitforseconds(fhsDelay2);
        upSlash2.setActive(true);
        agent.speed = baseSpeed * 3;
        yield return new waitforseconds(0.1f);
        agent.speed = 0;
        upSlash2.setActive(false);


        yield return new waitforseconds(fhsDelay3);


        upSlash1.setActive(true);
        agent.speed = baseSpeed * 3;
        yield return new waitforseconds(0.1f);
        agent.speed = 0;
        upSlash1.setActive(false);

        yield return new waitforseconds(fhsDelay2);
        upSlash2.setActive(true);
        agent.speed = baseSpeed * 3;
        yield return new waitforseconds(0.1f);
        agent.speed = 0;
        upSlash2.setActive(false);

        yield return new waitforseconds(fhsDelay1);
        upSlash1.setActive(true);
        agent.speed = baseSpeed * 3;
        yield return new waitforseconds(0.1f);
        agent.speed = 0;
        upSlash1.setActive(false);


        yield return new waitforseconds(fhsDelay3);


        GameObject clone = Instantiate(groundCircle)
        clone.setActive(true);

        attackEnder();

    }

    public IEnumerator JetPackAttack()
    {
        yield return new WaitForSeconds(jetpackDelay);
        agent.speed *= 10;
        yield return new waitforseconds(0.5f);
        agent.speed = baseSpeed;
        jetpackSlash.setActive(true);
        yield return new waitforseconds(0.1f);
        jetpackSlash.setActive(false);

        attackEnder();
    }

    public IEnumerator SlashCombo();
    {
        yield return new WaitForSeconds(slashComboDelay);
        comboSlash1.setActive(true);
        agent.speed *= 3;
        yield return new waitforseconds(0.1f);
        comboSlash1.setActive(false);
        agent.speed = 0;

        yield return new waitforseconds(slashComboDelay2);
        comboSlash2.setActive(true);
        agent.speed *= 3;
        yield return new waitforseconds(0.1f);
        comboSlash2.setActive(false);

        attackEnder();
    }

    public IEnumerator SlashCombo2()
    {
        attackEnder();
    }

    public IEnumerator RangedAttack()
    {

        yield return new waitforseconds(rangedDelay);
        GameObject clone = Instantiate(bazooka1)
        clone.setActive(true);
        agent.speed *= -10;
        yield return new waitforseconds(0.05f);
        agent.speed = baseSpeed;
        yield return new waitforseconds(rangedDelay2);
        GameObject clone1 = Instantiate(bazooka2)
        clone1.setActive(true);

        attackEnder();

    }

    public IEnumerator JumpAttack()
    {
        attackEnder();
    }


    public void randomAttack()
    {
        agent.speed = baseSpeed;
        if(!isBusy)
        {
        closingDistance = false;
        agent.speed = 0;

        int attackNumber = Random.Range(1,100);
        
        if(attackNumber<30)
        startCoroutine(ForeHeadSlam());

        else if(attackNumber<40)
        startCoroutine(JetPackAttack());

        else if(attackNumber<65)
        startCoroutine(SlashCombo());

        else if(attackNumber<90)
        startCoroutine(SlashCombo2());

        else if(attackNumber<100)
        startCoroutine(JumpAttack());


        isBusy = true;
        }

    }

    public void randomRanged()
    {

        int randomNumber = Random.Range(1,3);

        if(randomNumber == 1)
        startCoroutine(RangedAttack());

        else if(randomNumber == 2)
        startCoroutine(JumpAttack());


    }

    public void longDistance()
    {
        if(!isBusy && !closingDistance && !isWaiting)


        
        int longDis = Random.Range(1,5);

        if(longDis == 1)
        closeDistance();

        else if(longDis == 2)
        randomRanged();

        else
        isWaiting = true;
        
        Invoke("notWaiting",2);
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
