using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-200)]
public class GameManager : Singleton<GameManager>
{
    public bool singleSceneForAllLevels;
    public int startLevelCountForLoop;

    public bool isThisLoaderScene;
    public bool sameLevelOnFail;

    public GameObject[] levels;

    private string json;
    private const string level = "level";

    
    public int currentLevel;

    [HideInInspector] public bool isMultiplied;

    public bool isLevelStarted;
    public bool isLevelCompleted;
    public bool isLevelFailed;

    private List<int> levelNumbers;
    private int[] randomLevels;
    private int totalLevelCount;
    public GameObject loaderPanel;

    public int CompletedLevelSectorCount;

    void Awake()
    {
        RandomizeLevels();

        AssignSaveLoadParameters();

        SelectLoadType();
        LoadJsonData();
    }

    private void LoadJsonData()
    {
        json = File.ReadAllText(Application.dataPath + "/level_data.json");
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        isLevelStarted = false;

        isMultiplied = false;
    }


    private void RandomizeLevels()
    {
        // Decide total level count
        if (singleSceneForAllLevels)
        {
            totalLevelCount = levels.Length;
        }
        else
        {
            totalLevelCount = SceneManager.sceneCountInBuildSettings;
        }

        // Limit starting level count for the loop after main levels
        if (totalLevelCount <= startLevelCountForLoop)
        {
            startLevelCountForLoop = Mathf.Clamp(totalLevelCount - 1, 1, 100);
            // Debug.LogError("Start level of the loop can't be the last level");
        }

        // Define necessary lists for randomization
        levelNumbers = new List<int>();

        if (singleSceneForAllLevels)
        {
            for (int i = startLevelCountForLoop; i < levels.Length + 1; i++)
            {
                levelNumbers.Add(i);
            }

            randomLevels = new int[levels.Length - startLevelCountForLoop + 1];
        }
        else
        {
            for (int i = startLevelCountForLoop; i < totalLevelCount + 1; i++)
            {
                levelNumbers.Add(i);
            }

            randomLevels = new int[totalLevelCount - startLevelCountForLoop + 1];
        }

        // Store current state of the RNG
        Random.State originalRandomState = Random.state;

        if (singleSceneForAllLevels)
        {
            // Use a specific seed to get the same outcome every time
            Random.InitState(levels.Length);
        }
        else
        {
            // Use a specific seed to get the same outcome every time
            Random.InitState(totalLevelCount);
        }

        // Randomize level list for after manin levels
        for (int i = 0; i < randomLevels.Length; i++)
        {
            int randomIndex = Random.Range(0, levelNumbers.Count);

            if (levelNumbers.Count > 1)
            {
                while (randomIndex == randomLevels.Length - 1)
                {
                    randomIndex = Random.Range(0, levelNumbers.Count);
                }
            }

            randomLevels[i] = levelNumbers[randomIndex];

            levelNumbers.RemoveAt(randomIndex);
        }

        // Return the RNG to how it was before
        Random.state = originalRandomState;
    }

    private void AssignSaveLoadParameters()
    {
        // Set current level and gem count numbers by loading saved data
        if (!PlayerPrefs.HasKey(level))
        {
            PlayerPrefs.SetInt(level, 1);
        }

        currentLevel = PlayerPrefs.GetInt(level);
    }

    private void SelectLoadType()
    {
        // Decide load method based on level management system
        if (isThisLoaderScene)
        {
            loaderPanel.SetActive(true);
            LevelLoad();
        }
        else if (singleSceneForAllLevels)
        {
            SelectLevel();
        }
    }

    private void LevelLoad()
    {
        string json = File.ReadAllText(Application.dataPath + "/level_data.json");
        List<Level> levelList = Level.ListFromJson(json);

        // Decide which scene to load
        if (currentLevel > levelList.Count)
        {
            currentLevel = randomLevels[(currentLevel - (startLevelCountForLoop - 1) - 1) % randomLevels.Length];
        }

        if (singleSceneForAllLevels && !isThisLoaderScene)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void SelectLevel()
    {
        // Decide which level to load on the same scene
        if (currentLevel > totalLevelCount)
        {
            currentLevel = randomLevels[(currentLevel - (startLevelCountForLoop - 1) - 1) % randomLevels.Length];
        }

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(false);
        }

        levels[currentLevel - 1].SetActive(true);

        AssignSaveLoadParameters();
    }

    public void LevelComplete()
    {
        if (!(isLevelCompleted || isLevelFailed))
        {
            UIManager.instance.LevelCompleteForUI(0.5f);

            List<Level> levelList = Level.ListFromJson(json);
            if (currentLevel < levelList.Count)
            {
                currentLevel++;
            }
            else
            {
                currentLevel = 1;
            }


            Save();
            isLevelCompleted = true;
        }
    }

    public void LevelComplete(float multiplier)
    {
        if (!(isLevelCompleted || isLevelFailed))
        {
            UIManager.instance.LevelCompleteForUI(0.5f);
            currentLevel++;

            isMultiplied = true;
            Save();
            isLevelCompleted = true;
        }
    }

    public void LevelFail()
    {
        if (!(isLevelFailed || isLevelCompleted))
        {
            UIManager.instance.LevelFailForUI(0.5f);
            isLevelFailed = true;
        }
    }


    private void Save()
    {
        PlayerPrefs.SetInt(level, currentLevel);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // if (sameLevelOnFail)
        // {
        //     SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // }
        // else
        // {
        //     LevelLoad();
        // }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // LevelLoad();
    }
}
