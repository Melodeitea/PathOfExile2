using UnityEditor;
using UnityEngine;


public class Lever : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public bool isUsable = true;
    bool Activated = false;
    int ID = 0;
    public int KeyCode = 0;
    public UnlockableElement UnlockableElement = null;
    void Start()
    {

    }

    public void SetLever(UnlockableElement locker, int id , int keycode)
    {
        UnlockableElement = locker; 
        ID = id;
        KeyCode = keycode;
    }
    public void Use()
    {
        Debug.Log("Try to open : "+this.name);
        if(isUsable && Activated == false)
        {
            isUsable = false;
            Activated = true;
            UnlockableElement.Unlock(ID, KeyCode);
        }
    }
}