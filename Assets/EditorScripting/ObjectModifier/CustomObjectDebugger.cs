using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CustomObjectDebugger : EditorWindow
{
    /*/*
    private GameObject selectedGameObject;
    private List<GameObject> filteredGameObjects = new List<GameObject>();
    
    //Filters
    private string searchQuery = "";
    private string tagFilter = "";
    private string componentFilter = "";
    
    
    private bool showTransform = true;
    
    [MenuItem("Tools/Custom Object Debugger")]
    public static void ShowWindow()
    {
        GetWindow<CustomObjectDebugger>("GameObject Debugger");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    #1#
        
    }

    /*
    private void OnEnable()
    {
        /*filteredGameObjects.AddRange(GameObject.FindObjectsOfType<GameObject>());#2#
    }

    private void OnGUI()
    {#1#
        /*GUILayout.Label("Custom GameObject Debugger", EditorStyles.boldLabel);
        GUILayout.Label("Search & Filter", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Search by Name", searchQuery);
        tagFilter = EditorGUILayout.TextField("Tag", tagFilter);
        componentFilter = EditorGUILayout.TextField("Filter by Component", componentFilter);
        FilteredGameObjects();
        if (filteredGameObjects.Count > 0)
        {
            foreach (GameObject go in filteredGameObjects)
            {
                if (GUILayout.Button(go.name))
                {
                    selectedGameObject = go;
                }
            }
        }
        else
        {
            GUILayout.Label("No GameObjects match the filter criteria", EditorStyles.boldLabel);
        }#1#
    }

    private void FilteredGameObjects()
    {
        /*filteredGameObjects.Clear();
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in allGameObjects)
        {
            bool matchesSearch = string.IsNullOrEmpty(searchQuery) || go.name.ToLower().Contains(searchQuery.ToLower());
            bool matchesTag = string.IsNullOrEmpty(tagFilter) || go.CompareTag(tagFilter);
            bool matchesComponent = string.IsNullOrEmpty(componentFilter) || go.CompareTag(componentFilter);

            if (matchesSearch && matchesTag && matchesComponent)
            {
                filteredGameObjects.Add(go);
            }
        }#1#
    }
    */
    

    // Update is called once per frame
}
