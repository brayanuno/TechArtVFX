using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

[CustomPropertyDrawer(typeof(MonsterAbility))]
public class MonsterAbilityDrawer : PropertyDrawer
{
    public SerializedProperty name;
    public SerializedProperty damage;
    public SerializedProperty elementType;

    private float lineHeight = 0;

    //how to draw to the inspector window
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {  
        

        //fill properties
        EditorGUI.BeginProperty(position, label, property);

        
        name = property.FindPropertyRelative("name");
        
        damage = property.FindPropertyRelative("damage");

        elementType = property.FindPropertyRelative("elementType");



       //rect information
       Rect foldOutBox = new Rect(position.min.x,
                            position.min.y, 
                            position.size.x , 
                            EditorGUIUtility.singleLineHeight);

        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);


        if (property.isExpanded)
        {
            //draw our properties
            DrawNameProperty(position);
            DrawDamageProperty(position);
            DrawElementProperty(position);
        }
        //Drawing Instruction here

        EditorGUI.EndProperty();

        
    }

    private void DrawNameProperty(Rect position)
    {
        EditorGUIUtility.labelWidth = 50;
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x * .4f;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);

        EditorGUI.PropertyField(drawArea, name, new GUIContent("Name"));
    }

    private void DrawDamageProperty(Rect position)
    {
        EditorGUIUtility.labelWidth = 70;
        float xPos = position.min.x + (position.width * .5f);
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight ;
        float width = position.size.x * .45f;
        float height = EditorGUIUtility.singleLineHeight;

        Rect drawArea = new Rect(xPos, yPos, width, height);

        EditorGUI.PropertyField(drawArea, damage, new GUIContent("Position"));
    }

    private void DrawElementProperty(Rect position)
    {
        Rect drawArea = new Rect(position.min.x + (position.width * .2f),
            position.min.y + (EditorGUIUtility.singleLineHeight * 2),
            position.size.x * .8f, 
            EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(drawArea, elementType, new GUIContent("ElementType"));
    }




    //updating with more vertical spacing
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        //init Lines
        int totalLines = 1;
        
        //increase our height if we property isExpanded

        if (property.isExpanded)
        {
            totalLines += 3;
        }
        
        return (EditorGUIUtility.singleLineHeight * totalLines);

    }

}
