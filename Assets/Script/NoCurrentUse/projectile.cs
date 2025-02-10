using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    // Start is called before the first frame update
    bool bAlive = false;
    bool bPierce = true;
    Rigidbody Rb;
    Vector3 InitPos;
    float MaxDistance = 15;
    Vector3 Direction;
    float Speed = 10;
    MeshRenderer RbMesh;
    List<int> IDEnnemyHit = new List<int>();
    SpellData SpellInfo = new SpellData();



    private void getBaseComponent()// get all base component necessary for later interaction
    {
        Rb = GetComponent<Rigidbody>();
        RbMesh = GetComponent<MeshRenderer>();
        RbMesh.enabled = false;
    }
    public void Initialize(Vector3 direction, Vector3 spawnpos, SpellData spellinfo) // //Initialize the projectile prefab with start position, direction and spell complementary informations
    {
        getBaseComponent();
        this.transform.position = spawnpos;
        InitPos = this.transform.position;
        Direction = direction;
        bAlive = true;
        Rb.linearVelocity = Direction * Speed;
        RbMesh.enabled=true;    
       
        SpellInfo = spellinfo;
        SpellInfo.damage = 20;
    }
    // Update is called once per frame
    void Update()
    {
        if (bAlive)
        {
            if (Vector3.Distance(InitPos, transform.position) >= MaxDistance) // verify if the max rang of the projectile is exceeded if true stop the projectile
            {
                EndProj();
            }
        }
        
    }
    private void EndProj()  // stop and disable the prefab
    {
        bAlive = false;
        Rb.linearVelocity = Vector3.zero;
        RbMesh.enabled = false;
        transform.position = new Vector3(10000, 10000, 10000);
        Destroy(this.gameObject);
    }
    public void OnTriggerEnter(Collider other) // when a potential target enter the trigger area check the quality of the object
    {
        
       if( other.tag == "ennemy") // if it's an ennemy
       {
            Debug.Log("ennemyhit : "+ other.GetComponent<Ennemy>().ID);
 
            
            if (IDEnnemyHit.Count == 0 || IDEnnemyHit.Contains(other.GetComponent<Ennemy>().ID)!) // verify if it's not the same ennemy as previously touch if it isn't deal dmg to it
            {
                Debug.Log("ennemytarget");
                other.GetComponent<Ennemy>().Takehit(SpellInfo.damage);
                IDEnnemyHit.Add(other.GetComponent<Ennemy>().ID);

            }
            if (bPierce != true) // if the projectile doesn't have the property to pierce, destroy the projectile
            {
                Destroy(this.gameObject);
            }
        }
       
    }
}
