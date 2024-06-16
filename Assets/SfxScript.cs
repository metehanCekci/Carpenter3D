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
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Jump);
    }
    public void playDash()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Dash);
    }
    public void playSlide()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Slide);
    }
    public void playSlam()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Slam);
    }
    public void playFall()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(Fall);
    }
}
