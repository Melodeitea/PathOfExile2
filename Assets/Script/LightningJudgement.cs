using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningJudgement : MonoBehaviour
{
    Player Activator;
    public bool bActive = false;
    private Vector3 InitPos = new Vector3(1000, 1000, 1000);
    public float SpellMoveSpeed = 1;
    private Vector3 DestinationPos;

    public float TickTime = 1f;
    float LastProckTime = 0;
    public float Damage = 5;
    public float ChargeTime = 1;
    public int MaxCharge = 3;
    private float ChargeCumulated = 0;
    private float BeginningTime;
    private float TimerMana = 0;
    public int Manacost = 30;

    IEnumerator dmgCoroutine = null;

    [Header("Lightning Objects")]
    public GameObject[] StreamLightnings = new GameObject[3]; // Kept original three objects
    Vector3[] StreamBasePos = new Vector3[3];

    [Header("Spell Visuals")]
    public GameObject LightningPrefab; // Lightning effect replacing cylinders
    public ParticleSystem ImpactVFX; // Lightning impact effect

    [Header("Spell Audio")]
    public AudioClip ChargeSound;
    public AudioClip ImpactSound;
    [Range(0f, 1f)] public float SoundVolume = 0.5f; // NEW: Volume control
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Store the original positions of stream objects
        for (int i = 0; i < StreamLightnings.Length; i++)
        {
            StreamBasePos[i] = StreamLightnings[i].transform.localPosition;

            // Disable the default cylinder visuals
            if (StreamLightnings[i].TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                renderer.enabled = false; // Hide the cylinder
            }
        }
    }

    void Update()
    {
        if (bActive)
        {
            MoveAll();
            MoveStream();
            ManaManager();
        }
    }

    private void ManaManager()
    {
        TimerMana += Time.deltaTime;
        if (TimerMana >= ChargeTime)
        {
            TimerMana -= ChargeTime;
            Activator.ManaComsuption(Manacost);
        }
    }

    public void ActivateSpell(Vector3 destinationpos, Player activator)
    {
        Activator = activator;
        DestinationPos = destinationpos;
        transform.position = DestinationPos;
        bActive = true;
        BeginningTime = Time.time;
        TimerMana = 0;

        // Play charge sound
        if (ChargeSound != null) audioSource.PlayOneShot(ChargeSound, SoundVolume);

        // Replace cylinder visuals with LightningPrefab
        for (int i = 0; i < StreamLightnings.Length; i++)
        {
            Instantiate(LightningPrefab, StreamLightnings[i].transform.position, Quaternion.identity, StreamLightnings[i].transform);
        }

        // Damage tick system
        LastProckTime = Time.time;
        DealDamage(Damage);
        StartCoroutine(ProcDamage());
    }

    private IEnumerator ProcDamage()
    {
        yield return new WaitForSeconds(TickTime);
        LastProckTime = Time.time;
        DealDamage(Damage);
        dmgCoroutine = ProcDamage();
        StartCoroutine(dmgCoroutine);
    }

    private void DealDamage(float damage)
    {
        foreach (var enemy in ObjectInTriangle(
                     StreamLightnings[0].transform.position,
                     StreamLightnings[1].transform.position,
                     StreamLightnings[2].transform.position))
        {
            enemy.Takehit(damage);

            // Play impact sound with volume control
            if (ImpactSound != null)
            {
                AudioSource enemyAudio = enemy.GetComponent<AudioSource>();
                if (enemyAudio != null) enemyAudio.PlayOneShot(ImpactSound, SoundVolume);
            }

            // Ensure the impact VFX plays on the enemy
            if (ImpactVFX != null)
            {
                ParticleSystem impactEffect = Instantiate(ImpactVFX, enemy.transform.position, Quaternion.identity);
                impactEffect.Play();
                Destroy(impactEffect.gameObject, impactEffect.main.duration);
            }
        }
    }

    public void DesactiveSpell()
    {
        bActive = false;
        Activator = null;
        DestinationPos = Vector3.zero;
        ChargeCumulated = 0;
        LastProckTime = 0;

        if (dmgCoroutine != null)
        {
            StopCoroutine(dmgCoroutine);
        }

        float RemainingProckDamage = Damage * ((Time.time - LastProckTime) / TickTime);
        DealDamage(RemainingProckDamage);

        transform.position = InitPos;

        // Reset StreamLightnings to original positions
        for (int i = 0; i < StreamLightnings.Length; i++)
        {
            StreamLightnings[i].transform.localPosition = StreamBasePos[i];
        }
    }

    private List<Ennemy> ObjectInTriangle(Vector3 A, Vector3 B, Vector3 C)
    {
        List<Ennemy> EnnemiesPotential = GetComponentInChildren<GetEnnemyInTrigger>().Ennemies;
        List<Ennemy> FinalEnnemies = new List<Ennemy>();

        foreach (var enemy in EnnemiesPotential)
        {
            Vector3 P = enemy.transform.position;
            Vector2 AB = new Vector2(B.x, B.z) - new Vector2(A.x, A.z);
            Vector2 AP = new Vector2(P.x, P.z) - new Vector2(A.x, A.z);

            Vector2 BC = new Vector2(C.x, C.z) - new Vector2(B.x, B.z);
            Vector2 BP = new Vector2(P.x, P.z) - new Vector2(B.x, B.z);

            Vector2 CA = new Vector2(A.x, A.z) - new Vector2(C.x, C.z);
            Vector2 CP = new Vector2(P.x, P.z) - new Vector2(C.x, C.z);

            float ADot = Vector2.Dot(AB, AP);
            float BDot = Vector2.Dot(BC, BP);
            float CDot = Vector2.Dot(CA, CP);

            if (Mathf.Sign(ADot) == Mathf.Sign(BDot) && Mathf.Sign(BDot) == Mathf.Sign(CDot))
            {
                FinalEnnemies.Add(enemy);
            }
        }

        return FinalEnnemies;
    }

    public void MoveStream()
    {
        int i = 0;
        foreach (var stream in StreamLightnings)
        {
            stream.transform.localPosition -= StreamBasePos[i] * Time.deltaTime * 2;
            i++;
            if (Vector3.Magnitude(stream.transform.localPosition) <= 0.25f)
            {
                DesactiveSpell();
            }
        }
    }

    public void MoveAll()
    {
        Vector3 Dir = DestinationPos - transform.position;
        Dir = Vector3.Normalize(Dir);
        transform.position += Dir * Time.deltaTime * SpellMoveSpeed;
    }

    public void ChangeDestinationPos(Vector3 destinationPos)
    {
        this.DestinationPos = destinationPos;
    }

}
