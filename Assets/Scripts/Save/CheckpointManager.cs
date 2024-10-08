using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    private List<Checkpoint> checkpoints;
    private int currentCheckpoint = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadCheckpoint();
    }

    void Start()
    {
        checkpoints = new List<Checkpoint>(FindObjectsOfType<Checkpoint>());
        checkpoints.Sort((a, b) => a.CheckpointNumber.CompareTo(b.CheckpointNumber));
    }

    public void UpdateCheckpoint(int checkpointNumber)
    {
        currentCheckpoint = checkpointNumber;
        PlayerPrefs.SetInt("CurrentCheckpoint", currentCheckpoint);
    }

    private void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CurrentCheckpoint"))
        {
            currentCheckpoint = PlayerPrefs.GetInt("CurrentCheckpoint");
        }
    }

    public Vector3 GetCheckpointPosition()
    {
        // BAD LINE bu kodumun kodu returnlarken listenin çok büyük olduðunu savunuyor az kaldý 
        return checkpoints[currentCheckpoint].transform.position;
    }
}
