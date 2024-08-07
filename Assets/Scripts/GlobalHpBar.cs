using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalHpBar : MonoBehaviour
{

    public bool notPlayer = false;
    public Slider healthSlider;

    public Slider easeSlider;
    public float maxHp; // Maksimum can puanı
    public float hp; // Maksimum can puanı

    private float lerpSpeed = 0.1f;
    [SerializeField] PlayerMovement pm;
    [SerializeField] Canvas deathMenu;
    Camera cam;

    public bool isDead = false;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        cam = Camera.main;


    }

    

    // Start is called before the first frame update
    void Start()
    {
        if(!notPlayer)
        {
        ReadJson.Instance.ReadSaveFile();
        healthSlider.maxValue = ReadJson.Instance.saveFile.maxHP;
        easeSlider.maxValue = ReadJson.Instance.saveFile.maxHP;
        maxHp = ReadJson.Instance.saveFile.maxHP;
        }

        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // Can barının doluluk oranını hesapla ve ayarla
        healthSlider.value = hp;
        /*Debug.Log($"Is dead: {pm.isDead}");
        Debug.Log($"Time scale: {Time.timeScale}");*/

        if (hp <= 0 && !notPlayer && !isDead)
        {
            pm.isDead = true;
            pm.enabled = false;
            deathMenu.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            cam.GetComponent<CameraController>().enabled = false;
            isDead = true;
            Time.timeScale = 0;
        }

        if (healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, hp, lerpSpeed);
        }
    }
}
