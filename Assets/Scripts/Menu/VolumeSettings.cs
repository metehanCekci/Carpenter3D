using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider mainSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public const string MIXER_MAIN = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    void Awake()
    {
        mainSlider.onValueChanged.AddListener(SetMainVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    void Start()
    {
        mainSlider.value = PlayerPrefs.GetFloat(AudioManager.MAIN_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MAIN_KEY, mainSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }
    void SetMainVolume(float value)
    {
        mixer.SetFloat(MIXER_MAIN, Mathf.Log10(value) * 20);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
    }
    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
