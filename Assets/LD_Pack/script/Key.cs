using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyID;  // Unique identifier for this key

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log($"Player picked up Key {keyID}");
            player.CollectKey(keyID);
            gameObject.SetActive(false); // Hide the key once collected
        }
    }
}
