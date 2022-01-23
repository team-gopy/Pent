using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
    bool secondDimension = false;
    private float dimensionalSwapWaitTime = 2f;
    private float dimensionalSwapCoolDown = 2f;

    List<DimensionalGates> gatesInLevel = new List<DimensionalGates>();
    List<KeyController> keysInLevel = new List<KeyController>();
    
    //Okkio: Getting the Red and Blue player automatically on awake().
    public GameObject bluePlayer;
    public GameObject redPlayer;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject background;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Light2D globalLight;
    private float switchCoolDown = 0;

    // Colors
    private bool switchingColors = false;
    private Color currentMapColor;
    private Color currentBackgroundColor;

    private Color blueMap = new Color (10f/255f,17f/255f,40f/255f);
    private Color blueBackground = new Color(16f/255f,90f/255f,124f/255f);

    private Color redMap = new Color (72.0f/255.0f,35.0f/255.0f,60.0f/255.0f);
    private Color redBackground = new Color(255f/255f,214f/255f,228f/255f);
    
    private delegate void TodoFunc();

    // this will be shaking status and current shake direction
    // will shake left when it even and otherwise too
    // 0 will be no shake state
    private int shaking;

    void Start()
    {   
        GetPlayers();
        GetLevels();
        // Okkio: Sets the default level state which is always blue's world.
        DefaultLevelState();
        // Okkio: Get all the dimensional gates in the level.
        GetAllGates();
        GetAllKeys();
    }
    
    void Update()
    {
        HandleChangingDimensions();
        HandleDimensionalSwap();
    }

    void HandleChangingDimensions()
    {
        // cool down for the swap 
        switchCoolDown -= Time.deltaTime; 
        // Stops the lerping from running after it is finished.
        if(switchingColors)
        {
            LerpMapColors();
        }
        // Okkio: Shouldn't this be in the PlayerController? also GetKeyDown fixes a bug of holding shift.
        if (!Input.GetKeyDown(KeyCode.LeftShift) || switchCoolDown > 0)
            return;
        
        UpdateCurrentDimension();

        // add cool down
        // Okkio: Made the cool down 1s to match the animation
        switchCoolDown = 1f;
    }
    
    void GetPlayers()
    {
        bluePlayer = GameObject.Find("Blue");
        redPlayer = GameObject.Find("Red");
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
    
    void GetAllGates()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("DimensionalGate"))
        {
            gatesInLevel.Add(obj.GetComponent<DimensionalGates>());
        }
    }
    
    void GetAllKeys()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Key"))
        {
            keysInLevel.Add(obj.GetComponent<KeyController>());
        }
    }
    
    void DefaultLevelState()
    {
        // Enable Blue
        bluePlayer.GetComponent<PlayerController>().suppersed = false;
        bluePlayer.GetComponent<Renderer>().enabled = true;

        cam.Follow = bluePlayer.GetComponent<Transform>();
        map.GetComponent<Tilemap>().color = blueMap;
        background.GetComponent<Tilemap>().color = blueBackground;
        
        // Suppress Red.
        redPlayer.SetActive(false);
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
        // Okkio: Changed the previous supression code to SetActive()
        // switch the current dimension
        secondDimension = !secondDimension;
        
        GameObject currentPlayer = secondDimension ? redPlayer : bluePlayer;
        GameObject otherPlayer = secondDimension ? bluePlayer : redPlayer;

        // set the cam to follow the current player
        cam.Follow = currentPlayer.GetComponent<Transform>();
        
        // setup the current player
        currentPlayer.SetActive(true);
        currentMapColor = secondDimension ? redMap : blueMap;
        currentBackgroundColor = secondDimension ? redBackground : blueBackground;
        switchingColors = true;
        
        // suppress the other player
        bool tmpToSaveDimensionalPadState = otherPlayer.GetComponent<PlayerController>().isOnSwapPad;
        otherPlayer.SetActive(false); 
        otherPlayer.GetComponent<PlayerController>().isOnSwapPad = tmpToSaveDimensionalPadState;
        
        // Okkio: Update dimensional gate collision.
        int currentDimension = GetCurrentDimension();
        foreach(DimensionalGates gate in gatesInLevel)
        {
            gate.UpdateGateCollision(currentDimension);
            gate.UpdateColors(currentDimension);
        }
        foreach(KeyController key in keysInLevel)
        {
            key.UpdateKeys(currentDimension);
        }
        StopAllCoroutines();
        StartCoroutine(Delay(1f, () => switchingColors = false ));
    }

    //  Okkio: Lerping the colors.
    void LerpMapColors()
    {
        map.GetComponent<Tilemap>().color = Color.Lerp(map.GetComponent<Tilemap>().color , currentMapColor,0.01f);
        background.GetComponent<Tilemap>().color = Color.Lerp(background.GetComponent<Tilemap>().color, currentBackgroundColor ,0.01f);
        
    }
    
    // Okkio: Delay to stop the lerp function from running after it finishes.
    // geeko: Delay fucntion will take a function to call after the delay
    IEnumerator Delay(float time, TodoFunc todo)
    {
        yield return new WaitForSeconds(time);
        todo();
    }

    public void FlashEffect(float amount)
    {
        globalLight.intensity += amount;
        StartCoroutine(Delay(0.1f, () => globalLight.intensity = 1f));
    }

    public void ShakeEffect(float time)
    {
        cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;

        StartCoroutine(Delay(time, () =>
        {
            print("dl");
            cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        }));
    }

    private void HandleDimensionalSwap()
    {
        dimensionalSwapCoolDown -= Time.deltaTime;
        
        if(dimensionalSwapCoolDown > 0) return;
        
        bool bluePlayerOnSwapPad = bluePlayer.GetComponent<PlayerController>().isOnSwapPad;
        bool redPlayerOnSwapPad = redPlayer.GetComponent<PlayerController>().isOnSwapPad;

        if (!bluePlayerOnSwapPad || !redPlayerOnSwapPad)
        {
            // print("should swap");
            dimensionalSwapWaitTime = 2f;
            return;
        }

        if (dimensionalSwapWaitTime <= 0)
        {
            ShakeEffect(0.5f);
            StartCoroutine(Delay(0.5f, () =>
            {
                GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
                FlashEffect(0.5f);
                (bluePlayer.transform.position, redPlayer.transform.position) =
                    (redPlayer.transform.position, bluePlayer.transform.position);

                
            }));
            dimensionalSwapWaitTime = 2f;
                            dimensionalSwapCoolDown = 2f;
            return;
        }
        
        dimensionalSwapWaitTime -= Time.deltaTime;
    }
}
