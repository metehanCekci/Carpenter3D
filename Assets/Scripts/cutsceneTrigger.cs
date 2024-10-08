using UnityEngine;
using System.Collections;

public class cutsceneTrigger : MonoBehaviour
{
    public Animator anim;
    public AudioSource ass;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("intro");
            anim.SetTrigger("Intro");

            ass.Play();
            ass.GetComponent<IntroEnder>().startIntro();
        }
    }

}