using UnityEngine;

public class gold : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int goldvalue = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGoldValue(int value)
    {
        goldvalue = value;
    }
    public int GetGoldValue()
    {
        return goldvalue;
    }
    public int Pickup()
    {
        this.transform.position = new Vector3(1000, 1000, 1000);
        return goldvalue;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Onenter");
        if(other.tag == "Player")
        {

        }
    }
}
