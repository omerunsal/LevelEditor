using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{
    private GameObject selectedPrefab;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool isPlacingObject = false;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Object Placement", EditorStyles.boldLabel);

        selectedPrefab = EditorGUILayout.ObjectField("Object Prefab", selectedPrefab, typeof(GameObject), false) as GameObject;

        if (selectedPrefab == null)
        {
            EditorGUILayout.HelpBox("Select a prefab to place in the scene.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Place Object"))
        {
            isPlacingObject = true;
        }

        if (isPlacingObject)
        {
            Handles.BeginGUI();
            GUI.Label(new Rect(10, 10, 100, 20), "Left click to place object.");
            Handles.EndGUI();

            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                var aa = Camera.main;
                
                SceneView sceneView = SceneView.currentDrawingSceneView;
                // SceneView LastsceneView = SceneView.lastActiveSceneView;
                // var sceneViews = SceneView.sceneViews;
                
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject obj = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;
                    obj.transform.position = hit.point;
                    spawnedObjects.Add(obj);
                    Selection.activeGameObject = obj;
                }
            }
        }

        if (GUILayout.Button("Clear Objects"))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                DestroyImmediate(obj);
            }
            spawnedObjects.Clear();
        }
    }
}