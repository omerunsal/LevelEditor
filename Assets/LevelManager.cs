using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int _currentLevel = 1;

    private const string LEVEL_KEY = "current_level";
    public GameObject[] _prefabs;
    private void Awake()
    {
        _currentLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);

        LoadLevel(_currentLevel);
    }

    public void Fail()
    {
        RestartLevel();
    }

    public void Complete()
    {
        _currentLevel++;
        SaveCurrentLevel();
        LoadLevel(_currentLevel);
    }

    private void LoadLevel(int levelNumber)
    {
        // Load the level with the specified number
        // ...
        
        string json = File.ReadAllText(Application.dataPath + "/level_data.json");
        List<Level> levelList = Level.ListFromJson(json);

        Level level = levelList.Find(l => l.LevelNumber == levelNumber);
        
        ObjectGroupBuilder objectGroupBuilder = FindObjectOfType<ObjectGroupBuilder>();
        
        for (int i = 0; i < _prefabs.Length; i++)
        {
            foreach (var objectGroup in level.ObjectGroups)
            {
                if (objectGroup.GroupLayout == _prefabs[i].GetComponent<CollectableGroup>().GroupLayout && objectGroup.ObjectShape == _prefabs[i].GetComponent<CollectableGroup>().Shape )
                {
                    GameObject instance = objectGroupBuilder.BuildProductionWith(_prefabs[i].GetComponent<CollectableGroup>(), objectGroup.Position,objectGroup.Rotation).gameObject;
                    
                    instance.transform.parent = GameObject.FindWithTag("LevelEnvironments").transform;
                }
            }
        }
       
        // Save the current level in PlayerPrefs
        SaveCurrentLevel();
    }

    private void RestartLevel()
    {
        // Restart the current level
        LoadLevel(_currentLevel);
    }

    private void SaveCurrentLevel()
    {
        // Save the current level in PlayerPrefs
        PlayerPrefs.SetInt(LEVEL_KEY, _currentLevel);
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        _currentLevel = level;
        SaveCurrentLevel();
    }
}
