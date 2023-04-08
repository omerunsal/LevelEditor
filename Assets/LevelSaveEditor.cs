using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(Platform))]
public class LevelSaveEditor : Editor
{
    private int _selectedPrefabIndex = 0;
    private GameObject[] _prefabs;
    private string _levelName = "";
    private string[] _sceneNames;
    private int _selectedSceneIndex;
    
    private void OnEnable()
    {
        _prefabs = Resources.LoadAll<GameObject>("Prefabs");
        
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        _sceneNames = new string[scenePaths.Length];

        for (int i = 0; i < scenePaths.Length; i++)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(scenePaths[i]);
            _sceneNames[i] = Path.GetFileNameWithoutExtension(scenePath);
        }
    }

    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Object Controls", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        // Seçim kutusunu oluşturun
        _selectedPrefabIndex = EditorGUILayout.Popup("Object Group", _selectedPrefabIndex, GetPrefabNames());
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Add Object Group", GetButtonStyle(Color.green, Color.black)))
        {
            // Seçilen prefabı sahneye ekleyin
            GameObject prefab = _prefabs[_selectedPrefabIndex];
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
            instance.transform.parent = ((Component)target).transform;
        }
          
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        
        // GUILayout.BeginHorizontal();
        // _levelName = EditorGUILayout.TextField("Level Name", _levelName);
        // GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        _selectedSceneIndex = EditorGUILayout.Popup("Scenes", _selectedSceneIndex, _sceneNames);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save This Scene As New Level"))
        {
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
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "Assets/Scenes/" + newSceneName + ".unity");
        }
        
        if (GUILayout.Button("Load Scene"))
        {
            string sceneName = _sceneNames[_selectedSceneIndex];

            // Seçilen sahneyi yükle
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");

            // Seçilen objeleri ekranda seçili olarak göster
            Selection.objects = GameObject.FindObjectsOfType<GameObject>();
        }

        if (GUILayout.Button("Reset Editor Scene"))
        {
            Platform platform = (Platform)target;

            List<Transform> childObjects = new List<Transform>();
            foreach (Transform child in platform.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        GUILayout.EndHorizontal();
    }
    
    private GUIStyle GetButtonStyle(Color backgroundColor, Color textColor)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.background = MakeTex(1, 1, backgroundColor);
        style.normal.textColor = textColor;
        return style;
    }
    
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
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
