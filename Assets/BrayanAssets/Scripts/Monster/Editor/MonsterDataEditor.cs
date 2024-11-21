using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterData))]
public class MonsterDataEditor : Editor
{


    public override void OnInspectorGUI()
    {
        MonsterData data = (MonsterData)target;

        EditorGUILayout.LabelField(data.Name.ToUpper(), EditorStyles.boldLabel);
        EditorGUILayout.Space(10);


        //
        float difficulty = data.Health + data.Damage + data.Speed;

        ProgressBar(difficulty / 100, "DIFFICULTY");


        // Add before
        base.OnInspectorGUI();
        // Add after
        
        if(data.Name == string.Empty)
        {
            EditorGUILayout.HelpBox("Caution:No Name Specified , Please name the monster", MessageType.Warning);
        }

        if (data.MonsterType == MonsterType.None)
        {

        }
    }

    void ProgressBar (float value , string nameLabel)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 32, "TextField");

        EditorGUI.ProgressBar(rect,value, nameLabel);
        EditorGUILayout.Space(12);
    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
