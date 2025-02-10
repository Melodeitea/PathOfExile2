using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;

using UnityEngine;

public class LightningJudgement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Player Activator;
    public bool bActive = false;
    // 
    private Vector3 InitPos = new Vector3(1000, 1000, 1000);
    public float SpellMoveSpeed = 1;
    private Vector3 DestinationPos;
    //
    public float TickTime = 1f;
    float LastProckTime = 0;
    public float Damage = 5;
    public float ChargeTime = 1;
    public int MaxCharge = 3;
    private int MinCharge = 1;
    private float ChargeCumulated = 0;
    private float BeginningTime;
    private float TimerMana = 0;
    public int Manacost = 5;

    IEnumerator dmgCoroutine = null;
    //
    public GameObject[] StreamLightnings = new GameObject[3];
    Vector3[] StreamBasePos = new Vector3[3];
    public float StreamConvergenceSpeed;
    public float StreamDamage;
    //
    float CS = 0;
    // feedback
    public GameObject ExplosionGO;
   
    void Start()
    {
        for(int i = 0; i < StreamLightnings.Length; i++) // get base pos of streams
        {
            StreamBasePos[i] = StreamLightnings[i].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bActive)
        {
            MoveAll();
            MoveStream();
            ManaManager();

        }
    }
    void ManaManager()
    {
        TimerMana += Time.deltaTime;
        if(TimerMana>= CS)
        {
            TimerMana -= CS;
            Activator.ManaComsuption(Manacost);
        }
    }
    void DealDamage(float damage)
    {
        foreach(var ennemy in ObjectInTriangle(StreamLightnings[0].transform.position, StreamLightnings[1].transform.position, StreamLightnings[2].transform.position))
        {
            ennemy.Takehit(damage);
        }
    }

    private List<Ennemy> ObjectInTriangle(Vector3 A, Vector3 B, Vector3 C)
    {
        List<Ennemy> EnnemiesPotential = this.GetComponentInChildren<GetEnnemyInTrigger>().Ennemies;
        List<Ennemy> FinalEnnemies = new List<Ennemy>(); // crée un tableau de taille equivalente

        foreach(var ennemy in EnnemiesPotential)
        {
            Vector3 P = ennemy.transform.position;
            Vector2 AB = new Vector2(B.x,B.z) - new Vector2(A.x,A.z);
            Vector2 AP = new Vector2(P.x, P.z) - new Vector2(A.x, A.z);

            Vector2 BC = new Vector2(C.x, C.z) - new Vector2(B.x, B.z);
            Vector2 BP = new Vector2(P.x, P.z) - new Vector2(B.x, B.z);

            Vector2 CA = new Vector2(A.x, A.z) - new Vector2(C.x, C.z);
            Vector2 CP = new Vector2(P.x, P.z) - new Vector2(C.x, C.z);

            float ADot = Vector2.Dot(AB, AP);
            float BDot = Vector2.Dot(BC, BP);
            float CDot = Vector2.Dot(CA, CP);

            Mathf.Sign(ADot);
            Mathf.Sign(BDot);
            Mathf.Sign(CDot);

            if (Mathf.Sign(ADot) == Mathf.Sign(BDot) && Mathf.Sign(BDot) == Mathf.Sign(CDot))
            {
                FinalEnnemies.Add(ennemy); 
            }
        }

        return FinalEnnemies;
    }
    IEnumerator ProcDamage()
    {
       
       yield return new WaitForSeconds(TickTime);
       //Debug.Log("Tick : "+Time.time);
       LastProckTime = Time.time;
       DealDamage(Damage);
       dmgCoroutine = ProcDamage();
       StartCoroutine(dmgCoroutine);
    }
    void Explosion()
    {

        // damages ennemmies;
        float temps = Time.time - BeginningTime;
        Debug.Log("Time appearance : " + temps);
        Debug.Log("CS : " + CS);

        ChargeCumulated = (Time.time - BeginningTime )/ CS;
       
        ChargeCumulated = MathF.Floor(ChargeCumulated);
        if(ChargeCumulated > MaxCharge)
        {
            ChargeCumulated = MaxCharge;
        }
        float TotalDamage = Damage * ChargeCumulated * ChargeCumulated;
        float RemainingProckDamage = Damage * ((Time.time - LastProckTime) / TickTime);

        Debug.Log("ChargeCumulated : " + ChargeCumulated);
        Debug.Log("TotalDamage : " + TotalDamage );
        Debug.Log("RemainingProckDamage : " + RemainingProckDamage);
        DealDamage(TotalDamage + RemainingProckDamage);
        Activator.CurrentSpellEnded();

        GameObject GOEXPLO = Instantiate(ExplosionGO);
        GOEXPLO.transform.position = this.transform.position+ new Vector3(0,5f,0);
        GOEXPLO.GetComponent<ParticleSystem>().Play();


        DesactiveSpell();
       



    }

    public void MoveStream()
    {
        int i = 0;
        foreach(var stream in StreamLightnings)
        {
            
            stream.transform.localPosition -= StreamBasePos[i] * Time.deltaTime * StreamConvergenceSpeed;
            i++;
            if(Vector3.Magnitude(stream.transform.localPosition)<=0.25f)
            {
                Explosion();
            }
        }
    }

    public void MoveAll()
    {
        Vector3 Dir = DestinationPos - this.transform.position;
        Dir = Vector3.Normalize(Dir);
        this.transform.position += Dir * Time.deltaTime * SpellMoveSpeed;
    }
    public void ChangeDestinationPos(Vector3 DestinationPos)
    { this.DestinationPos = DestinationPos; }


    
    public void ActivateSpell(Vector3 destinationpos, float castingspeed , Player activator)
    {
        Activator = activator;
        this.DestinationPos = destinationpos;
        this.transform.position = DestinationPos;
        bActive = true;
        BeginningTime = Time.time;
        CS = castingspeed;
        TimerMana = 0;
        //calculateStreamspeed Distance à parcourir / (nombre de charge max /castspeed)
        float distanceToCenter = 2;
        float RatioChargeAndCastspeed = MaxCharge / CS;
        StreamConvergenceSpeed = distanceToCenter / MaxCharge ;// distanceToCenter / RatioChargeAndCastspeed;

         
        foreach (var litghning in StreamLightnings)
        {
            litghning.GetComponent<LigthningStream>().ActivateSpell();
        }
        //
        LastProckTime = Time.time;
        DealDamage(Damage);
        StartCoroutine(ProcDamage());
    }
    public void DesactiveSpell()
    {
        bActive = false;
        Activator = null;
        DestinationPos = Vector3.zero;
        ChargeCumulated = 0;
        LastProckTime = 0;

        if(dmgCoroutine != null)
        {
            StopCoroutine(dmgCoroutine);
        }
        
        float RemainingProckDamage = Damage * ((Time.time - LastProckTime) / TickTime);
        DealDamage(RemainingProckDamage);

        this.transform.position = InitPos;
        for (int i = 0; i < StreamLightnings.Length; i++) // set base pos of streams
        {
             StreamLightnings[i].transform.localPosition = StreamBasePos[i];
        }
        foreach (var litghning in StreamLightnings)
        {
            litghning.GetComponent<LigthningStream>().DesactivateSpell();
        }
    }
}
