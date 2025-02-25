using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public int requiredKeyID;  // The ID of the key required to open the door
    private bool isUnlocked = false;  // Tracks whether the door is unlocked

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        // If the player collides with the door and they have the required key, unlock the door
        if (player != null)
        {
            if (player.HasKey(requiredKeyID))
            {
                UnlockDoor(player);
            }
            else
            {
                Debug.Log("You need the correct key to open this door!");
            }
        }
    }

    private void UnlockDoor(Player player)
    {
        if (!isUnlocked)
        {
            isUnlocked = true;  // Set the door to unlocked

            // Optionally, use the key after unlocking (remove it from the player's inventory)
            player.UseKey(requiredKeyID);

            // Log the success and trigger door open animation or state change
            Debug.Log("Door unlocked!");
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // This is where you would implement your door opening logic.
        // You could animate the door, disable the collider, or activate the door’s open state.

        // Example: Open the door (using a simple animation or state change)
        Animator doorAnimator = GetComponent<Animator>();
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open"); // Trigger an animation called "Open"
        }

        // Optionally, disable the collider to prevent the player from trying to open it again
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;  // Disable the collider so the player can't interact with it again
        }
    }
}
