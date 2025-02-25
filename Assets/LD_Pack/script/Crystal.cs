using UnityEngine;

public class Crystal : MonoBehaviour
{
    public AudioClip collectSound;
    public int manaValue = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var crystalManager = FindObjectOfType<CrystalManager>();
            if (crystalManager != null)
            {
                crystalManager.AddCrystalMana(manaValue, this.gameObject);
                var player = FindObjectOfType<Player>();
                player?.ActivateCrystal(manaValue);
            }

            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            gameObject.SetActive(false);  // Deactivate crystal after pickup
        }
    }
}
