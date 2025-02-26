using UnityEngine;
using System.Collections;
using TMPro;

public class Key : MonoBehaviour
{
    public int keyID;
    public AudioClip pickupSound;
    public GameObject pickupEffect;
    private AudioSource audioSource;
    public TextMeshProUGUI hudMessage;
    public TextMeshProUGUI keyCounter;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log($"Player picked up Key {keyID}");
            player.CollectKey(keyID);

            // Play pickup sound
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Update HUD
            if (hudMessage != null) hudMessage.GetComponent<TextMeshProUGUI>().text = "Clé collectée !";
            if (keyCounter != null) keyCounter.GetComponent<TextMeshProUGUI>().text = $"{player.collectedKeys}/3 clés collectées"; 


            // Visual effect
            StartCoroutine(KeyPickupEffect());
        }
    }

    private IEnumerator KeyPickupEffect()
    {
        float time = 0f;
        while (time < 1f)
        {
            transform.Rotate(0, 180 * Time.deltaTime, 0);
            transform.position += Vector3.up * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        // Instantiate sparkle effect
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
}
