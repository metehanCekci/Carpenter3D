using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResetBoss : MonoBehaviour
{
    public GameObject music;
    public GameObject trigger;
    public GameObject bossBar;

    private void Start() {

    }
    public void Reset()
    {
        this.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        this.gameObject.GetComponent<KaruiAi>().enabled = false;
        this.gameObject.GetComponent<FollowScript>().isFollowing = false;
        this.gameObject.GetComponent<FollowScript>().isRotating = false;
        Debug.Log("off");
        trigger.SetActive(true);


        music.SetActive(false);
        this.gameObject.GetComponent<EnemyHealthScript>().hp = 1000;
        bossBar.GetComponent<GlobalHpBar>().hp = bossBar.GetComponent<GlobalHpBar>().maxHp;
        bossBar.GetComponent<GlobalHpBar>().healthSlider.maxValue = bossBar.GetComponent<GlobalHpBar>().maxHp;
        bossBar.GetComponent<GlobalHpBar>().easeSlider.maxValue = bossBar.GetComponent<GlobalHpBar>().maxHp;
        bossBar.SetActive(false);
    }
}
