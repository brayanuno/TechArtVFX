using UnityEngine;
using UnityEditor;
using Codice.CM.Common;
using UnityEditor.Rendering;

[CustomEditor(typeof(MonsterData))]
public class MonsterDataEditor : Editor
{
   
    public SerializedProperty name;
    public SerializedProperty monsterType; 
    public SerializedProperty chanceToDropItem;
    public SerializedProperty rangeOfAwareness;

    public SerializedProperty canEnterCombat;

    public SerializedProperty damage ;
    public SerializedProperty health;
    public SerializedProperty speed;

    public SerializedProperty battlecry;

    public SerializedProperty abilities;

    private void OnEnable()

        
    {
        abilities = serializedObject.FindProperty("abilities");

        name = serializedObject.FindProperty("name");
        monsterType = serializedObject.FindProperty("monsterType"); 
        chanceToDropItem = serializedObject.FindProperty("chanceToDropItem"); 
        rangeOfAwareness = serializedObject.FindProperty("rangeOfAwareness");

        canEnterCombat = serializedObject.FindProperty("canEnterCombat");

        damage = serializedObject.FindProperty("damage"); 
        health = serializedObject.FindProperty("health"); 
        speed = serializedObject.FindProperty("speed"); 

        battlecry = serializedObject.FindProperty("battlecry"); 

        
    }


    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();


        //Title
        EditorGUILayout.LabelField(name.stringValue.ToUpper(), EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        
        float difficulty = health.intValue + damage.intValue + speed.intValue;

        ProgressBar(difficulty / 100, "DIFFICULTY");


        // Redrawing Custom GUI

        EditorGUILayout.LabelField("Gemeral Stats", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(name, new GUIContent("Name"));

        if (name.stringValue == string.Empty)
        {
            EditorGUILayout.HelpBox("Caution:No Name Specified , Please name the monster", MessageType.Warning);
        }

        EditorGUILayout.PropertyField(monsterType, new GUIContent("Monster Type"));

        if (monsterType.GetEnumValue<MonsterType>() == MonsterType.None)
        {
            EditorGUILayout.HelpBox("Please Select A type", MessageType.Warning);
        }

        EditorGUILayout.LabelField("Item Drop Chance:");

        chanceToDropItem.floatValue = EditorGUILayout.Slider(chanceToDropItem.floatValue, 0, 100);

        EditorGUILayout.PropertyField(rangeOfAwareness, new GUIContent("Awareness"));
        EditorGUILayout.PropertyField(canEnterCombat, new GUIContent("Can Enter Combat"));

        if (canEnterCombat.boolValue == true)

        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Combat Stats", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.PropertyField(damage, new GUIContent("Damage"));
            EditorGUILayout.PropertyField(speed, new GUIContent("Speed"));
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(damage, new GUIContent("Health"));

            if (GUILayout.Button("Random Stats"))
            {
                RandomStats();
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.PropertyField(battlecry, new GUIContent("BattleCry"));

        EditorGUILayout.PropertyField(abilities, new GUIContent("Abilities"));

           

        ///*after all change apply modified*/
        serializedObject.ApplyModifiedProperties();

    }

    void ProgressBar (float value , string nameLabel)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 32, "TextField");

        EditorGUI.ProgressBar(rect,value, nameLabel);
        EditorGUILayout.Space(12);
    }

    void RandomStats ()
    {
        damage.intValue = UnityEngine.Random.Range(1, 50);
        speed.intValue = UnityEngine.Random.Range(1, 50);
        health.intValue = UnityEngine.Random.Range(1, 50);
    }




}
