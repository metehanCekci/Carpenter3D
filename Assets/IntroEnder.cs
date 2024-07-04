using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEnder : MonoBehaviour
{
    public GameObject mus;
    public FollowScript fs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float remainingTime = (this.gameObject.GetComponent<AudioSource>().clip.length - this.gameObject.GetComponent<AudioSource>().time);
        if(remainingTime <= 0)
        {
            mus.SetActive(true);
            fs.isFollowing = true;
        }

        
    }
}
