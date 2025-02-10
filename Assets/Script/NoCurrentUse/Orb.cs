using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    bool bAlive = false;

    Rigidbody Rb;
    Vector3 InitPos;
    MeshRenderer RbMesh;
    SpellData SpellInfo = new SpellData();
    float TickTime = 0.25f;
    float previousTick  =0;
    float Duration = 5;
    float InitTime = 0;
    int SpellRadius = 3;
    float BaseHeight = 1.5f;
    void Start()
    {
        
    }
    private void getBaseComponent()// get all base component necessary for later interaction
    {
        
        RbMesh = GetComponent<MeshRenderer>();
       
    }
    public void Initialize(Vector3 spawnpos, SpellData spellinfo) //Initialize the meteor prefab with start position and landing point, and spell complementary informations
    {
        getBaseComponent();
        spawnpos.y = BaseHeight;
        this.transform.position = spawnpos;
        InitPos = this.transform.position;
        bAlive = true;
        RbMesh.enabled = true;
        SpellInfo = spellinfo;
        SpellInfo.damage = 50;
        InitTime = Time.time;

    }
    private void EndOrb() // stop and disable the prefab
    {
        bAlive = false;
        RbMesh.enabled = false;
        transform.position = new Vector3(10000, 10000, 10000);
    }
    // Update is called once per frame
    void Update()
    {
        if(bAlive)
        {
            if(Time.time<previousTick+TickTime)
            {
                previousTick = Time.time;
            }
            if(Time.time > InitTime + Duration)
            {
                EndOrb();
            }
        }
    }
    private void HitEnnemysInArea()
    {
        Collider[] allennemyhit = Physics.OverlapSphere(this.transform.position, SpellRadius, 1 << LayerMask.NameToLayer("ennemy"));
        foreach (Collider collider in allennemyhit)
        {
            collider.gameObject.GetComponent<Ennemy>().Takehit(SpellInfo.damage);
        }
       
    }
    
}
