using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEnder : MonoBehaviour
{
    public GameObject mus;
    public FollowScript fs;
    public GameObject close;
    public GameObject afar;
    public GameObject bossbar;
    public GameObject SpotLight;
    public GameObject AreaLight;
    public GameObject subtitles;
    public EnemyHealthScript ehs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        SpotLight.SetActive(true);
        fs.isRotating = false;
        subtitles.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        float remainingTime = (this.gameObject.GetComponent<AudioSource>().clip.length - this.gameObject.GetComponent<AudioSource>().time);
        if(remainingTime <= 0)
        {
            mus.SetActive(true);
            fs.isFollowing = true;
            fs.gameObject.GetComponent<KaruiAi>().enabled = true;
            close.SetActive(true);
            afar.SetActive(true);
            ehs.enabled = true;
            bossbar.SetActive(true);
            SpotLight.SetActive(false);
            AreaLight.SetActive(true);
            fs.isRotating = true;
            fs.gameObject.GetComponent<Animator>().SetBool("isWalking" , true);
            
            Destroy(this);
        }

        
    }
}
