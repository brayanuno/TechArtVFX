// ***********************************************************************
//                                NOTICE
//
//      THIS DOCUMENT REPRESENTS CONFIDENTIAL PROPRIETARY PROGRAM
//      PRODUCTS AND PROPRIETARY INFORMATION AND COPYRIGHTABLE MATERIAL
//      OWNED BY Light & Wonder, Inc. AND ITS SUBSIDIARIES (COLLECTIVELY,
//      "Light & Wonder").
//
//      NEITHER RECEIPT NOR POSSESSION OF THIS DOCUMENT CONFERS ANY RIGHT
//      TO REPRODUCE, COPY, PREPARE DERIVATIVE WORKS, USE, OR DISCLOSE,
//      IN WHOLE OR IN PART, ANY PROGRAM, PRODUCT OR INFORMATION CONTAINED
//      HEREIN WITHOUT WRITTEN AUTHORIZATION FROM Light & Wonder.
//
//      COPYRIGHT Light & Wonder, Inc. AND ITS SUBSIDIARIES.
//      ALL RIGHTS RESERVED.
//
// ***********************************************************************

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class FlyoutPath : MonoBehaviour
{
    /// <summary>
    /// FlyUp Object.
    /// </summary>
    [Header("Flyout Properties:")]
    public GameObject flyoutObject; // CurrenFlyout Object
    
    /// <summary>
    /// Target Object.
    /// </summary>
    public GameObject target; // Current Target Object
    
    /// <summary>
    /// Track position on the entire path (normalized)
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float T = 0f;
    
    /// <summary>
    /// List of Path Nodes 
    /// </summary>
    public List<PathNode> pathNodes = new List<PathNode>(); 
    
    /// <summary>
    ///Curve smoothness
    /// </summary>
    [FormerlySerializedAs("resolution")] public int CurveResolution = 50; 
    
    /// <summary>
    ///Speed curve affecting the entire path
    /// </summary>
    public AnimationCurve speedCurve;
    
    /// <summary>
    ///Movement Speed
    /// </summary>
    public float speed = 5f; 
    
    /// <summary>
    /// Loop Conditional
    /// </summary>
    public bool loop = false;
    
    /// <summary>
    /// Either destroy the gameObject or not
    /// </summary>
    public bool destroyOnImpact;
    
    /// <summary>
    /// DestroyObject Slight Delay
    /// </summary>
    public float destroyDelay = .07f;
    
    /// <summary>
    /// Either destroy the gameObject or not
    /// </summary>
    private bool stopMovement = false;

    #region Waves
    
    /// <summary>
    /// Wave Movement Type
    /// </summary>
    public enum WaveType
    {
        Sine,
        Cosine,
        Atan
    }
    
    /// <summary>
    /// Wave Movement Properties 
    /// </summary>
    [Header("Waves:")]
    public AnimationCurve waveTapering;
    public float amplitude = 1f;
    public float wavelength = 1f;
    public WaveType waveType = WaveType.Sine;
    #endregion
    
    #region gizmo variables
    /// <summary>
    /// Gizmo Variables 
    /// </summary>
   [Header("Gizmo Variables:")]

    public bool showNodeOrbs = true;

    public bool showInfluencerOrbs = true;

    public bool showInfluencerLines = true;

    #endregion

    /// <summary>
    /// calculates a point on a cubic Bézier curve for a given segment index and parameter 
    /// </summary>
    /// <param name="t"> position along the Bézier curve segment. </param>
    /// <param name="segmentIndex"> Index of the segment in the pathNodes list. </param>
    /// <returns> Vector3 position, which represents a point in 3D space. T </returns>
    public Vector3 GetPoint(float t, int segmentIndex)
    {
        // Check if the segment index is valid
        if (segmentIndex < 0 || segmentIndex >= pathNodes.Count - 1)
        {
           
            return Vector3.zero;
        }
        
        // Get the path nodes for the specified segment
        PathNode p0 = pathNodes[segmentIndex];
        PathNode p1 = pathNodes[segmentIndex + 1];

        // Get points for the segment
        Vector3 startPoint = p0.transform.position;
        Vector3 handle1 = p0.handle2 != null ? p0.handle2.position : startPoint;
        Vector3 handle2 = p1.handle1 != null ? p1.handle1.position : p1.transform.position;
        Vector3 endPoint = p1.transform.position;
        
        // Calculate the point on the cubic Bézier curve using the formula
        return Mathf.Pow(1 - t, 3) * startPoint +
               3 * Mathf.Pow(1 - t, 2) * t * handle1 +
               3 * (1 - t) * Mathf.Pow(t, 2) * handle2 +
               Mathf.Pow(t, 3) * endPoint;
    }
    
    // <summary>
    /// a wave offset based on the specified wave type (sine, cosine, or arctangent).
    /// </summary>
    /// <param name="t"> Wave Segment </param>
    /// <returns> Wave offset T </returns>
    private Vector3 GetWaveOffset(float t)
    {
        float waveValue = 0f;
        // Calculate the tapered amplitude using the wave tapering curve value
        float taperedAmplitude = amplitude * waveTapering.Evaluate(t);
        
        // Determine the wave value based on the wave type
        switch (waveType)
        {
            case WaveType.Sine:
                waveValue = Mathf.Sin(t * Mathf.PI * 2 / wavelength);
                break;
            case WaveType.Cosine:
                waveValue = Mathf.Cos(t * Mathf.PI * 2 / wavelength);
                break;
            case WaveType.Atan:
                waveValue = Mathf.Atan(t * Mathf.PI * 2 / wavelength);
                break;
        }
        // Return the wave offset as a Vector3, adjusting the Y-axis value
        return new Vector3(0, waveValue * taperedAmplitude, 0); 
    }
    
    /// <summary>
    /// Calculates the total length of a path by summing the distances between consecutive nodes in the pathNode 
    /// </summary>
    /// <returns>  total length of the path T </returns>
    public float GetTotalPathLength()
    {
        // Initialize total length to zero
        float totalLength = 0f;
        // Loop through each segment in the specified path
        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            // Get the start and end positions of the current segment
            Vector3 start = pathNodes[i].transform.position;
            Vector3 end = pathNodes[i + 1].transform.position;
            // Calculate the distance between the start and end points and add it to the total length
            totalLength += Vector3.Distance(start, end);
        }
        // Return the total length of the path
        return totalLength;
    }
    
    /// <summary>
    /// Visualize the path defined by pathNodes On the Editor
    /// </summary>
   private void OnDrawGizmos()
    {
        // Return early if there are fewer than 2 path nodes
        if (pathNodes.Count < 2) return;
        
        // Set the Path Gizmos color to green
        Gizmos.color = Color.green;
        
        // Loop through each segment in the path
        for (int segment = 0; segment < pathNodes.Count - 1; segment++)
        {
            // Get the starting point of the segment
            Vector3 previousPoint = GetPoint(0, segment);
            
            // Draw the curve for the segment Depending of the resolution
            for (int i = 1; i <= CurveResolution; i++)
            {
                //Calculate the parameter t for the current point
                float t = i / (float)CurveResolution;
                // Get the points on the curve
                Vector3 point = GetPoint(t, segment);
                // Draw a line from the previous point to the current point
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
        //If a target is specified, update the position of the last path node to match the target's Pos
        if (target != null)
        {
            PathNode pathNodeObj = pathNodes[pathNodes.Count - 1];
            pathNodeObj.gameObject.transform.position = target.transform.position;
        }
        
        UpdateGzimos();
    }
    /// <summary>
    /// updates the visibility of gizmos (node orbs and influencer orbs) 
    /// </summary>
    void UpdateGzimos()
    {
        // Check if node orbs should be shown
        if (showNodeOrbs)
        {
            // Enable MeshRenderer for each node in pathNodes
            foreach (var nodes in pathNodes)
            {
                nodes.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        } else {
            // Disable MeshRenderer for each node in pathNodes
            foreach (var nodes in pathNodes)
            {
                nodes.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        
        // Check if influencer orbs should be shown
        if (showInfluencerOrbs)
        {
            // Enable MeshRenderer for each child of each node in pathNodes
            foreach (var nodes in pathNodes)
            {
                foreach (Transform children in nodes.gameObject.transform)
                {
                    children.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        else
        {
            // Disable MeshRenderer for each child of each node in pathNodes
            foreach (var nodes in pathNodes)
            {
                foreach (Transform children in nodes.gameObject.transform)
                {
                    children.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
    
    /// <summary>
    /// handles the movement of an object along a Bézier path, adjusting its speed based on a curve and normalizing it over the path length.
    /// </summary>
    void Update()
    {
        // less than 2 PathNodes Return 
        if (pathNodes.Count < 2) return;

        // Use the speed curve for movement adjustment
        float curveValue = speedCurve.Evaluate(T);
        
        // Smooth sampling over the entire Bézier path
        float pathLength = GetTotalPathLength();
        
        // Normalize speed
        T += (speed * curveValue * Time.deltaTime) / pathLength; 

        // Stop Movement
        stopMovement = true;
        
        // Check if the path is loopable
        if (loop)
        {
            if (T > 1f)
            {
                //Reset T to loop back to the start
                T = 0f;
            }
        }
       
        else
        {
            if (T >= 1f)
            {
                // Clamp T to 1 and stop movement
                T = 1f;
                stopMovement = false;
                
                if (destroyOnImpact)
                {
                    // Destroy the object after a delay if it reaches the end
                    Destroy(this.gameObject, destroyDelay);
                }
            } 
        }
        
        // if movement has stopped 
        if (stopMovement)
        {
            // Calculate the number of segments in the path
            int numSegments = pathNodes.Count - 1;
            float segmentT = T * numSegments; 
            int segmentIndex = Mathf.FloorToInt(segmentT);
            float localT = segmentT - segmentIndex;

            // Ensure valid segment index
            if (segmentIndex >= numSegments) segmentIndex = numSegments - 1;

            //Get the position on the Bézier curve
            Vector3 position = GetPoint(localT, segmentIndex);
        
            //Calculate wave offset
            Vector3 waveOffset = GetWaveOffset(segmentT);
        
            //Slight Movement Offset to the flyoutObject
            flyoutObject.transform.position = position + waveOffset;

            // Compute direction for smooth rotation
            Vector3 nextPosition = GetPoint(Mathf.Clamp01(localT + 0.01f), segmentIndex);
            Vector3 direction = (nextPosition - position).normalized;
            flyoutObject.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}