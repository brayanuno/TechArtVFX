using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        
        //get a ref of te attribute
        SeparatorAttribute separatorAttribute = attribute as SeparatorAttribute;

        //Define the line
        Rect separatorRect = new Rect(position.xMin,
                            position.yMin + separatorAttribute.spacing,
                            position.width,
                            separatorAttribute.height);

        //drawing the rect
        EditorGUI.DrawRect(separatorRect,Color.white);
        
    }

    public override float GetHeight()
    {
        SeparatorAttribute separatorAttribute = attribute as SeparatorAttribute;

        float totalSpacing = separatorAttribute.height + separatorAttribute.spacing * 2;

        return totalSpacing ;
    }
}
