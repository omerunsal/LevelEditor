using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

 [DefaultExecutionOrder(-100)]
public class LevelGenerator : Singleton<LevelGenerator>
{
    public GameObject[] _prefabs;
    public GameObject[] _checkpoints;

    private void Awake()
    {
        GenerateLevel(GameManager.Instance.currentLevel);
    }

    private void GenerateLevel(int levelNumber)
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/level_data.json");
        List<Level> levelList = Level.ListFromJson(json);
        
        if (levelList.Count <= GameManager.Instance.currentLevel)
        {
            GameManager.Instance.currentLevel = 1;
            levelNumber = 1;
        }

        Level level = levelList.Find(l => l.LevelNumber == levelNumber);

        ObjectGroupBuilder objectGroupBuilder = FindObjectOfType<ObjectGroupBuilder>();

        for (int i = 0; i < _prefabs.Length; i++)
        {
            foreach (var objectGroup in level.ObjectGroups)
            {
                if (objectGroup.GroupLayout == _prefabs[i].GetComponent<CollectableGroup>().GroupLayout &&
                    objectGroup.ObjectShape == _prefabs[i].GetComponent<CollectableGroup>().Shape)
                {
                    GameObject instance = objectGroupBuilder
                        .BuildProductionWith(_prefabs[i].GetComponent<CollectableGroup>(), objectGroup.Position,
                            objectGroup.Rotation).gameObject;

                    instance.transform.parent = GameObject.FindWithTag("LevelEnvironments").transform;
                }
            }
        }

        _checkpoints[0].GetComponent<CheckpointController>().CheckpointCount = level.FirstCheckpointCount;
        _checkpoints[1].GetComponent<CheckpointController>().CheckpointCount = level.SecondCheckpointCount;


    }
}