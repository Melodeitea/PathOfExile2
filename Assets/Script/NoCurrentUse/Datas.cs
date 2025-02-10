using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Datas  // ne pas s'en occuper pour l'instant
{
 
    public static List<SpellData> LoadSpellData () // return the list of all spells informations to the caller
    {
        TextAsset data = Resources.Load<TextAsset>("DATA/SpellsInfos"); // load textasset
       // Debug.Log(data);
       // generateSpellsData(data.text);

        List<SpellData> spellDatas = new List<SpellData>(); //create the list of spells

        string[] spells = data.text.Split("\n"); // explod the chain at each return to line
       
        for(int i=1; i<spells.Length;i++) // fill the list with each spells present in the data
        {
            spellDatas.Add(new SpellData(spells[i]));
        }


        return spellDatas; // reutnr the list
    }
    public static List<Item> ItemData() // same as (LoadSpellData) but for Items
    {
        TextAsset data = Resources.Load<TextAsset>("DATA/ItemInfo");
        Debug.Log(data);
        // generateSpellsData(data.text);

        List<Item> ItemDatas = new List<Item>();

        string[] spells = data.text.Split("\n");
        foreach (string Iteminfo in spells)
        {

            ItemDatas.Add(new Item(Iteminfo));
        }

        return ItemDatas;
    }

}
