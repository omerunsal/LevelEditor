﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR    
[CustomEditor(typeof(Platform))]
public class LevelSaveEditor : Editor
{
    private int _selectedPrefabIndex = 0;
    private GameObject[] _prefabs;
    private string[] _sceneNames;
    private int _selectedSceneIndex;
    
    private int firstCheckpointVal = 0;
    private int secondCheckpointVal = 0;

    string jsonString = "";
    private string resourcePath = "/Resources/level_data.json";

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
        jsonString = File.ReadAllText(Application.dataPath + resourcePath);
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Platform platform = (Platform)target; 
         jsonString = File.ReadAllText(Application.dataPath + resourcePath);
        
        
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Object Controls", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _selectedPrefabIndex = EditorGUILayout.Popup("Object Group", _selectedPrefabIndex, GetPrefabNames());
        EditorGUILayout.EndHorizontal();
        
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Object Group", GetButtonStyle(Color.green, Color.black)))
        {
            GameObject prefab = _prefabs[_selectedPrefabIndex];

            ObjectGroupBuilder objectGroupBuilder = FindObjectOfType<ObjectGroupBuilder>();
            GameObject instance = objectGroupBuilder.BuildProduction(prefab.GetComponent<CollectableGroup>());

            instance.name = prefab.name;
            instance.transform.parent = ((Component)target).transform;
        }
        
        if (GUILayout.Button("Delete Last Added Object Group", GetButtonStyle(Color.red, Color.black)))
        {
            

            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);
                
