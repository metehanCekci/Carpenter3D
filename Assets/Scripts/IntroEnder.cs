using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IntroEnder : MonoBehaviour
{
    public float enemySpeed;
    public GameObject mus;
    public FollowScript fs;
    public GameObject close;
    public GameObject afar;
    public GameObject bossbar;
    public GameObject SpotLight;
    public GameObject AreaLight;
    public GameObject subtitles;

    public GameObject trigger;
    public EnemyHealthScript ehs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startIntro()
    {
        SpotLight.SetActive(true);
        fs.isRotating = false;
        subtitles.GetComponent<SubtitleScript>().playSub();
        SfxScript.Instance.playLightSwitch();

        trigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float remainingTime = (this.gameObject.GetComponent<AudioSource>().clip.length - this.gameObject.GetComponent<AudioSource>().time);
        if(this.gameObject.activeInHierarchy)
        if(remainingTime <= 0)
        {
            IntroEnd();
        }

        
    }

    public void IntroEnd()
    {
            mus.SetActive(true);

            fs.isFollowing = true;

            close.SetActive(true);
            afar.SetActive(true);
            ehs.enabled = true;
            bossbar.SetActive(true);
            SpotLight.SetActive(false);
            AreaLight.SetActive(true);
            fs.isRotating = true;
            fs.gameObject.GetComponent<Animator>().SetBool("isWalking" , true);
            fs.gameObject.GetComponent<NavMeshAgent>().speed = enemySpeed;
            fs.gameObject.GetComponent<KaruiAi>().enabled = true;
            SfxScript.Instance.playLightSwitch();

            
            this.gameObject.SetActive(false);
    }
}
