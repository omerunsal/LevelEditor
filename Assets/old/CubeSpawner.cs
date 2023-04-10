using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;
public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float moveSpeed = 5f;

    private GameObject spawned;

    public void SpawnCube()
    {
        spawned = Instantiate(cubePrefab);
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && spawned !=null )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                 Vector3 spawnPosition = hit.point;
                // spawned = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
                // spawned.transform.position = spawnPosition;
                PlayerInfo playerInfo = new PlayerInfo(spawnPosition, spawnPosition, "FirstObject");
                
               
                
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new MyJsonContractResolver()
                };

                var jsonString = JsonConvert.SerializeObject(playerInfo, settings);
                
                // string json = JsonConvert.SerializeObject(playerInfo);
                //
                //
                // using (StreamWriter sw = new StreamWriter("player.json"))
                // {
                //     sw.Write(jsonString);
                // }
                
                using (StreamWriter sw = new StreamWriter(Application.dataPath + "/player.json"))
                {
                    sw.Write(jsonString);
                }
                
                string json = File.ReadAllText("player.json");
                PlayerInfo pInfo = PlayerInfo.FromJson(json);

                SaveScene();
            }
        }
    }
    
    public void SaveScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), "Assets/Scenes/MyNewLevel.unity");

        string srcPath = Application.dataPath + "/Scenes/MyNewLevel.unity";
        string destPath = Application.dataPath + "/../MyNewLevel.unity";
        FileUtil.CopyFileOrDirectory(srcPath, destPath);

        SceneManager.LoadScene("MyNewLevel");
    }
}


