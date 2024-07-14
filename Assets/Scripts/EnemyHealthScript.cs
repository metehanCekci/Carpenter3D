using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    public float hp;
    public float maxhp;
    public bool isBoss = false;

    public GlobalHpBar bossHP;
    [SerializeField] bool isTesting = false;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
                if(isBoss)
            bossHP.hp = hp;
        else if (!isTesting)
            transform.GetChild(0).GetComponent<TMP_Text>().text = hp.ToString();


    }

    public void takeDamage(float damageAmt)
    {
        if (!isTesting)
        {
            if(this.enabled)
            hp -= damageAmt;
            if (hp <= 0) death();
        }
    }

    public void death()
    {
        this.gameObject.SetActive(false);
    }
}
