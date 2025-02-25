using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isUsable = true;  // Check if the lever can be used
    private bool isActivated = false;  // Track if the lever has been activated
    public int ID = 0;  // Lever's ID
    public int KeyCode = 0;  // Keycode for the lever to match
    public UnlockableElement unlockableElement = null;  // Reference to the associated unlockable element (e.g., door)

    void Start()
    {
        // Initialize lever if needed (e.g., set up default rotation, etc.)
    }

    // This method is used to link the lever to an unlockable element (like a door)
    public void SetLever(UnlockableElement locker, int id, int keycode)
    {
        unlockableElement = locker;  // Set the reference to the unlockable element (door)
        ID = id;  // Set the ID for the lever
        KeyCode = keycode;  // Set the keycode to unlock the element
    }

    // This method is called when the player presses the "E" key to interact with the lever
    public void Use()
    {
        if (isUsable && !isActivated)
        {
            isUsable = false;  // The lever can only be used once until reset
            isActivated = true;  // Mark the lever as activated

            // Rotate the lever by -90 degrees on the Z-axis (flip the lever)
            FlipLever();

            // Unlock the associated unlockable element (e.g., door)
            unlockableElement.Unlock(ID, KeyCode);

            // Play activation sound or visual effects (optional)
            PlayActivationSound();
        }
    }

    // Method to flip the lever when activated (rotate it -90 degrees on the Z-axis)
    private void FlipLever()
    {
        // Rotate the lever by -90 degrees on the Z-axis
        transform.Rotate(0, 0, -90);
        Debug.Log("Lever flipped!");
    }

    // Play sound effect when the lever is activated (optional)
    private void PlayActivationSound()
    {
        // Insert sound for lever activation (e.g., "click" or "clank" sound)
        Debug.Log("Lever activation sound played!");
    }
}
