using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using TMPro;

public class KeyDoor : MonoBehaviour
{
	public int requiredKeyID;
	private bool isUnlocked = false;
	public AudioClip unlockSound;
	public AudioClip doorOpenSound;
	public TextMeshProUGUI hudMessage;
	public TextMeshProUGUI keyCounter;
	private AudioSource audioSource;

	public NavMeshSurface navMeshSurface;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();

		if (player != null && player.HasKey(requiredKeyID))
		{
			UnlockDoor(player);
		}
		else
		{
			if (hudMessage != null) hudMessage.GetComponent<TextMeshProUGUI>().text = ("You need the correct key to open this door!");
		}
	}

	private void UnlockDoor(Player player)
	{
		if (!isUnlocked)
		{
			isUnlocked = true;
			player.UseKey(requiredKeyID);
			if (unlockSound != null) audioSource.PlayOneShot(unlockSound);
			if (hudMessage != null) hudMessage.GetComponent<TextMeshProUGUI>().text = "Porte déverrouillée";
			if (keyCounter != null) keyCounter.GetComponent<TextMeshProUGUI>().text = $"{player.GetKeyCount()}/3 clés restantes";
			OpenDoor();
			if (navMeshSurface != null)
			{
				navMeshSurface.BuildNavMesh();  // Rebuild the NavMesh after unlocking
			}
		}
	}

	private void OpenDoor()
	{
		
		transform.Rotate(0, -90, 0);

		// Play door opening sound
		if (doorOpenSound != null) audioSource.PlayOneShot(doorOpenSound);

		// Disable collider
		Collider doorCollider = GetComponent<Collider>();
		if (doorCollider != null)
		{
			doorCollider.enabled = false;
		}

		// Update NavMesh
		if (navMeshSurface != null)
		{
			navMeshSurface.BuildNavMesh();
		}
	}
}
