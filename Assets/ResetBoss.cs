using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoss : MonoBehaviour
{
    public bool reset = false;
    public GameObject music;
                public GameObject trigger;
                public GameObject bossBar;
    public void Reset()
    {
        reset = true;
        this.gameObject.GetComponent<KaruiAi>().enabled = false;
        trigger.SetActive(true);
        

        music.SetActive(false);
        bossBar.GetComponent<GlobalHpBar>().hp = bossBar.GetComponent<GlobalHpBar>().maxHp;
        bossBar.GetComponent<GlobalHpBar>().healthSlider.maxValue = bossBar.GetComponent<GlobalHpBar>().maxHp;
        bossBar.GetComponent<GlobalHpBar>().easeSlider.maxValue = bossBar.GetComponent<GlobalHpBar>().maxHp;
        bossBar.SetActive(false);
    }
}
