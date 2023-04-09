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
    private int selectedChildIndex = 0;


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

        _selectedPrefabIndex = EditorGUILayout.Popup("Object Group", _selectedPrefabIndex, GetPrefabNames());
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add Object Group", GetButtonStyle(Color.green, Color.black)))
        {
            GameObject prefab = _prefabs[_selectedPrefabIndex];
            // GameObject instance = Instantiate(prefab); //TODO: silinebilir.

            ObjectGroupBuilder objectGroupBuilder = FindObjectOfType<ObjectGroupBuilder>();
            GameObject instance = objectGroupBuilder.BuildProduction(prefab.GetComponent<CollectableGroup>());

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
        if (GUILayout.Button("Save As New Level"))
        {
            string[] existingSceneNames = Directory.GetFiles("Assets/Scenes", "*.unity");
            List<string> existingScenes = new List<string>();

            foreach (string sceneName in existingSceneNames)
            {
                existingScenes.Add(Path.GetFileNameWithoutExtension(sceneName));
            }

            int newSceneNumber = 1;
            string newSceneName = "Level";

            while (existingScenes.Contains(newSceneName + newSceneNumber))
            {
                newSceneNumber++;
            }

            newSceneName += newSceneNumber;

            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "Assets/Scenes/" + newSceneName + ".unity");
        }

        if (GUILayout.Button("Load Scene"))
        {
            string sceneName = _sceneNames[_selectedSceneIndex];

            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");

            Selection.objects = GameObject.FindObjectsOfType<GameObject>();
        }

        if (GUILayout.Button("Reset Editor Scene"))
        {
            Platform platform = (Platform)target;

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
        if (GUILayout.Button("Update the selected scene"))
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName != "EditorScene")
            {
                string scenePath = "Assets/Scenes/" + sceneName + ".unity";
                bool sceneExists = File.Exists(scenePath);

                if (sceneExists)
                {
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
                }
                else
                {
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
                }
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Movement Settings", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Label("These buttons are for moving the last added object");
        GUILayout.BeginHorizontal();
        GUILayout.Space(43); // add some space before the buttons to center them
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("▲", GUILayout.Width(40), GUILayout.Height(40)))
        {
            Platform platform = (Platform)target;

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
            Platform platform = (Platform)target;

            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.Translate(Vector3.left);
            }
        }

        if (GUILayout.Button("▼", GUILayout.Width(40), GUILayout.Height(40)))
        {
            Platform platform = (Platform)target;

            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.Translate(Vector3.back);
            }
        }

        if (GUILayout.Button("▶", GUILayout.Width(40), GUILayout.Height(40)))
        {
            Platform platform = (Platform)target;

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
            Platform platform = (Platform)target;

            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.rotation *= Quaternion.Euler(0, -30, 0);
            }
        }

        if (GUILayout.Button("↻", GUILayout.Width(40), GUILayout.Height(40)))
        {
            Platform platform = (Platform)target;

            if (platform.transform.childCount > 0)
            {
                Transform lastObjectTransform = platform.transform.GetChild(platform.transform.childCount - 1);

                lastObjectTransform.localRotation *= Quaternion.Euler(0, 30, 0);
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Save Level Data to JSON", GetButtonStyle(Color.blue, Color.white)))
        {
            Platform platform = (Platform)target;


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

                Level level = new Level(1, ObjectGroupGroupListForThisLevel); //TODO: level sayısı kontrol edilmeli

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new MyJsonContractResolver()
                };

                var jsonString = JsonConvert.SerializeObject(level, settings);
                Debug.Log(jsonString);

                File.WriteAllText(Application.dataPath + "/level_data.json", jsonString);
            }
        }
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
        string[] names = new string[_prefabs.Length];

        for (int i = 0; i < _prefabs.Length; i++)
        {
            names[i] = _prefabs[i].name;
        }

        return names;
    }
}