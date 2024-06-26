using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHpBar : MonoBehaviour
{

    public Slider healthSlider;

    public Slider easeSlider;
    public float maxHp = 100f; // Maksimum can puanı
    public float hp; // Maksimum can puanı

    private float lerpSpeed = 0.1f;
    [SerializeField] PlayerMovement pm;
    [SerializeField] Canvas deathMenu;

    // Awake is called when the script instance is being loaded
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // Can barının doluluk oranını hesapla ve ayarla
        healthSlider.value = hp;

        if(hp<=0) {
            Time.timeScale = 0;
            pm.isDead = true;
            deathMenu.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            }

        if(healthSlider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, hp, lerpSpeed);
        }
    }
}
