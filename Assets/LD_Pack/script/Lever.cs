using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isUsable = true;
    private bool activated = false;
    private int ID = 0;
    public int KeyCode = 0;
    public UnlockableElement linkedDoor;
    private bool playerInRange = false;

    public AudioClip leverSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInRange && isUsable && !activated && Input.GetKeyDown(UnityEngine.KeyCode.E))
        {
            Use();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void SetLever(UnlockableElement locker, int id, int keycode)
    {
        linkedDoor = locker;
        ID = id;
        KeyCode = keycode;
    }

    public void Use()
    {
        if (isUsable && !activated)
        {
            Debug.Log($"Lever {this.name} activated.");
            isUsable = false;
            activated = true;

            // Play lever sound
            if (leverSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(leverSound);
            }

            // Lower the lever
            transform.Rotate(0, 0, -90);

            // Unlock linked door
            linkedDoor?.Unlock(ID, KeyCode);
        }
    }
}
