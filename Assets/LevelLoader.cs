using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public List<GameObject> ObjectGroups = new List<GameObject>();

    private void Awake()
    {
        //TODO: json'da level adıyla veriyi al pozisyonlarda uygun prefab'i üret
        
        
    }

    private void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/level_data.json");
        List<Level> levelList = Level.ListFromJson(json);
        
        Debug.Log("asd");
    }
}
