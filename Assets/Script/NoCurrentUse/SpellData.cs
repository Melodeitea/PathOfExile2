using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

using UnityEditor;
using UnityEngine;


[Serializable]
public class SpellData //Utilisable mais soyez prudent
{
    
    public string ID;
   
    public string name;

    public string description;
 
    public float damage;
  
    public string type;
   
    public float manacost;
    
    public float casttime;
   
    public string icon;
    
    public CastType cType;
    public SpellData()
    {

    }
    public SpellData(string infos)
    {
        string[] variables = infos.Split("\t");
        /* foreach(string var in variables)
        {
            Debug.Log(var + "//");
        }*/
        ID = variables[0];
        name = variables[1];
        type = variables[3];
        damage = int.Parse(variables[5]);
        manacost = float.Parse(variables[6]);
        casttime = float.Parse(variables[7]);
        icon = variables[8];
        cType = (CastType)Enum.Parse(typeof(CastType), variables[11]);
        description = variables[13];

    }
    public SpellData(string ID, string name, string description, string damage, string type, string manacost, string casttime, string icon)
    {

    }
    
    

}
public enum CastType
{
    classic,
    charge,
    continu,
}
