using UnityEngine;
using System.IO;

public class ReadJson : MonoBehaviour
{
    [System.Serializable]
    public class Config
    {
        public bool hasDoubleJump;
        public bool hasGroundPound;

    }

    void Start()
    {
        readAll();
    }

    public void readAll()
    {
        // JSON dosyasını oku
        string path = Path.Combine(Application.streamingAssetsPath, "SaveFile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Config config = JsonUtility.FromJson<Config>(json);

            // hasDoubleJump değerini yazdır
            bool hasDoubleJump = config.hasDoubleJump;
            bool hasGroundPound = config.hasDoubleJump;
        }
        else
        {
            Debug.LogError("JSON dosyası bulunamadı: " + path);
        }
    }
}

