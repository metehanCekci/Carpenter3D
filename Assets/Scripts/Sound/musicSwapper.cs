using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicSwapper : MonoBehaviour
{
    [SerializeField] AudioSource calm;
    [SerializeField] AudioSource combat;
    [SerializeField] public bool isCombat = false;

    [SerializeField] float phaseSpeed = 0.5f;
    [SerializeField] float audioLevel = 0.5f;

    void Start()
    {
        // Ensure initial volumes are set
        calm.volume = audioLevel;
        combat.volume = 0;
    }

    void Update()
    {
        if (this.enabled)
        {
            try
            {
                if (isCombat)
                {
                    if (combat.volume < audioLevel)
                    {
                        calm.volume = Mathf.Max(0, calm.volume - phaseSpeed * Time.deltaTime);
                        combat.volume = Mathf.Min(audioLevel, combat.volume + phaseSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    if (calm.volume < audioLevel)
                    {
                        calm.volume = Mathf.Min(audioLevel, calm.volume + phaseSpeed * Time.deltaTime);
                        combat.volume = Mathf.Max(0, combat.volume - phaseSpeed * Time.deltaTime);
                    }
                }
            }
            catch
            {
                // Handle exceptions if necessary
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isCombat)
            {
                isCombat = true;
            }
        }
    }
}
