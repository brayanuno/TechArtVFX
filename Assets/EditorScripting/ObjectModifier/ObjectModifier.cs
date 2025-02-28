using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectModifier : MonoBehaviour
{
    /*private static EditorWindow window;

    public GameObject[] objectToReplace = null;
    public GameObject ObjectToReplace = null;
    
    private List<Transform> selectedObjectTransforms = new List<Transform>();
    private List<GameObject> selectedGameObjects = new List<GameObject>();
    private List<Object> selectedFolderFromProject  = new List<Object>();
    private List<string> selectedFolderPathTable = new List<string>();
    private List<string> tagTable = new List<string>();
    private List<string> labelTable = new List<string>();
    
    private List<int> modifiedObjectIndices = new List<int>();
    
    private Dictionary<string, string> objectInfoTable = new Dictionary<string, string>();  
    private Dictionary<int, List<GameObject>> groupedObjectsTable = new Dictionary<int, List<GameObject>>();

    private bool[] showFoldoutTable = null;
    private bool[] resetTransformTable = null;
    private bool[] mainTransformTable = null;
    private bool[] maintainHierachyTable = null;
    private bool[] dialogForNoObjectDisplay = null;
    
    private bool permissionToModifyGive = false;

    private Vector3[] offsetPosTable = null;
    private Vector3[] offsetRotTable = null;
    private Vector3[] offsetScaleTable = null;

    private string[] changedTagTable = null;
    private string[] searchByOptions = new string[] { "Tag", "Name" };
    private string[] searchFromOptions = new string[] { "Scene", "Project" };
    
    private Vector2 scrollView = Vector2.zero;
    private string message = "";
    private string searchString = "";
    
    private int numberOfItemsToGroup;
    private int toSearchBy = 0;
    private int toSearchFrom = 0;
    private int currentGroupCount = 0;
    private int numberOfObjectsModified = 0;
    private int originalObjectsReplaceCount = 0;
    
    Vector3 offsetPos = Vector3.zero;
    Vector3 offsetROt = Vector3.zero;
    Vector3 offsetScale = Vector3.zero;
    
    private bool selectedProjectSearch = false;
    private bool showLabel = false;
    private bool foldersFromProjectAdded = false;
    private bool searchInvoked = false;
    private bool objectsGrouped = false;
    private bool hasReplacementPrefab = true;
    private bool folderFromProjectAdded = false;
    private bool maintainTransforms = false;
    private bool resetTransforms = false;
    private bool maintainHierarchy = true;
    private bool changePos = false;
    private bool changeRot = false;
    private bool changeScale = false;
    private bool continueWithChangees = false;
    
    
    [MenuItem("Tools/ObjectModifier", false, 10)]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static void Init()
    {
        /*window = GetWindow(typeof(ObjectModifier), false, "Object Modifier");#1#
        window.position = new Rect(100f, 100f, 100f, 100f);
        window.minSize = new Vector2(600f, 600f);
        window.Show();
        Selection.activeObject = null;
    }
    
  

    private void OnGUI()
    {
        GUILayout.Space(13f);

        if (showLabel)
            GUI.enabled = false;

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Search using:", EditorStyles.boldLabel);
            GUILayout.Space(-100f);
            toSearchBy = EditorGUILayout.Popup(toSearchBy,searchByOptions);
            GUILayout.Space(-2f);
            EditorGUILayout.LabelField("From: ", EditorStyles.boldLabel);
            GUILayout.Space(-170f);
            toSearchFrom = EditorGUILayout.Popup(toSearchFrom, searchFromOptions);

            if (toSearchFrom ==1 && !selectedProjectSearch && !foldersFromProjectAdded)
            {
               
                /*if (EditorUtility.DisplayDialog("Select folder(s)", "Select one or more folder from Panel to search From", "Select Folders"))
                {
                    message = "Select one or more folders from the Project Panel to search from.";
                }#1#
                

                Selection.activeObject = null;
                showLabel = false;
                searchInvoked = false;
                selectedFolderFromProject.Clear();
                selectedGameObjects.ToList().Clear();
                selectedObjectTransforms.Clear();
                objectsGrouped = false;
            }
            
            else if (toSearchFrom == 0 && selectedProjectSearch)
            {
                message = "No Object Selected";
                Selection.activeObject = null;
                selectedProjectSearch = false;
                selectedFolderFromProject.Clear();
                ClearSelection(true);
            }
        }

        if (selectedProjectSearch)
        {
            string buttonString = foldersFromProjectAdded ? "Remove" : "Add" + selectedFolderFromProject + " items.";
            if (!showLabel)
                GUI.enabled = true;
            if (selectedFolderFromProject.Count < 1)
            {
                buttonString = "Select Folders";
                GUI.enabled = false;
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void ClearSelection(bool clearSelectedObjects = true, bool fromProject = false)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
