using UnityEngine;

public class Console : MonoBehaviour
{
 
    private Player Player;

    void Start()
    {
        GetPlayer();
    }

    private bool GetPlayer()// get Player component
    {
        bool isPlayer = false;
        GameObject player = GameObject.Find("Player");
        if(player != null)
        {
            Player = player.GetComponent<Player>();
            if(Player != null)
            {
                isPlayer = true;
            }
        }
        return isPlayer;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))// self harm player
        {
            if(Player == null)
            {
                if(GetPlayer())
                {
                    Player.TakeHit(20);
                }
            }
            else
            {
                Player.TakeHit(20);
            }
  
        }
        
    }
}
