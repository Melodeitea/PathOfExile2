using UnityEngine;
using System.Collections.Generic;

public class UnlockableElement : MonoBehaviour
{
    public int KeyCode = 0;
    private int LockNumber = 0;
    private int UnlockNumber = 0;
    [SerializeField]
    public List<Lever> Levers = new List<Lever>();
    private bool isUnlocked = false;

    void Start()
    {
        if (Levers.Count <= 0)
        {
            Debug.LogWarning(this.name + " has no lever");
        }
    }

    public void AddLock(Lever lever)
    {
        LockNumber++;
        Levers.Add(lever);
    }

    public List<Lever> ResetLevers()
    {
        List<Lever> tempLevers = new List<Lever>(Levers);
        Levers.Clear();
        LockNumber = 0;
        return tempLevers;
    }

    public virtual void OpenLock()
    {
        if (!isUnlocked)
        {
            Debug.Log("Door unlocked: " + this.name);
            isUnlocked = true;
            // Add actual logic to open the door (animation, collider disable, etc.)
            // Example: Disable the door's collider to allow the player to walk through
            Collider doorCollider = GetComponent<Collider>();
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }
        }
    }

    public virtual void CloseLock()
    {
        Debug.Log("Door locked: " + this.name);
        isUnlocked = false;
    }

    public virtual void Unlock(int id, int keycode)
    {
        if (keycode == KeyCode)
        {
            UnlockNumber++;
            Debug.Log("Lever activated for door " + this.name);
        }
        CheckOpening();
    }

    private void CheckOpening()
    {
        if (UnlockNumber >= LockNumber)
        {
            OpenLock();
        }
    }
}
