using System.Collections;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    public AudioClip refillSound;              // Sound to play when the fountain refills the player
    private AudioSource audioSource;           // AudioSource component for playing sound
    public ParticleSystem refillParticles;     // Particle system for the fountain refill effect
    public Player player;                      // Reference to the Player script to refill mana/crystals

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        refillParticles?.Stop();  // Ensure particles are stopped at the start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered fountain: Refilling mana and crystal!");

            // Get the Player script and call RefillManaAndCrystal with a suitable refill amount
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                // Call RefillManaAndCrystal directly from the Player script
                float refillAmount = 50f;  // Set the refill amount to a specific value, or use another approach
                player.RefillManaAndCrystal(refillAmount);
            }

            PlayRefillFeedback(); // Play the sound and particle effect
        }
    }



    private void PlayRefillFeedback()
    {
        audioSource?.PlayOneShot(refillSound);  // Play the refill sound
        refillParticles?.Play();                // Play the particle effect
    }
}