                DestroyImmediate(lastObjectTransform.gameObject);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("1.Checkpoint Count:");
        int.TryParse(GUILayout.TextField(firstCheckpointVal.ToString()), out firstCheckpointVal);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("2.Checkpoint Count:");
        int.TryParse(GUILayout.TextField(secondCheckpointVal.ToString()), out secondCheckpointVal);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        _selectedSceneIndex = EditorGUILayout.Popup("Scenes (Level 0 means Editor Scene)", _selectedSceneIndex, GetLevelNumbers());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save As New Level Data in JSON"))
        {
            int levelNumber = 1;
           
            if (File.Exists(Application.dataPath + resourcePath))
            {
                 jsonString = File.ReadAllText(Application.dataPath + resourcePath);
               
                List<Level> levels = Level.ListFromJson(jsonString);
                if (levels != null && levels.Count > 0)
                {
                    levelNumber = levels[levels.Count - 1].LevelNumber + 1; 
                }
                
                jsonString = jsonString.TrimEnd(']');
            }

            if (platform.transform.childCount > 0)
            {
                Transform[] childTransforms = new Transform[platform.transform.childCount];

                List<ObjectGroup> ObjectGroupGroupListForThisLevel = new List<ObjectGroup>();


                for (int i = 0; i < platform.transform.childCount; i++)
                {
                    var objectGroup = platform.transform.GetChild(i);

                    ObjectGroup collectableGroup =
                        new ObjectGroup(objectGroup.gameObject.GetComponent<CollectableGroup>().Shape,
                            objectGroup.position,
                            objectGroup.rotation.eulerAngles,
                            objectGroup.gameObject.GetComponent<CollectableGroup>().GroupLayout);
                    ObjectGroupGroupListForThisLevel.Add(collectableGroup);

                    childTransforms[i] = platform.transform.GetChild(i);
                }

                ObjectGroup[] objectGroupArray = ObjectGroupGroupListForThisLevel.ToArray();
                Level level = new Level(levelNumber, objectGroupArray,firstCheckpointVal, secondCheckpointVal);

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new JsonContractResolver()
                };

                string newJsonString = JsonConvert.SerializeObject(level, Formatting.Indented, settings);
                 jsonString += "," + newJsonString + "]";

                File.WriteAllText(Application.dataPath + resourcePath, jsonString);
                
                Debug.Log("Succesfully Added Json file");
            }
        }

        if (GUILayout.Button("Load Selected Scene"))
        {

            if (platform.transform.childCount > 0)
            {
                List<GameObject> childObjects = new List<GameObject>();
                foreach (Transform child in platform.transform)
                {
                    childObjects.Add(child.gameObject);
                }

                childObjects.ForEach(child => DestroyImmediate(child));
                childObjects.Clear();
            }
            
            LoadSelectedLevel();
            Repaint();
        }

        if (GUILayout.Button("Reset Editor Scene"))
        {

            if (platform.transform.childCount > 0)
            {
                List<GameObject> childObjects = new List<GameObject>();
                foreach (Transform child in platform.transform)
                {
                    childObjects.Add(child.gameObject);
                }

                childObjects.ForEach(child => DestroyImmediate(child));
                childObjects.Clear();
            }
            
        }


        GUILayout.EndHorizontal();

       
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Update Selected Level Data in JSON", GetButtonStyle(Color.red, Color.black)))
        {
            if (_selectedSceneIndex != 0 && platform.transform.childCount > 0)
            {
                string json = File.ReadAllText(Application.dataPath + resourcePath);
                List<Level> levels = Level.ListFromJson(json);

                Level selectedLevel = levels[_selectedSceneIndex];

                selectedLevel.FirstCheckpointCount = firstCheckpointVal;
                selectedLevel.SecondCheckpointCount = secondCheckpointVal;
                
                selectedLevel.ObjectGroups = new ObjectGroup[platform.transform.childCount];
                for (int i = 0; i < platform.transform.childCount; i++)
                {
                    var objectGroup = platform.transform.GetChild(i);
                    selectedLevel.ObjectGroups[i] =
                        new ObjectGroup(objectGroup.gameObject.GetComponent<CollectableGroup>().Shape,
                            objectGroup.position,
                            objectGroup.rotation.eulerAngles,
                            objectGroup.gameObject.GetComponent<CollectableGroup>().GroupLayout);
                }

                levels[_selectedSceneIndex] = selectedLevel;

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new JsonContractResolver()
                };
                string updatedJson = JsonConvert.SerializeObject(levels, Formatting.Indented, settings);

                File.WriteAllText(Application.dataPath + resourcePath, updatedJson);
                
                Debug.Log("Succesfully Update Json file");
            }
            else
            {
                Debug.Log("You can't update Scene 0(Editor Scene)");
            }
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("NOTE: Of course, you can rotate and move objects directly from the scene. If you click the Save or Update buttons, it will still write the JSON data for the selected level.", MessageType.Info);

        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Movement Settings", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Label("These buttons are for moving the last added object");
        GUILayout.BeginHorizontal();
        GUILayout.Space(43); 
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("▲", GUILayout.Width(40), GUILayout.Height(40)))
        {
            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.Translate(Vector3.forward);
            }
        }

        GUILayout.Space(43);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("◀", GUILayout.Width(40), GUILayout.Height(40)))
        {
            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.Translate(Vector3.left);
            }
        }

        if (GUILayout.Button("▼", GUILayout.Width(40), GUILayout.Height(40)))
        {
           
            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.Translate(Vector3.back);
            }
        }

        if (GUILayout.Button("▶", GUILayout.Width(40), GUILayout.Height(40)))
        {
            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.Translate(Vector3.right);
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Rotate Settings", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Label("These buttons are for rotating the last added object");
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("↺", GUILayout.Width(40), GUILayout.Height(40)))
        {
            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.rotation *= Quaternion.Euler(0, -30, 0);
            }
        }

        if (GUILayout.Button("↻", GUILayout.Width(40), GUILayout.Height(40)))
        {
            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.localRotation *= Quaternion.Euler(0, 30, 0);
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        
    }

    private void LoadSelectedLevel()
    {
        ObjectGroupBuilder objectGroupBuilder = FindObjectOfType<ObjectGroupBuilder>();
        
        string json = File.ReadAllText(Application.dataPath + resourcePath);
        List<Level> levels = Level.ListFromJson(json);

        Level selectedLevel = levels[_selectedSceneIndex];
        
        for (int i = 0; i < _prefabs.Length; i++)
        {
            foreach (var objectGroup in selectedLevel.ObjectGroups)
            {
                if (objectGroup.GroupLayout == _prefabs[i].GetComponent<CollectableGroup>().GroupLayout && objectGroup.ObjectShape == _prefabs[i].GetComponent<CollectableGroup>().Shape )
                {
                    GameObject instance = objectGroupBuilder.BuildProductionWith(_prefabs[i].GetComponent<CollectableGroup>(), objectGroup.Position,objectGroup.Rotation).gameObject;
                    
                    instance.transform.parent = ((Component)target).transform;
                }
            }
        }
        firstCheckpointVal = selectedLevel.FirstCheckpointCount;
        secondCheckpointVal = selectedLevel.SecondCheckpointCount;
    }


    private GUIStyle GetButtonStyle(Color backgroundColor, Color textColor)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.background = MakeText(1, 1, backgroundColor);
        style.normal.textColor = textColor;
        return style;
    }

    private Texture2D MakeText(int width, int height, Color color)
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
        string[] names = new string[_prefabs.Length];
    
        for (int i = 0; i < _prefabs.Length; i++)
        {
            names[i] = _prefabs[i].name;
        }
    
        return names;
    }
    
    private string[] GetLevelNumbers()
    {
        string json = File.ReadAllText(Application.dataPath + resourcePath);
        List<Level> levels = Level.ListFromJson(json);
    
        string[] prefabNames = new string[levels.Count];
    
        for (int i = 0; i < levels.Count; i++)
        {
            prefabNames[i] = levels[i].LevelNumber.ToString();
        }
    
        return prefabNames;
    }
    
}
#endif
