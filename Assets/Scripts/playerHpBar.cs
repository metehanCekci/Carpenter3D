using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHpBar : MonoBehaviour
{

    public float playerHp = 100f; // Oyuncunun can puanı
    public float maxHp = 100f; // Maksimum can puanı
    private Image hpBarImage; // Can barının Image bileşeni
    [SerializeField] PlayerMovement pm;
    [SerializeField] Canvas deathMenu;

    // Awake is called when the script instance is being loaded
    void Awake()
    {

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

        if(playerHp<=0) {
            Time.timeScale = 0;
            pm.isDead = true;
            deathMenu.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            }
    }
}
