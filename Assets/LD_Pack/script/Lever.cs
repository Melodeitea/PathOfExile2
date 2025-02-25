using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isUsable = true;  // Can the lever be used?
    private bool activated = false;
    private int ID = 0;
    public int KeyCode = 0;
    public UnlockableElement linkedDoor;  // Reference to the door
    private bool playerInRange = false; // Whether the player is near the lever

    public void SetLever(UnlockableElement locker, int id, int keycode)
    {
        linkedDoor = locker;
        ID = id;
        KeyCode = keycode;
    }

    void Update()
    {
        // Check if the player is pressing "E" and if they are near the lever
        if (playerInRange && isUsable && !activated && Input.GetKeyDown(UnityEngine.KeyCode.E))
        {
            Use();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone of the lever
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player is near the lever: " + this.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger zone, stop interaction
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left the lever area.");
        }
    }

    public void Use()
    {
        if (isUsable && !activated)
        {
            Debug.Log($"Lever {this.name} activated.");
            isUsable = false;
            activated = true;
            linkedDoor?.Unlock(ID, KeyCode);  // Send unlock request to the linked door
        }
    }

    // Optional: Reset the lever (if needed in the future)
    public void ResetLever()
    {
        activated = false;
        isUsable = true;
    }
}
