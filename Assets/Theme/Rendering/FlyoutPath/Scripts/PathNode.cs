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

using UnityEngine;

public class PathNode : MonoBehaviour
{
    // <summary>
    ///  First adjustment handle
    /// </summary>
    public Transform handle1; 
    
    // <summary>
    ///  Second adjustment handle
    /// </summary>
    public Transform handle2; 
    
    private void OnDrawGizmos()
    {
        // Check if handle1 is not null
        if (handle1 != null)
        {
            //Check if the parent Path component has showInfluencerLines enabled
            if (transform.parent.GetComponent<FlyoutPath>().showInfluencerLines)
                //Draw a line from the current position to handle1's position
                Gizmos.DrawLine(transform.position, handle1.position);
            
        }
        //Check if handle2 is not null
        if (handle2 != null)
        {
            // Check if the parent Path component has showInfluencerLines enabled
            if (transform.parent.GetComponent<FlyoutPath>().showInfluencerLines)
                // Draw a line from the current position to handle2's position
                Gizmos.DrawLine(transform.position, handle2.position);
      
        }
    }
    
    
}