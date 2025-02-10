using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LigthningStream : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //List<Ennemy> ennemies;

    List <OverTimeDmgManager> Ennemies = new List<OverTimeDmgManager>();
    private float TickRate = 0.1f;
    private float Damage = 10;
    private float DPS = 100;
    bool bActive = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bActive == true)
        {
            DamageAll();
        }
            
    }
    public void EmptyAll()
    {
        Ennemies.Clear();
    }
    public void ActivateSpell()
    {
        bActive = true;
    }
    public void DesactivateSpell()
    {
        bActive = false;
        EmptyAll();
    }
    public void ForceTick()
    {
        foreach (var enemy in Ennemies)
        {
            float TimeLastHit = Time.time - enemy.LastTimeDamageTaken; //temps ecoule depuis dernier tick
            float CalculDmg = (TimeLastHit / TickRate) * Damage; // nombre de tick * par degats
            enemy.Ennemy.Takehit(CalculDmg);
            enemy.setTimer(Time.time);
        }
    }
    IEnumerator TickDamage(float NextTickTime)
    {
        yield return new WaitForSeconds(NextTickTime);
        //faire apres le delais
        if(bActive)
        {
            foreach (var enemy in Ennemies)
            {
                float TimeLastHit = Time.time - enemy.LastTimeDamageTaken; //temps ecoule depuis dernier tick
                float CalculDmg = (TimeLastHit / TickRate) * Damage; // nombre de tick * par degats
                enemy.Ennemy.Takehit(CalculDmg);
                enemy.setTimer(Time.time);

            }
            TickDamage(TickRate);
        }
        else
        {
            ForceTick();
        }
        
    }
    //easymethod
    private void DamageAll()
    {
        foreach (var enemy in Ennemies)
        {
            enemy.Ennemy.Takehit(DPS * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Ennemy>())
        {
            Ennemies.Add(new OverTimeDmgManager(other.GetComponent<Ennemy>()));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Ennemy>())
        {
            Ennemies.Remove(new OverTimeDmgManager(other.GetComponent<Ennemy>()));
            
        }
    }
}
public struct OverTimeDmgManager
{
    public Ennemy Ennemy;
    public float LastTimeDamageTaken ;
    public OverTimeDmgManager (Ennemy ennemy)
    {
        Ennemy = ennemy;
        LastTimeDamageTaken = Time.time;
    }

    public void setTimer(float timer)
    {
        LastTimeDamageTaken = timer;
    }
}
