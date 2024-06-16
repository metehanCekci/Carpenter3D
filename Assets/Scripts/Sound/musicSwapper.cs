using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicSwapper : MonoBehaviour
{
    [SerializeField] AudioSource calm;
    [SerializeField] AudioSource combat;
    [SerializeField] bool phaseToCombat = false;

    [SerializeField] bool phaseToCalm = false;

    [SerializeField] float phaseSpeed = 0.5f;

    [SerializeField] float audioLevel = 0.25f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (phaseToCombat)
        {
            if (combat.volume < audioLevel)
            {
                calm.volume -= phaseSpeed * Time.deltaTime;
                combat.volume += phaseSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (calm.volume < audioLevel)
            {
                calm.volume += phaseSpeed * Time.deltaTime;
                combat.volume -= phaseSpeed * Time.deltaTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (phaseToCombat == false)
            {
                phaseToCombat = true;
                phaseToCalm = false;
            }
            else
            {
                phaseToCalm = true;
                phaseToCombat = false;
            }

    }
}
