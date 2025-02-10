using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteor : MonoBehaviour
{
    // Start is called before the first frame update
    bool bAlive = false;
   
    Rigidbody Rb;
    Vector3 InitPos;
    Vector3 ImpactPoint;
    float Speed = 50;
    MeshRenderer RbMesh;
    int SpellRadius = 5;
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
    public void Initialize(Vector3 impactpoint, Vector3 spawnpos, SpellData spellinfo) //Initialize the meteor prefab with start position and landing point, and spell complementary informations
    {
        getBaseComponent();
        this.transform.position = spawnpos;
        InitPos = this.transform.position;
        ImpactPoint = impactpoint;
        bAlive = true;
        Rb.linearVelocity = Vector3.Normalize(impactpoint- InitPos) *Speed;
        RbMesh.enabled = true;
        SpellInfo = spellinfo;
        SpellInfo.damage = 50;

    }
    private void EndMeteor() // stop and disable the prefab
    {
        bAlive = false;
        Rb.linearVelocity = Vector3.zero;
        RbMesh.enabled = false;
        transform.position = new Vector3(10000, 10000, 10000);
    }
    // Update is called once per frame

    private void OnCollisionEnter(Collision collision) // detecte ground and all possible ennemys  in range of the point of impact
    {
        if (bAlive )
        {
            Collider[] allennemyhit = Physics.OverlapSphere(this.transform.position, SpellRadius, 1<<LayerMask.NameToLayer("ennemy")) ;
            foreach(Collider collider in allennemyhit)
            {
                collider.gameObject.GetComponent<Ennemy>().Takehit(SpellInfo.damage);
            }
            EndMeteor();

        }
    }
}
