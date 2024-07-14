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
            ReadJson.Instance.ReadSaveQuick();
            if(!ReadJson.Instance.saveQuick.bossIntro)
            {
            Debug.Log("intro");
            anim.SetTrigger("Intro");

            ass.Play();
            ass.GetComponent<IntroEnder>().startIntro();

            ReadJson.Instance.saveQuick.bossIntro = true;
            }
            else
            {
                ass.GetComponent<IntroEnder>().IntroEnd();
            }
        }
    }

}