using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class trap : MonoBehaviour
{
    private Player Player;
    private IEnumerator Cor;

    [Header("Trap Settings")]
    public int Damage = 30;
    public float LaunchTime = 1f; // Time before damage is dealt

    [Header("Trap Activation Feedback")]
    public AudioClip trapWhirrSound; // Sound before activation
    public ParticleSystem trapBlinkEffect; // VFX before activation

    [Header("Trap Damage Feedback")]
    public AudioClip impactSound; // 🔹 NEW: Sound when player is hit
    public ParticleSystem damageEffect; // VFX when player is hit
    public Image dmgFlick; // 🔹 Stays active, only changes transparency
    public TextMeshProUGUI damageTextUI; // UI text for "Damage Taken!"

    private AudioSource audioSource;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 🔹 Ensure dmgFlick starts fully transparent (0 alpha)
        if (dmgFlick != null)
        {
            Color flickColor = dmgFlick.color;
            flickColor.a = 0f; // Set to fully transparent
            dmgFlick.color = flickColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player))
        {
            Cor = Launch(LaunchTime);
            StartCoroutine(Cor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player))
        {
            if (Player != null && Cor != null)
            {
                StopCoroutine(Cor);
                Player = null;
            }
        }
    }

    private IEnumerator Launch(float time)
    {
        Debug.Log("Trap activated, preparing to deal damage...");

        // 🔹 TRAP ACTIVATION FEEDBACK 🔹
        if (trapBlinkEffect != null)
        {
            Instantiate(trapBlinkEffect, transform.position, Quaternion.identity);
        }
        if (audioSource != null && trapWhirrSound != null)
        {
            audioSource.PlayOneShot(trapWhirrSound);
        }

        yield return new WaitForSeconds(time);

        if (Player != null)
        {
            Player.TakeHit(Damage);
            Debug.Log($"Player took {Damage} damage from trap!");

            // 🔹 TRAP DAMAGE FEEDBACK 🔹
            if (damageEffect != null)
            {
                Instantiate(damageEffect, Player.transform.position, Quaternion.identity);
            }
            if (audioSource != null && impactSound != null)
            {
                audioSource.PlayOneShot(impactSound); // 🔹 NEW: Play damage impact sound
            }
            if (dmgFlick != null)
            {
                StartCoroutine(FadeDamageFlick());
            }
            if (damageTextUI != null)
            {
                StartCoroutine(ShowDamageText());
            }

            // Restart trap sequence (continuous damage if player stays)
            Cor = Launch(LaunchTime);
            StartCoroutine(Cor);
        }
    }

    // 🔹 UI FEEDBACK: DAMAGE FLICK TRANSPARENCY CHANGE 🔹
    private IEnumerator FadeDamageFlick()
    {
        // Step 1: Set to semi-transparent (alpha 0.5)
        float fadeInDuration = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            Color flickColor = dmgFlick.color;
            flickColor.a = Mathf.Lerp(0f, 0.5f, elapsedTime / fadeInDuration);
            dmgFlick.color = flickColor;
            yield return null;
        }

        // Step 2: Fade out to fully transparent (alpha 0)
        float fadeOutDuration = 0.3f;
        elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            Color flickColor = dmgFlick.color;
            flickColor.a = Mathf.Lerp(0.5f, 0f, elapsedTime / fadeOutDuration);
            dmgFlick.color = flickColor;
            yield return null;
        }
    }

    // 🔹 UI FEEDBACK: SHOW "DAMAGE TAKEN" TEXT 🔹
    private IEnumerator ShowDamageText()
    {
        damageTextUI.text = "Dégâts subis!";
        damageTextUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        damageTextUI.gameObject.SetActive(false);
    }
}
