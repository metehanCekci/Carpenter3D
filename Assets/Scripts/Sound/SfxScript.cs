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
    [SerializeField] AudioClip Swing;
    [SerializeField] AudioClip LightSwitch;
    [SerializeField] AudioClip Crush;
    [SerializeField] AudioClip Woosh;

    private AudioSource audioSource;
    private AudioSource loopAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            audioSource = GetComponent<AudioSource>();
            loopAudioSource = gameObject.AddComponent<AudioSource>();
            loopAudioSource.loop = true;
        }
        else
        {
            Destroy(gameObject); // Destroy extra instance
        }
    }

    public void playJump()
    {
        audioSource.PlayOneShot(Jump);
    }

    public void playDash()
    {
        audioSource.PlayOneShot(Dash);
    }

    public void playSlide()
    {
        if (!loopAudioSource.isPlaying)
        {
            loopAudioSource.clip = Slide;
            loopAudioSource.Play();
        }
    }

    public void stopSlide()
    {
        if (loopAudioSource.isPlaying)
        {
            loopAudioSource.Stop();
        }
    }

    public void playSlam()
    {
        audioSource.PlayOneShot(Slam);
    }

    public void playFall()
    {
        audioSource.PlayOneShot(Fall);
    }

    public void playAttack()
    {
        audioSource.PlayOneShot(Attack);
    }

    public void playCrush()
    {
        audioSource.PlayOneShot(Crush);
    }

    public void playLightSwitch()
    {
        audioSource.PlayOneShot(LightSwitch);
    }

    public void playHit()
    {
        audioSource.PlayOneShot(Hit);
    }

    public void playFootstep()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(Footstep);
        Invoke("resetPitch", 0.5f);
    }

    public void playHurt()
    {
        audioSource.PlayOneShot(Hurt);
    }

    public void playParry()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(Parry);
        Invoke("resetPitch", 0.5f);
    }

    public void playSwing()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(Swing);
        Invoke("resetPitch", 0.5f);
    }

    public void playWoosh()
    {
        audioSource.PlayOneShot(Woosh);
    }

    public void resetPitch()
    {
        audioSource.pitch = 1;
    }
}
