using UnityEngine;

public class gold : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	int goldvalue = 100;
	private GoldManager goldManager; // Reference to the GoldManager

	private void Start()
	{
		// Find the GoldManager in the scene
		goldManager = Object.FindFirstObjectByType<GoldManager>();

		if (goldManager == null)
		{
			Debug.LogError("GoldManager not found in the scene!");
		}
	}

	// Sets the gold value (customizable for each object)
	public void SetGoldValue(int value)
	{
		goldvalue = value;
	}
	public int GetGoldValue()
	{
		return goldvalue;
	}
	public int Pickup()
	{
		Debug.Log($"Gold picked up! Value: {goldvalue}");

		// Trigger feedback through the GoldManager
		if (goldManager != null)
		{
			goldManager.PlayGoldFeedback(this.transform.position);
		}

		this.gameObject.SetActive(false);

		return goldvalue; // Return the gold value to the player
	}
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnEnter");
		if (other.tag == "Player")
		{
			// The player script will handle the actual collection and UI update
		}
	}
}
