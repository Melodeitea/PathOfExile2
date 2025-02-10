using System.Collections;
using UnityEngine;

public class trap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Player Player;
    IEnumerator Cor;
    int Damage = 50;
    float LaunchTime = 1;
    private void OnTriggerEnter(Collider other)
    {
        Player p;
        if (other.TryGetComponent<Player>(out p) )
        {
            Player = p;
            Cor = Launch(LaunchTime);
            StartCoroutine(Cor);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player p;
        if (other.TryGetComponent<Player>(out p))
        {
            if (p == Player)
            {
                StopCoroutine(Cor);
                Player = null;
            }

        }
    }
    IEnumerator Launch(float time)
    {
   
        Debug.Log("Prepare to launch");
        yield return new WaitForSeconds(time);
        Player.TakeHit(Damage);
        if(Player != null)
        {
            Cor = Launch(LaunchTime);
            StartCoroutine(Cor);
        }
    }
}
