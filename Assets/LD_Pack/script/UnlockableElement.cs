using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class UnlockableElement : MonoBehaviour
{
    public int KeyCode = 0;
    private int LockNumber = 0;
    private int UnlockNumber = 0;
    public List<Lever> Levers = new List<Lever>();
    private bool isUnlocked = false;

    public AudioClip unlockSound;
    private AudioSource audioSource;
    public GameObject glowEffect;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

            // Play unlock sound
            if (unlockSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(unlockSound);
            }

            // Show glow effect
            if (glowEffect != null)
            {
                glowEffect.SetActive(true);
            }

            // Open door (move up by 2 units)
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(0, 2, 0);

        while (time < 1f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        // Rebake NavMesh
        Unity.AI.Navigation.NavMeshSurface navMeshSurface = FindObjectOfType<Unity.AI.Navigation.NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
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
