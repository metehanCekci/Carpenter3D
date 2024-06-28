using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxScript : MonoBehaviour
{

    public static SfxScript Instance { get; private set; }
    [SerializeField] AudioClip Jump;
    [SerializeField] AudioClip Dash;
    [SerializeField] AudioClip Slide;
    [SerializeField] AudioClip Slam;
    [SerializeField] AudioClip Fall;
    [SerializeField] AudioClip Attack;
    [SerializeField] AudioClip Hit;
    [SerializeField] AudioClip Footstep;
    [SerializeField] AudioClip Hurt;
    [SerializeField] AudioClip Parry;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
                if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Fazladan oluşturulan nesneyi yok et
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playJump()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Jump);
    }
    public void playDash()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Dash);
    }
    public void playSlide()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Slide);
    }
    public void playSlam()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Slam);
    }
    public void playFall()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(Fall);
    }
    public void playAttack()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Attack);
    }
    public void playHit()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Hit);
    }
    public void playFootstep()
    {
        this.gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.9f,1.1f);
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Footstep);
        Invoke("resetPitch",0.5f);
    }

    public void playHurt()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Hurt);
    }
    public void playParry()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Parry);        
    }

    public void resetPitch()
    {   
        this.gameObject.GetComponent<AudioSource>().pitch = 1;
    }
}
