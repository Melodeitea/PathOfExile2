using System.Collections.Generic;
using UnityEngine;

public class GetEnnemyInTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Ennemy> Ennemies = new List<Ennemy>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ennemy>())
        {
            Ennemies.Add(other.GetComponent<Ennemy>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Ennemy>())
        {
            Ennemies.Remove(other.GetComponent<Ennemy>());

        }
    }
}
