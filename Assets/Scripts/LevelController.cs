using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class LevelController : MonoBehaviour
{
    struct Level
    {
        public string levelName;
        public int levelIndex;
        public Level (string levelName, int levelIndex)
        {
            this.levelName = levelName;
            this.levelIndex = levelIndex;
        }
    }
    List<Level> availableLevels = new List<Level>();
    public int currentLevel = 0;
    bool secondDimension = true;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject background;
    [SerializeField] private CinemachineVirtualCamera cam;
    private float switchCoolDown = 0;

    void Start()
    {
        GetLevels();
    }

    void Awake() {
        UpdateCurrentDimension();
    }

    // Update is called once per frame
    void Update()
    {
        HandleChangingDimensions();
    }

    void HandleChangingDimensions()
    {
        // cool down for the swap 
        switchCoolDown -= Time.deltaTime; 
        if (!Input.GetKey(KeyCode.LeftShift) || switchCoolDown > 0)
            return;
        
        UpdateCurrentDimension();
        
        // add cool down
        switchCoolDown = 0.5f;
    }

    void GetLevels()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for( int i = 0; i < sceneCount; i++ )
        {
            string currentSceneName =  System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            Level sceneToLevel = new Level(currentSceneName,i);
            availableLevels.Add(sceneToLevel);
        }

    }
    void ChangeLevel(int newLevelIndex)
    {
        SceneManager.LoadScene(newLevelIndex);
        currentLevel = newLevelIndex;
    }
    public int GetCurrentDimension()
    {
        return secondDimension ? 1 : 0;
    }
    public void UpdateCurrentDimension()
    {
        
        // switch the current dimension
        secondDimension = !secondDimension;
        
        GameObject currentPlayer = secondDimension ? player2 : player1;
        GameObject otherPlayer = secondDimension ? player1 : player2;
        
        // set the cam to follow the current player
        cam.Follow = currentPlayer.GetComponent<Transform>();
        
        // setup the current player
        currentPlayer.GetComponent<PlayerController>().suppersed = false;
        currentPlayer.GetComponent<Renderer>().enabled = true;
        map.GetComponent<Tilemap>().color = secondDimension ? Color.blue : Color.magenta;
        background.GetComponent<Tilemap>().color = secondDimension ? Color.gray : Color.yellow;
        
        // suppress the other player
        otherPlayer.GetComponent<PlayerController>().suppersed = true;
        otherPlayer.GetComponent<Renderer>().enabled = false;
    }
    
}
