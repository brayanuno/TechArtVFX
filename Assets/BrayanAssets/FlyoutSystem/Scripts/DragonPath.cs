using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DragonPath : MonoBehaviour
{
    static Quaternion dragonSegmentRotationCorrection =
        Quaternion.AngleAxis(90f, Vector3.left);

    static Quaternion dragonHeadRotationCorrection =
        Quaternion.AngleAxis(90f, Vector3.up);

    public enum WaveType
    {
        Sine,
        Cosine,
        Atan
    }

    #region variables

    [Header("FlyoutPrefab")]

    public GameObject dragonBase = null;
    public GameObject dragonSkeletonStart = null;

    [FormerlySerializedAs("pathPos")]
    [Header("Path Variables")]

    [Range(0.0f, 1.0f)]
    public float CurrentPathPos = 0.0f;

    public List<PathNode> dragonIdlePath = new List<PathNode>();

    // ensure curve segment can only be 1 at the lowest
    [Range(1, 32)]
    public int SampleResolution = 16;

    public float nodeSeperation = 0.002f;
    public float speed = 0.05f;
    public AnimationCurve speedCurve;

    public AnimationCurve waveTapering;
    public float amplitude = 1f;
    public float wavelength = 1f;
    public WaveType waveType = WaveType.Sine;

    #endregion

    #region protected variables
    protected float waveTimer = 0.0f;
    protected int segmentCount = 0;
    #endregion

    #region gizmo variables
    [Header("Gizmo Variables")]

    public bool showNodeOrbs = true;

    public bool showInfluencerOrbs = true;

    public bool showInfluencerLines = true;

    #endregion

    protected Vector3 CalculateCubicBezierPoint(float t, PathNode point1, PathNode point2)
    {
        float it = 1.0f - t;

        return
          point1.transform.position * (it * it * it) +
          point2.transform.position * (t * t * t) +
          point1.influencer1.transform.position * (3 * it * it * t) +
          point2.influencer2.transform.position * (3 * it * t * t)
          ;
    }

    /// <summary>
    /// Generates line points for cubic bezier curve.
    /// </summary>
    /// <param name="curveSegments"> number of segments in the curve</param>
    /// <returns></returns>
    protected void GenerateBezierLine(int curveSegments, List<PathNode> path)
    {
        float linePos;
        int secondNode;
        int secondBezierNode;
        int i, j;

        if (path.Count > 1)
        {
            for (i = 0; i < path.Count; ++i)
            {
                secondNode = (i + 1) % path.Count;

                path[i].lineToNextNode = new Vector3[curveSegments];
                path[i].directionToNextBezierNode = new Vector3[curveSegments];
                path[i].magnitudeToNexBezierNode = new float[curveSegments];

                for (j = 0; j < curveSegments; ++j)
                {
                    linePos = j / (float)curveSegments;
                    path[i].lineToNextNode[j] = CalculateCubicBezierPoint(linePos, path[i], path[secondNode]);
                }
            }

            for (i = 0; i < path.Count; ++i)
            {
                for (j = 0; j < curveSegments; ++j)
                {
                    secondNode = i;
                    secondBezierNode = j + 1;

                    if (secondBezierNode >= curveSegments)
                    {
                        secondBezierNode %= curveSegments;
                        secondNode = (i + 1) % path.Count;
                    }

                    path[i].directionToNextBezierNode[j] = path[secondNode].lineToNextNode[secondBezierNode] - path[i].lineToNextNode[j];
                    path[i].magnitudeToNexBezierNode[j] = path[i].directionToNextBezierNode[j].magnitude;
                }
            }
        }
        else
        {
            for (i = 0; i < path.Count; ++i)
            {
                path[i].lineToNextNode = null;
            }
        }
    }

    protected void AlterNodeVisibility(bool show, List<PathNode> path)
    {
        foreach (PathNode node in path)
        {
            node.GetComponent<MeshRenderer>().enabled = show;
        }
    }

    protected void AlterInfluencerVisibility(bool show, List<PathNode> path)
    {
        foreach (PathNode node in path)
        {
            node.influencer1.GetComponent<MeshRenderer>().enabled = show;
            node.influencer2.GetComponent<MeshRenderer>().enabled = show;
        }
    }

    private float PositionDragonPart(Transform transPos, Transform transMesh, Transform transParent, Quaternion rotationCorrection, float pos, float alteredWaveTimer, float segmentPos)
    {
        // get path nodes and bezier nodes
        float posBetweenNodes = (pos * dragonIdlePath.Count) % 1f;
        float posBetweenBezierNodes = (posBetweenNodes * SampleResolution) % 1f;

        int firstPathNode = (int)Mathf.Floor(pos * dragonIdlePath.Count) % dragonIdlePath.Count;
        int firstBezierNode = (int)Mathf.Floor(posBetweenNodes * SampleResolution) % SampleResolution;

        int secondPathNode = firstPathNode;
        int secondBezierNode = (firstBezierNode + 1);

        // keep in bounds
        if (secondBezierNode >= SampleResolution)
        {
            secondBezierNode %= SampleResolution;
            secondPathNode = (firstPathNode + 1) % dragonIdlePath.Count;
        }

        float distance = Mathf.Lerp(
            dragonIdlePath[firstPathNode].magnitudeToNexBezierNode[firstBezierNode],
            dragonIdlePath[secondPathNode].magnitudeToNexBezierNode[secondBezierNode],
            posBetweenBezierNodes
        );

        // determine position
        Vector3 newPosition = Vector3.Lerp(
            dragonIdlePath[firstPathNode].lineToNextNode[firstBezierNode],
            dragonIdlePath[secondPathNode].lineToNextNode[secondBezierNode],
            posBetweenBezierNodes
        );

        // apply wavy effects to position based on position
        float k = 2 * Mathf.PI / wavelength * (alteredWaveTimer - 0.5f);
        float yChange = waveType switch
        {
            WaveType.Sine => amplitude * waveTapering.Evaluate(segmentPos) * Mathf.Sin(k),
            WaveType.Cosine => amplitude * waveTapering.Evaluate(segmentPos) * Mathf.Cos(k),
            WaveType.Atan => amplitude * waveTapering.Evaluate(segmentPos) * Mathf.Atan(k),
        };

        // determine rotation
        Vector3 headingDirection;

        if (transParent != null)
        {
            newPosition += (yChange * transPos.up) * waveTapering.Evaluate(segmentPos); //+ (yChange * forwardVector) for X movement

            headingDirection =
                transParent.position - transPos.position;
        }
        else
        {
            newPosition += (yChange * transPos.up) * -waveTapering.Evaluate(segmentPos); //+ (yChange * forwardVector) for X movement

            headingDirection = Vector3.Lerp(
                dragonIdlePath[firstPathNode].directionToNextBezierNode[firstBezierNode],
                dragonIdlePath[secondPathNode].directionToNextBezierNode[secondBezierNode],
                posBetweenBezierNodes
            );

            headingDirection.y = yChange * -waveTapering.Evaluate(segmentPos) * 2;
        }

        float roll = Mathf.Lerp(
            dragonIdlePath[firstPathNode].roll,
            dragonIdlePath[(firstPathNode + 1) % dragonIdlePath.Count].roll,
            posBetweenNodes
        );

        // rotate visual part
        transMesh.GetChild(1).rotation = Quaternion.LookRotation(headingDirection) * Quaternion.AngleAxis(roll, Vector3.forward) * rotationCorrection;

        transPos.position = newPosition;

        // return the distance value
        return distance;
    }

    private void RecursiveSegmentUpdating(Transform parentBone, float pos, float gap, float alteredWaveTimer, float segmentPos)
    {
        Transform childBone =
            parentBone.transform.childCount > 0 ? parentBone.transform.GetChild(0) : null;

        if (childBone != null)
        {
            pos -= nodeSeperation / gap;

            // loop back over if value goes negative
            while (pos < 0)
            {
                pos += 1f;
            }

            alteredWaveTimer -= nodeSeperation;

            // loop back over if value goes negative
            while (alteredWaveTimer < 0)
            {
                alteredWaveTimer += 1f;
            }

            float distance = PositionDragonPart(parentBone.transform.GetChild(0), parentBone.transform, parentBone, dragonSegmentRotationCorrection, pos, alteredWaveTimer, segmentPos);

            segmentPos += 1f / segmentCount;

            RecursiveSegmentUpdating(childBone, pos, distance, alteredWaveTimer, segmentPos);
        }
    }

    private void Start()
    {
        showNodeOrbs = false;
        showInfluencerOrbs = false;

        // count out segments
        Transform currentSeg = dragonSkeletonStart.transform;

        while (currentSeg.childCount > 0)
        {
            currentSeg = currentSeg.GetChild(0);
            ++segmentCount;
        }

        // generate path nodes
        // for release this will be the only time it is called
        // instead of constantly on Gizmo draw to account for design updates
        GenerateBezierLine(SampleResolution, dragonIdlePath);

        AlterNodeVisibility(showNodeOrbs, dragonIdlePath);
        AlterInfluencerVisibility(showInfluencerOrbs, dragonIdlePath);
    }

    private void Update()
    {
        if (dragonIdlePath.Count > 1)
        {
            float speed = speedCurve.Evaluate(CurrentPathPos) * this.speed;

            waveTimer = (waveTimer + Time.deltaTime * speed) % 1f;

            float distance = PositionDragonPart(dragonSkeletonStart.transform.parent, dragonSkeletonStart.transform, null, dragonHeadRotationCorrection, CurrentPathPos, waveTimer, 0);

            CurrentPathPos = (CurrentPathPos + Time.deltaTime * (speed / distance)) % 1f;

            float alteredWaveTimer = waveTimer - nodeSeperation;

            // loop back over if value goes negative
            while (alteredWaveTimer < 0)
            {
                alteredWaveTimer += 1f;
            }

            RecursiveSegmentUpdating(
                dragonSkeletonStart.transform.GetChild(0), CurrentPathPos, distance, alteredWaveTimer, 1f / segmentCount);
        }
    }

    private void OnValidate()
    {
        AlterNodeVisibility(showNodeOrbs, dragonIdlePath);
        AlterInfluencerVisibility(showInfluencerOrbs, dragonIdlePath);
    }

    private void OnDrawGizmos()
    {
        //foreach (PathNode node in dragonIdlePath)
        for (int i = 0; i < dragonIdlePath.Count; ++i)
        {
            PathNode node = dragonIdlePath[i];
            PathNode node2 = dragonIdlePath[(i + 1) % dragonIdlePath.Count];

            // generate path nodes
            GenerateBezierLine(SampleResolution, dragonIdlePath);

            // draw path
            if (node.lineToNextNode != null)
            {
                Gizmos.color = Color.green;

                for (int j = 0; j < SampleResolution - 1; ++j)
                {
                    Gizmos.DrawLine(
                        node.lineToNextNode[j],
                        node.lineToNextNode[j + 1]
                    );
                }

                Gizmos.DrawLine(
                    node.lineToNextNode[SampleResolution - 1],
                    node2.lineToNextNode[0]
                );
            }

            // draw line to influencers
            if (showInfluencerLines)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(
                    node.transform.position,
                    node.influencer1.transform.position
                );
                Gizmos.DrawLine(
                    node.transform.position,
                    node.influencer2.transform.position
                );
            }
        }

        Gizmos.color = Color.white;
    }
}
