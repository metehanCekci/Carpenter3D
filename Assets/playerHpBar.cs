using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance { get; private set; } // Singleton instance

    public float playerHp = 100f; // Oyuncunun can puanı
    public float maxHp = 100f; // Maksimum can puanı
    private Image hpBarImage; // Can barının Image bileşeni

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // If an instance already exists and it's not this instance, destroy this instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Set this instance as the singleton instance and don't destroy it on load
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hpBarImage = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Can barının doluluk oranını hesapla ve ayarla
        float fillAmount = playerHp / maxHp;
        hpBarImage.fillAmount = fillAmount;

        if(playerHp<0) SceneManager.LoadScene(0);
    }
}
