using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [Header("Feedback Settings")]
    public AudioClip goldPickupSound; // Sound effect for collecting gold
    public ParticleSystem goldPickupEffect; // Particle effect for gold collection
    public AudioSource audioSource; // AudioSource for playing sounds

    // Plays the feedback for gold collection
    public void PlayGoldFeedback(Vector3 position)
    {
        // Play the pickup sound
        PlayGoldPickupSound();

        // Trigger the particle effect at the gold's position
        TriggerGoldPickupEffect(position);
    }

    // Plays the sound effect
    private void PlayGoldPickupSound()
    {
        if (audioSource != null && goldPickupSound != null)
        {
            audioSource.PlayOneShot(goldPickupSound);
        }
        else
        {
            Debug.LogWarning("Gold pickup sound or AudioSource is not assigned!");
        }
    }

    // Spawns the particle effect
    private void TriggerGoldPickupEffect(Vector3 position)
    {
        if (goldPickupEffect != null)
        {
            // Instantiate the particle effect at the given position
            ParticleSystem effect = Instantiate(goldPickupEffect, position, Quaternion.identity);
            Destroy(effect.gameObject, effect.main.duration); // Destroy the effect after it finishes
        }
        else
        {
            Debug.LogWarning("Gold pickup particle effect is not assigned!");
        }
    }
}
