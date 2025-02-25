using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public int keyID = 0;  // Unique key ID
    public AudioClip keyPickupSound; // Sound when the key is collected
    public GameObject keyGlowEffect; // Effect when the key is collected
    public Text keyHudText; // Text to display how many keys are collected

    private AudioSource audioSource; // Reference to AudioSource

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Initialize the AudioSource
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play key collection sound if available
            if (audioSource && keyPickupSound != null)
            {
                audioSource.PlayOneShot(keyPickupSound);
            }

            // Play key collection visual effect
            if (keyGlowEffect != null)
            {
                keyGlowEffect.SetActive(true);
            }

            // Update HUD text with key collection count
            if (keyHudText != null)
            {
                keyHudText.text = "1/3 Keys Collected"; // Update with dynamic count
            }

            // Destroy key after collection
            Destroy(gameObject);
        }
    }
}