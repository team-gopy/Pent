using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    private int playersPassed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            
            
            if(player.GetKeyCount() > 0 )
            {
                player.PlayerCompletedLevel();
                playersPassed++;
                if(playersPassed == 2)
                {
                   StartCoroutine(Delay(2f));
                }
            }
        }
            
    }
 IEnumerator Delay(float time)
{
    yield return new WaitForSeconds(time);
     GameController.Instance.levelController.LoadNextLevel();
}


}
