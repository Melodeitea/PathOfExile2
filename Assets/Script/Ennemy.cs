using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ennemy : MonoBehaviour
{
    // Start is called before the first frame update
    float Life = 100;
    public float AddToLife =0;
    public float VisibleLife = 0;
    public bool bAlive = true;
    public int ID ;
    List<rebond> whoImtheTargetOf = new List<rebond>();
    public Material FeedBackMat;
    private Material BaseMat;

    void Start()
    {
        ID = (int)System.DateTime.Now.Ticks;
        BaseMat = this.GetComponent<MeshRenderer>().material;
        Life += AddToLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Takehit(float damage) // substract ennemy hp  and kill it if necessary
    {
        Life -= damage;
        // mat change
        this.GetComponent<MeshRenderer>().material = FeedBackMat;
        foreach (MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = FeedBackMat;
        }

        StartCoroutine(BackToMat());
        VisibleLife = Life;
        
        if(Life <= 0 )
        {
            EndEnnemy();
        }
    }
    IEnumerator BackToMat()
    {
       
        yield return new WaitForSeconds(0.1f);
       
        this.GetComponent<MeshRenderer>().material = BaseMat;
        foreach(MeshRenderer mr in  this.GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = BaseMat;
        }
    }
    private void EndEnnemy() // put life to zero, stop the ennemy and move it to an inaccessible zone
    {
        Life = 0;
        bAlive = false;
        this.transform.position = new Vector3(1000, 1000, 1000);
        foreach (rebond rebond in whoImtheTargetOf) // send a stop at every rebond spell currently targetting this ennemy 
        {
            Debug.Log("Not a viable target anymore : " + rebond);
            rebond.TargetIsOff(this);
        }
    }
    public bool ViableTarget(rebond whoAsk) // return true or false if the enenmy is a viable target for a rebond spell 
    {
        if(bAlive)
        {
            whoImtheTargetOf.Add(whoAsk);
            return true;
        }
        return false;
    }
  

    public void NotTargetAnymore(rebond whoAsk) // send a message to a rebond to stop following this ennemy as a target
    {
        
        whoImtheTargetOf.Remove(whoAsk);
    }
}
