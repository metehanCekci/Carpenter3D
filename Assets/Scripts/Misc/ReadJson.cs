using UnityEngine;
using System.IO;

public class ReadJson : MonoBehaviour
{
    public static ReadJson Instance { get; private set; }
    
    [System.Serializable]
    public class Config
    {
        public bool hasDoubleJump;
        public bool hasGroundPound;
    }

    public Config config;

    void Start()
    {
        readAll();
    }

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            readAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void readAll()
    {
        // JSON dosyasını oku
        string path = Path.Combine(Application.streamingAssetsPath, "SaveFile.JSON");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            config = JsonUtility.FromJson<Config>(json);


        }
        else
        {
            Debug.LogError("JSON dosyası bulunamadı: " + path);
        }
    }
}
