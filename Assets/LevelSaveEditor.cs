using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Platform))]
public class LevelSaveEditor : Editor
{
    private int _selectedPrefabIndex = 0;
    private GameObject[] _prefabs;

    private void OnEnable()
    {
        _prefabs = Resources.LoadAll<GameObject>("Prefabs");
    }

    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Seçim kutusunu oluşturun
        _selectedPrefabIndex = EditorGUILayout.Popup("Prefab", _selectedPrefabIndex, GetPrefabNames());

        if (GUILayout.Button("Add Prefab"))
        {
            // Seçilen prefabı sahneye ekleyin
            GameObject prefab = _prefabs[_selectedPrefabIndex];
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.parent = ((Component)target).transform;
        }

        if (GUILayout.Button("Save This Scene As New Level"))
        {
            // UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), "Assets/Scenes/Level.unity");
            
            string[] existingSceneNames = Directory.GetFiles("Assets/Scenes", "*.unity");
            List<string> existingScenes = new List<string>();

            foreach (string sceneName in existingSceneNames)
            {
                existingScenes.Add(Path.GetFileNameWithoutExtension(sceneName));
            }

            // Yeni bir sahne adı oluştur
            int newSceneNumber = 1;
            string newSceneName = "Level";

            while (existingScenes.Contains(newSceneName + newSceneNumber))
            {
                newSceneNumber++;
            }

            newSceneName += newSceneNumber;

            // Yeni sahneyi kaydet
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), "Assets/Scenes/" + newSceneName + ".unity");
        }
        
        if (GUILayout.Button("Reset Editor Scene"))
        {
            
        }
    }

    private string[] GetPrefabNames()
    {
        // Prefab isimlerini bir dizi olarak döndürün
        string[] names = new string[_prefabs.Length];

        for (int i = 0; i < _prefabs.Length; i++)
        {
            names[i] = _prefabs[i].name;
        }

        return names;
    }
}
