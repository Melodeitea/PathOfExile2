using System.Collections;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float SpeedRot = 360 / 10;
    Player Player;
    float PickupTime = 3;
    IEnumerator Cor;
    public int CrystalValue = 100;
    public float TimeProgress = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(this.transform.position,Vector3.up,SpeedRot*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p;
        if(other.TryGetComponent<Player>(out p))
        {
            Player = p;
            Cor = GettingPick(PickupTime);
            TimeProgress = PickupTime;
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
            }
            
        }
    }
    IEnumerator GettingPick(float time)
    {
        TimeProgress -= Time.deltaTime;
        Debug.Log("Progresse : "+ TimeProgress);
        yield return new WaitForSeconds(time);
        Player.TakeCrystal(CrystalValue);
        this.transform.position = new Vector3(1000,1000,1000);
        this.gameObject.SetActive(false) ;

    }
}
