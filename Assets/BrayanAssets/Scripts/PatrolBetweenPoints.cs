using UnityEditor;
using UnityEngine;

public class PatrolBetweenPoints : MonoBehaviour
{
    [Space(5)]
    [SerializeField]
    public Transform[] patrolPoints;

    private void OnDrawGizmos()
    {
        DrawPoints();

        DrawPaths();
    }

    private void DrawPoints()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform point in patrolPoints)
        {

            if (point != null)
                Gizmos.DrawSphere(point.position, .25f);

        }
    }

    private void DrawPaths()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < patrolPoints.Length - 1; i++)
        {
            if (patrolPoints[i] != null && patrolPoints[i + 1] != null)
            {
                Vector3 thisPoint = patrolPoints[i].position;
                Vector3 nextPOint = patrolPoints[i + 1].position;

                Gizmos.DrawLine(thisPoint, nextPOint);
            }

        }
    }
}
