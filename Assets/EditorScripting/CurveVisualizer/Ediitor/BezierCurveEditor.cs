using UnityEngine;
using UnityEditor;
using Vector2 = System.Numerics.Vector2;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    private void OnSceneGUI()
    {
        BezierCurve bezierCurve = (BezierCurve)target;

        // Make the control points draggable in the scene
        EditorGUI.BeginChangeCheck();
        
        // means its red
        Handles.color = Color.green;
        // Draw the handles for the control points
        Vector3 newPoint0 = bezierCurve.point0.position;
        /*Vector3 newPoint1 = Handles.PositionHandle(bezierCurve.point1.position, Quaternion.identity);*/
        Vector3 newPoint1 = Handles.FreeMoveHandle(bezierCurve.point1.position, .1f, Vector3.zero, Handles.SphereHandleCap);
        
        Vector3 newPoint2 = Handles.FreeMoveHandle(bezierCurve.point2.position, .1f, Vector3.zero, Handles.SphereHandleCap);
        
        Vector3 newPoint3 = bezierCurve.point3.position;
        /*Handles.PositionHandle(bezierCurve.point3.position, Quaternion.identity);*/

        if (EditorGUI.EndChangeCheck()) //If the points have been moved
        {
            Undo.RecordObject(bezierCurve, "Move Bezier Control Points");
            bezierCurve.point0.position = newPoint0;
            bezierCurve.point1.position = newPoint1;
            bezierCurve.point2.position = newPoint2;
            bezierCurve.point3.position = newPoint3;
        }

        // Draw the Bezier curve by calculating and displaying the points
        Handles.color = Color.green;

        int numPoints = 100; // Number of points along the curve
        
        Vector3 previousPoint = bezierCurve.point0.position;

        for (int i = 1; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector3 point = BezierCurve.CalculateBezierPoint(t, bezierCurve.point0.position, bezierCurve.point1.position, bezierCurve.point2.position, bezierCurve.point3.position);
            Handles.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }
}

