using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    int playersPassed = 0;
    private int keyCount = 0;
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
            if(player.GetKeyCount() > 0) 
            {      
                keyCount+= player.GetKeyCount();
                player.UseKey();
            }

            if(keyCount == 2)
            {
                GameController.Instance.levelController.OpenDoor();
                player.PlayerCompletedLevel();
                playersPassed++;
            }
            else
            {
                GameController.Instance.levelController.ShowFriendBehind();
            }

            if(playersPassed == 2)
            {
                StartCoroutine(Delay(1f));
            }
        }
            
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameController.Instance.levelController.HideFriendBehind();
        }
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        GameController.Instance.levelController.LoadNextLevel();
    }


    }
