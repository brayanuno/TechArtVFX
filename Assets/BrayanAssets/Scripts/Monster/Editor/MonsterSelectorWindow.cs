using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class MonsterSelectorWindow : EditorWindow
{
    private MonsterType selectedMonsterType = MonsterType.None;

    private MonsterType previusMonsterType = MonsterType.None;

    //create a temporary list of valid monster
    List<GameObject> selectableGameObjects = new List<GameObject>();

    private int selectionIndex = 0;

    //click on the window
    [MenuItem("Window/MonsterSelector")]
    public static void ShowWindow()
    {
        GetWindow<MonsterSelectorWindow>("Monster Selector");

    }
    private void OnGUI()
    {
        DisplayGUI();

    }

    void DisplayGUI ()
    {
        EditorGUILayout.Space(10);
        GUILayout.Label("Selection Filter:", EditorStyles.boldLabel);

        selectedMonsterType = (MonsterType)EditorGUILayout.EnumPopup("MonsterType To select: ", selectedMonsterType);

        UpdateSelectableOnChange();

        EditorGUILayout.Space(5);

        if (GUILayout.Button("Select All"))
        {
            SelectAllMonster();
        }

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Cycle Selection:");

        if (GUILayout.Button("Previous"))
        {
            SelectPrevious();
        }
        if (GUILayout.Button("Next"))
        {
            SelectNext();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void UpdateSelectableOnChange()
    {

        //need to update selection
        if (selectedMonsterType != previusMonsterType)
        {
            UpdateSelectable();
            previusMonsterType = selectedMonsterType;
        }
    }

    private void SelectPrevious()
    {
       
    }

    private void SelectNext()
    {
        //CHECK IF LIST IS VALID
        if (selectableGameObjects.Count <= 0)
        {
            return;
        }
        //reset to 0 if we are the last index of selectable GameObject
        if (selectionIndex >= selectableGameObjects.Count - 1)
        {
            selectionIndex = 0;
        }
        else
        {
            selectionIndex++;
        }
        
        //if objects exist , select the object
        if (selectableGameObjects[selectionIndex] != null)
        {
            Selection.activeObject = selectableGameObjects[selectionIndex];
        }

    }
    private void SelectAllMonster()
    {
        
        
        //select all the objects
        Selection.objects = selectableGameObjects.ToArray();

        //create a selection from all valid monster on the scene
        if (selectableGameObjects.Count == 0)
            EditorGUILayout.HelpBox("No Objects Found", MessageType.Warning);


    }

    //look for the objects on the scene and add to the Selectable Game Objects
    private void UpdateSelectable()
    {
        selectableGameObjects.Clear();
        Monster[] monsters = FindObjectsOfType<Monster>();

        //check each monster store if type matches

        foreach (Monster monster in monsters)
        {
            if (monster.Data.MonsterType == selectedMonsterType)
            {
                selectableGameObjects.Add(monster.gameObject);
            }
        }
    }

    private void OnHierarchyChange()
    {
        UpdateSelectable();
    }
}
