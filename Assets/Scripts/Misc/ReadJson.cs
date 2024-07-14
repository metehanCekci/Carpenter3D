using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ReadJson : MonoBehaviour
{
    public static ReadJson Instance { get; private set; }
    
    [System.Serializable]
    public class SaveFile
    {
        // BOOLS
        public bool hasDoubleJump;
        public bool hasGroundPound;
        public bool hasParry;
        public bool hasDash;
        public bool hasAxe;
        
        // FLOATS
        public float level;
        public float exp;
        public float maxSyringe;
        public float syringePower;
        public float maxHP;
        public float attackPower;
        public float attackSpeed;
        public float attackReach;
        
        // CHECKPOINTS
        [System.Serializable]
        public class Bonfire
        {
            public int BonfireID;
        }
        public List<Bonfire> bonfires = new List<Bonfire>();

        // BOSSES
        [System.Serializable]
        public class Boss
        {
            public int BossID;
        }
        public List<Boss> bosses = new List<Boss>();
    }

    [System.Serializable]
    public class SaveQuick
    {
        public float HP;
        public float Energy;
        public float syringeCount;

        [System.Serializable]
        public class Enemy
        {
            public int enemyID;
        }
        public List<Enemy> enemies = new List<Enemy>();

        public bool bossIntro;
    }

    public SaveFile saveFile = new SaveFile();
    public SaveQuick saveQuick = new SaveQuick();

    void Start()
    {
        ReadSaveFile();
        ReadSaveQuick();
    }

    void FixedUpdate()
    {
        WriteSaveQuick();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ReadSaveFile();
            ReadSaveQuick();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReadSaveFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "SaveFile.JSON");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveFile = JsonUtility.FromJson<SaveFile>(json);
        }
        else
        {
            Debug.LogError("JSON file not found: " + path);
        }
    }

    public void ReadSaveQuick()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "SaveQuick.JSON");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveQuick = JsonUtility.FromJson<SaveQuick>(json);
        }
        else
        {
            Debug.LogError("JSON file not found: " + path);
        }
    }

    public void WriteSaveFile()
    {
        string json = JsonUtility.ToJson(saveFile, true);
        string path = Path.Combine(Application.streamingAssetsPath, "SaveFile.JSON");
        File.WriteAllText(path, json);
    }

    public void WriteSaveQuick()
    {
        string json = JsonUtility.ToJson(saveQuick, true);
        string path = Path.Combine(Application.streamingAssetsPath, "SaveQuick.JSON");
        File.WriteAllText(path, json);
    }
}
