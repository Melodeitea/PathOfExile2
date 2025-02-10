using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    enum ItemSlot
    {
        Pants, Gloves, Weapon, Rings, Amulet, Helmet, Plastron
    }
    public string name;
    public int strength;
    public int intelligence;
    public int dexterity;
    public int manareductionscore;
    public int firedamage;
    public int neutraldamage;
    public int icedamage;
    public int lightningdamage;
    public int Armor;


    public Item(string info)
    {

    }
    
}
