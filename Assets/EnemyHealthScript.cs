using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    public float hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = hp.ToString();
    }

    public void takeDamage(float damageAmt)
    {
        hp-=damageAmt;
        if(hp<0) death();
    }

    public void death()
    {
        this.gameObject.SetActive(false);
    }
}
