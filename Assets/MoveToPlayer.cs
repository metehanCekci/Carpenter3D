using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float moveSpeed = 100f;  // Hareket hızı
    private Vector3 direction;  // Hareket yönü
    private bool isMovingToPlayer = true;  // İlk hareketin player'a doğru olduğunu belirten bayrak

    void Awake()
    {
        // Player objesini etiket ile bulma
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Player konumuna doğru olan yönü hesaplama (sadece yatay düzlemde)
            Vector3 playerPosition = player.transform.position;
            Vector3 currentPosition = transform.position;
            direction = (new Vector3(playerPosition.x, currentPosition.y, playerPosition.z) - currentPosition).normalized;
        }
        else
        {
            Debug.LogError("Player objesi bulunamadı! Lütfen 'Player' tag'li bir obje olduğundan emin olun.");
        }
    }

    void Update()
    {
        // Hareket etme
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (isMovingToPlayer)
        {
            // Player konumuna ulaştıysak
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && Vector3.Distance(transform.position, player.transform.position) < 0.1f)
            {
                // Hareket yönünü tersine çevirme
                direction = -direction;
                isMovingToPlayer = false;  // Player'a ulaştığımızı belirten bayrağı kapat
            }
        }
    }
}
