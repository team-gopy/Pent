using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        GetLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        secondDimension = !secondDimension;
    }
}
