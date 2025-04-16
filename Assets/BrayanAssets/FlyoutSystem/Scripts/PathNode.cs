// *****************************************************************************
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
// *****************************************************************************

using UnityEngine;

public class PathNode : MonoBehaviour
{
    public GameObject influencer1;
    public GameObject influencer2;
    public float roll = 0f;

    [HideInInspector]
    public Vector3[] lineToNextNode;
    [HideInInspector]
    public Vector3[] directionToNextBezierNode;
    [HideInInspector]
    public float[] magnitudeToNexBezierNode;
}
