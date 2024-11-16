using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class FireProjectile : MonoBehaviour
{

    public VisualEffect EffectAsset;
    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        targetPosition = EffectAsset.GetComponent<VisualEffect>().GetVector3("TargetToMove");

    }

    // Update is called once per frame
    void Update()
    {

        
        //if (Vector3.Equals(),targetPosition))
        //{
        //    Debug.Log("TargetReached");
        //}
        
    }
}
