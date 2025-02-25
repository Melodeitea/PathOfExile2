using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
	// UI References
	public Slider healthBar; // UI Slider for player HP
	public TextMeshProUGUI healthText; // UI Text for player HP

	public Slider manaBar; // UI Slider for mana
	public TextMeshProUGUI manaText; // UI Text for mana

	public Slider crystalManaBar; // UI Slider for crystal mana
	public TextMeshProUGUI crystalManaText; // UI Text for crystal mana

	// Add variables for the crystal mana
	public float CrystalManaMax = 100;
	public float CrystalMana = 100;
	public bool isCrystalActive; // Whether a crystal is active
	public CrystalManager crystalManager; // Reference to CrystalManager

	// keys n levers
	private List<int> collectedKeyIds = new List<int>();  // List of key IDs player has
	private List<int> activatedLevers = new List<int>();  // List of lever codes activated by player

	int PlayerCrystal = 0;
	int PlayerGold = 0;
	public TextMeshProUGUI goldText; // Reference to the TextMeshPro UI element for gold

	List<Item> PlayerItem = new List<Item>();
	Vector3 lastcursorpointgroundtarget = Vector3.zero;
	GameObject UnderCursorEnnemy = null;

	//Base stats
	public int Strength = 15;
	public int Intelligence = 15;
	public int Dexterity = 15;

	//mana
	public float ManaMax = 200;
	public float Mana = 100;
	[Tooltip("In mana per second (%Manamax/s)")]// en %
	public float ManaRegen = 2;
	public float SpellManaCost = 30; // Mana cost for spells

	//Life
	public float LifeMax = 200;
	public float Life = 100;
	[Tooltip("In Life per second (Life/s)")]// en valeur brut
	public float LifeRegen = 2;

	// Energyshield
	public float EnergyShieldMax = 200;
	public float EnergyShield = 100;
	public float EnergyShieldRegen = 20; // en %
	public float EnergyShieldDelay = 4; // en seconde
	public float EnergyShieldHitRecently = 0;
	// Res en %
	public int FireResist = 0;
	public int IceResist = 0;
	public int LightningResist = 0;
	public int ChaosResist = 0;

	// Spells 

	// SpellsStat
	List<SpellData> PlayerSpells = new List<SpellData>();
	float CastingSpeed = 0.25f;
	private bool bCasting = false;

	//Spells prefab
	public GameObject ProjectileSpell;
	public GameObject MeteorSpell;
	public GameObject RebondSpell;
	public GameObject OrbSpell;

	// LightningJudgement spell prefab
	public LightningJudgement Judgement;

	// deplacement et navigation 
	NavMeshAgent agent;
	float MvtSpeed = 5;
	Vector3 LookAtPos = new Vector3();
	Vector3 LookAtDest = new Vector3();
	bool bLookAtSpell = false;
	float DelayBeforeStopLookingAtSpell = 1.5f;
	float LastCastSpell = 0;
	float SpeedRotation = 360; // ° per sec

	// Input
	bool Spell1_Hold = false;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.speed = MvtSpeed;
		crystalManager = FindObjectOfType<CrystalManager>();

		UpdateHealthUI();
		UpdateManaUI();
		UpdateGoldUI();
	}

	// Update is called once per frame
	void Update()
	{
		ManaRegenerated();
		LifeRegenerated();
		EnergyShieldManager();
		ManageCrystalMana();
		InputManager();
		RotateToLookAt();
		PickUpGold();
	}

	//---------------------------// Base codes //---------------------------//
	private void InputManager()
	{
		if (Spell1_Hold && bCasting) // relocate pos of spell
		{
			Judgement.ChangeDestinationPos(TargetGround());
			LastCastSpell = Time.time;
		}
	}
	public Vector3 TargetGround()
	{
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layerMask = LayerMask.GetMask("ground");

		if (Physics.Raycast(ray.origin, ray.direction, out hit, 50, layerMask))
		{
			lastcursorpointgroundtarget = hit.point;
			return hit.point;
		}
		else
		{
			return Vector3.zero;
		}
	}

	private void RotateToLookAt()
	{
		// If the vector is not zero, perform the look rotation
		if (LookAtDest != Vector3.zero)
		{
			Vector3 direction = LookAtDest - this.transform.position;

			if (direction.sqrMagnitude > 0.01f) // Check if the direction has a significant length (avoid tiny values)
			{
				Quaternion rotarget = Quaternion.LookRotation(direction);
				this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotarget, SpeedRotation * Time.deltaTime);
			}
		}
	}

	public GameObject TargetEnnemy()
	{
		Camera cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layerMask = LayerMask.GetMask("ennemy");

		if (Physics.Raycast(ray.origin, ray.direction, out hit, 50, layerMask))
		{
			UnderCursorEnnemy = hit.rigidbody.gameObject;
			return hit.rigidbody.gameObject;
		}
		else
		{
			return null;
		}
	}

	public void OnMoveTo(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			agent.SetDestination(TargetGround());
			LookAtDest = TargetGround();
			if (bLookAtSpell)
			{
				bLookAtSpell = false;
			}
		}
	}

	public void OnSpell_1(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			Spell1_Hold = true;
			if (!bCasting && Mana >= SpellManaCost)
			{
				CastSpell();
			}
		}
		else if (ctx.canceled)
		{
			Spell1_Hold = false;
			if (bCasting)
			{
				Judgement.DesactiveSpell();
				bCasting = false;
			}
		}
	}

	private void CastSpell()
	{
		// First attempt to use crystal mana, and fall back to regular mana if not enough crystal mana
		ManaConsumption(SpellManaCost); // This will use crystal mana first, then regular mana

		// If you still have enough mana after consumption, proceed to cast the spell
		if (Mana >= SpellManaCost)
		{
			UpdateManaUI(); // Update the UI after mana consumption
			Vector3 targetPos = TargetGround();
			Judgement.ActivateSpell(targetPos, this);
			bCasting = true;
			LookAtPos = targetPos;
			bLookAtSpell = true;
			LastCastSpell = Time.time;
		}
		else
		{
			Debug.Log("Not enough mana to cast the spell!");
		}
	}

	private void EnergyShieldManager()
	{
		if (EnergyShield < EnergyShieldMax && EnergyShieldHitRecently + EnergyShieldDelay <= Time.time)
		{
			EnergyShield += EnergyShieldRegen * EnergyShieldMax * Time.deltaTime;
			if (EnergyShield > EnergyShieldMax)
			{
				EnergyShield = EnergyShieldMax;
			}
		}
	}

	//---------------------------// life //---------------------------//
	public void TakeHit(float dmg)
	{
		Life -= dmg;
		Debug.Log("Player took damage!");
		UpdateHealthUI();

		if (Life <= 0)
		{
			Die();
		}
	}

	public void UpdateHealthUI()
	{
		if (healthBar != null)
		{
			healthBar.value = Life / LifeMax;
		}
		if (healthText != null)
		{
			healthText.text = $"HP: {Mathf.CeilToInt(Life)} / {LifeMax}";
		}
	}
	private void LifeRegenerated()
	{
		Life += Time.deltaTime * LifeRegen;
		if (Life > LifeMax) Life = LifeMax;
		UpdateHealthUI();
	}

	private void Die()
	{
		Debug.Log("Player died!");
	}

	//---------------------------// gold //---------------------------//


	// test gold
	private void PickUpGold()
	{
		Collider[] nearObjects = Physics.OverlapBox(this.transform.position, Vector3.one * 2); // Detect nearby objects

		foreach (Collider col in nearObjects)
		{
			// Ignore the Player object itself
			if (col.gameObject == this.gameObject)
			{
				continue;
			}

			// Check if the object has the "Gold" tag
			if (col.CompareTag("Gold"))
			{
				gold goldScript = col.GetComponent<gold>();
				if (goldScript != null)
				{
					int goldValue = goldScript.Pickup(); // Call the Pickup method
					PlayerGold += goldValue; // Add the gold value to the player's total
					UpdateGoldUI(); // Update the UI to reflect the new gold count
					Debug.Log($"Gold collected! New total: {PlayerGold}");
				}
			}
		}
	}

	private void UpdateGoldUI()
	{
		if (goldText != null)
		{
			goldText.text = $"Gold: {PlayerGold}"; // Update the TextMeshPro UI
		}
	}

	//---------------------------// keys n levers //---------------------------//

	// Check if player has a specific key
	public bool HasKey(int keyId)
	{
		return collectedKeyIds.Contains(keyId);
	}

	// Check if player has activated a specific lever
	public bool HasLeverCode(int leverCode)
	{
		return activatedLevers.Contains(leverCode);
	}

	// Add a key to the player's inventory (for example when they collect it)
	public void AddKey(int keyId)
	{
		if (!collectedKeyIds.Contains(keyId))
		{
			collectedKeyIds.Add(keyId);
		}
	}

	// Activate a lever (for example when the player interacts with it)
	public void ActivateLever(int leverCode)
	{
		if (!activatedLevers.Contains(leverCode))
		{
			activatedLevers.Add(leverCode);
		}
	}


