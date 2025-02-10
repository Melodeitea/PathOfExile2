using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rebond : MonoBehaviour
{
    // Start is called before the first frame update
    bool bAlive = false;
    int nbRebond = 4;
    Rigidbody Rb;
    Vector3 InitPos;
    GameObject Target;
    float Speed = 10;
    int SearchRadius = 10;
    MeshRenderer RbMesh;
    Ennemy EnnemyHit = null;
    SpellData SpellInfo = new SpellData();


    void Start()
    {

    }
    private void getBaseComponent()// get all base component necessary for later interaction
    {
        Rb = GetComponent<Rigidbody>();
        RbMesh = GetComponent<MeshRenderer>();
        RbMesh.enabled = false;
    }
    public void Initialize(GameObject target, Vector3 spawnpos, SpellData spellinfo)//Initialize the rebond prefab with start position and the target ennemy he will track and spell complementary informations
    {
        getBaseComponent();
        this.transform.position = spawnpos;
        InitPos = this.transform.position;
        Target = target;
        bAlive = true;
        RbMesh.enabled = true;
        nbRebond = 4;
        SpellInfo = spellinfo;
        SpellInfo.damage = 20;
    }
    // Update is called once per frame
    void Update()
    {
        if (bAlive)
        {
            if(Target.GetComponent<Ennemy>().bAlive) // verify if the current target is still a vaible target
            {
                Rb.linearVelocity = Vector3.Normalize(Target.transform.position - this.transform.position)*Speed;
            }
            else // if the target is not a viable target anymore stop the rebond spell
            {
                EndRbond();
            }
        }
    }
    private void EndRbond()  // stop and disable the prefab
    {
        bAlive = false;
        Rb.linearVelocity = Vector3.zero;
        RbMesh.enabled = false;
        transform.position = new Vector3(10000, 10000, 10000);
        Destroy(this.gameObject);
    }
    public void TargetIsOff(Ennemy ennemy)//method call by the ennemy when he is not a viable target anymore to cancel targetting of the rebond spell
    {
        if (Target == ennemy)
        {
            EndRbond();
        }
        else
        {
            Debug.Log("Not the current target");
        }
    }
    
    private GameObject newTarget() // search for ennemy in a determine radius and asign the first viable one as a target
    {
        
        Collider[] allennemyhit = Physics.OverlapSphere(this.transform.position, SearchRadius, 1 << LayerMask.NameToLayer("ennemy"));
        foreach (Collider collider in allennemyhit)
        {
            Ennemy en = collider.gameObject.GetComponent<Ennemy>(); //get ennemy script 

            if (en != EnnemyHit && en.ViableTarget(this))// if ennemy has not being hit previously then assigne new target to rebound
            {
                //Debug.Log("new target available : " + en.gameObject.name);
                return en.gameObject;
            }

        }

        return null;// no viable target
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "ennemy" && other.gameObject == Target && bAlive) // if the ennemy touch by the rebond spell is the current target deal damage to it then search for another target if possible 
        {

            if (other.GetComponent<Ennemy>())
            {
                other.GetComponent<Ennemy>().Takehit(SpellInfo.damage);
                EnnemyHit = other.GetComponent<Ennemy>();
                other.GetComponent<Ennemy>().NotTargetAnymore(this);
                nbRebond--;
                if (nbRebond <= 0)
                {
                    EndRbond();
                }
                else
                {
                    Target = newTarget();
                    if(Target == null)
                    {
                        EndRbond();
                    }
                }
            }
            
        }

    }
}
