using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BatchRename : EditorWindow
{
    private string batchName;
    private string batchStartingNumber;
    private bool showOptions = true;

    [MenuItem("Window/Batch Rename")]
    public static void ShowWindow()
    {
        EditorWindow win = GetWindow(typeof(BatchRename));
        win.maxSize = new Vector2(500, 150);
        win.minSize = win.maxSize;
        GUIContent guiContent = new GUIContent();
        guiContent.text = "Batch Rename";
        win.titleContent = guiContent;
        win.Show();
    }

    private void OnGUI()
    {
        //STEP1
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Step 1: Select Objects in the hierarchy", EditorStyles.boldLabel);

        if (Selection.objects.Length == 0 )
        {
            EditorGUILayout.HelpBox("Please Select a GameObject", MessageType.Warning);
        }
        EditorGUILayout.Space();

        //Renaming
        GUIStyle guiStyle = new GUIStyle(EditorStyles.foldout);
        guiStyle.fontStyle = FontStyle.Bold;
        showOptions = EditorGUILayout.Foldout(showOptions, "Step 2: Enter Rename Info", guiStyle);
        if (showOptions)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("\tEnter name for batch:");
            batchName = EditorGUILayout.TextField(batchName);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("\tEnter Startng Number:");
            batchStartingNumber = EditorGUILayout.TextField(batchStartingNumber);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Step 3: Click the rename button:", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        CreatingButton();

        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
        Repaint();
    }

    void CreatingButton()
    {
        if (GUILayout.Button("Rename Button", GUILayout.ExpandWidth(true), GUILayout.MinWidth(220)))
        {
            int numberAsInt = int.Parse(batchStartingNumber);
            foreach (GameObject obj in Selection.objects)
            {
                obj.name = batchName + "_" + numberAsInt.ToString();
                numberAsInt++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