//---------------------------// crystals //---------------------------//


public void TakeCrystal(int val)
	{
		Debug.Log("More crystal");
		PlayerCrystal += val;
	}

	public void ActivateCrystal(int crystalManaValue)
	{
		isCrystalActive = true;
		CrystalMana = crystalManaValue;
		UpdateCrystalManaUI();
	}

	public void DeactivateCrystal()
	{
		isCrystalActive = false;
		CrystalMana = 0;
		UpdateCrystalManaUI();
	}

	// Mana Management
	public void ManaConsumption(float amount)
	{
		if (crystalManager != null && crystalManager.HasCrystalMana(amount))
		{
			crystalManager.UseCrystalMana(amount);
		}
		else
		{
			if (Mana >= amount)
				Mana -= amount;
			else
				Debug.Log("Not enough mana!");
		}
	}

	public void UpdateCrystalManaUI()
	{
		if (crystalManaBar != null && crystalManaText != null)
		{
			crystalManaBar.value = CrystalMana / CrystalManaMax;  // Update the slider value
			crystalManaText.text = $"Crystal Mana: {Mathf.CeilToInt(CrystalMana)} / {Mathf.CeilToInt(CrystalManaMax)}";  // Update the text
		}

		Canvas.ForceUpdateCanvases();  // Forces UI to update immediately (important for UI consistency)
	}



	private void ManageCrystalMana()
	{
		if (isCrystalActive && bCasting && CrystalMana > 0)
		{
			float drainAmount = Time.deltaTime * (ManaMax * ManaRegen / 100);
			CrystalMana -= drainAmount;
			if (CrystalMana < 0) CrystalMana = 0;

			UpdateCrystalManaUI();
		}

		if (isCrystalActive && CrystalMana <= 0)
		{
			DeactivateCrystal();
		}
	}

	public void RefillManaAndCrystal(float refillAmount)
	{
		// Refill regular mana
		Mana += refillAmount;
		if (Mana > ManaMax) Mana = ManaMax;

		// Refill crystal mana if the crystal is active and below max
		if (isCrystalActive)
		{
			CrystalMana = CrystalManaMax;  // Refill crystal to max value
		}

		// Update UI immediately after refilling
		UpdateManaUI();         // Update the regular mana UI
		UpdateCrystalManaUI();  // Update the crystal mana UI (this is crucial)

		Debug.Log($"Refilled Crystal Mana: {CrystalMana}/{CrystalManaMax}");  // Debug message to confirm
	}



	private void ManaRegenerated()
	{
		Mana += Time.deltaTime * (ManaMax * ManaRegen / 100);
		if (Mana > ManaMax) Mana = ManaMax;
		UpdateManaUI();
	}

	public void UpdateManaUI()
	{
		if (manaBar != null)
		{
			manaBar.value = Mana / ManaMax;
		}
		if (manaText != null)
		{
			manaText.text = $"Mana: {Mathf.CeilToInt(Mana)} / {ManaMax}";
		}
	}

	

	
}
