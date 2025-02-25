using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrystalManager : MonoBehaviour
{
    public float currentCrystalMana = 0f;
    public float maxCrystalMana = 100f;
    public Slider crystalManaSlider;
    public TextMeshProUGUI crystalManaText;

    private bool hasCrystal = false;
    private GameObject currentCrystal;

    private void Start()
    {
        crystalManaSlider.gameObject.SetActive(false);  // Initially hide slider
    }

    private void Update()
    {
        if (hasCrystal)
        {
            crystalManaSlider.gameObject.SetActive(true);
            crystalManaSlider.value = currentCrystalMana / maxCrystalMana;
            crystalManaText.text = $"{currentCrystalMana}/{maxCrystalMana}";
        }
    }

    public void AddCrystalMana(float mana, GameObject newCrystal)
    {
        if (hasCrystal) DropCrystal();  // Drop any existing crystal
        hasCrystal = true;

        currentCrystalMana = mana;
        maxCrystalMana = mana;
        currentCrystal = newCrystal;

        UpdateCrystalUI();
        PlayCollectionSound(newCrystal);
    }

    private void DropCrystal()
    {
        if (currentCrystal == null) return;

        Vector3 dropPosition = transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        currentCrystal.transform.position = dropPosition;
        currentCrystal.SetActive(true);
    }

    public void ResetCrystalMana()
    {
        currentCrystalMana = 0f;
        hasCrystal = false;
        crystalManaSlider.gameObject.SetActive(false);
        DropCrystal();
    }

    public bool UseCrystalMana(float amount)
    {
        if (currentCrystalMana >= amount)
        {
            currentCrystalMana -= amount;
            UpdateCrystalUI();
            return true;
        }
        return false;
    }

    public bool HasCrystalMana(float amount) => currentCrystalMana >= amount;

    public void UpdateCrystalUI()
    {
        if (crystalManaSlider == null || crystalManaText == null) return;

        crystalManaSlider.value = currentCrystalMana / maxCrystalMana;
        crystalManaText.text = $"{Mathf.CeilToInt(currentCrystalMana)} / {maxCrystalMana}";
    }

    private void PlayCollectionSound(GameObject newCrystal)
    {
        var crystalComponent = newCrystal.GetComponent<Crystal>();
        if (crystalComponent?.collectSound != null)
        {
            AudioSource.PlayClipAtPoint(crystalComponent.collectSound, newCrystal.transform.position);
        }
    }
}
