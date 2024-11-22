using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class MonsterSelectorWindow : EditorWindow
{
    private MonsterType selectedMonsterType = MonsterType.None;

    //click on the window
    [MenuItem("Window/MonsterSelector")]
    public static void ShowWindow()
    {
        GetWindow<MonsterSelectorWindow>("Monster Selector");

    }
    private void OnGUI()
    {
        Display();

    }

    void Display ()
    {
        EditorGUILayout.Space(10);
        GUILayout.Label("Selection Filter:", EditorStyles.boldLabel);

        selectedMonsterType = (MonsterType)EditorGUILayout.EnumPopup("MonsterType To select: ", selectedMonsterType);

        EditorGUILayout.Space(5);
        if (GUILayout.Button("Select All")) ;
        {
            FindAllMonster();
        }
    }

    private void FindAllMonster()
    {
        
        //collect all the monster in out scene

        Monster[] monsters = FindObjectsOfType<Monster>();

 
        //create a temporary list of valid monster
        List<GameObject> finalSelection = new List<GameObject>();

        //check each monster store if type matches

        foreach (Monster monster in monsters)
        {
            if (monster.Data.MonsterType == selectedMonsterType)
            {
                finalSelection.Add(monster.gameObject);
            }
        }
        Selection.objects = finalSelection.ToArray();

        //create a selection from all valid monster on the scene
        if (finalSelection.Count == 0)
            EditorGUILayout.HelpBox("No Objects Found", MessageType.Warning);



    }
}
