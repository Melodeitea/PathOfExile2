using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class UnlockableElement : MonoBehaviour
{
    public int KeyCode = 0;
    private int  LockNumber = 0;
    private int UnlockNumber = 0;
    [SerializeField]
    public List<Lever> Levers ;
    
    void Start()
    {
        if(Levers.Count <=0)
        {
            Debug.LogWarning(this.name+" has no lever");
        }
    }
    public void AddLock(Lever Iojb)
    {

        if(Levers==null)
        {
            Levers = new List<Lever> ();
        }
        LockNumber++;
        Levers.Add(Iojb);
    }
    public List<Lever> ResetLevers()
    {
        List<Lever> tLevers = Levers;
        Levers = null;
        LockNumber = 0;
        return tLevers;
    }
    public virtual void OpenLock()
    {
        Debug.Log("La porte est ouverte");
    }
    public virtual void CloseLock(int id)
    {

    }
    public virtual void Unlock(int id,int keycode)
    {
        Debug.Log("Index : " + id);
        if(keycode == KeyCode)
        {
            UnlockNumber ++;
        }
        CheckOpening();
    }
    private void CheckOpening ()
    {
        Debug.Log("bop");
        if(UnlockNumber >= LockNumber)
        {
            OpenLock();
        }
    }
   
}
