using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Vector2 = System.Numerics.Vector2;


public class TrailMovement : MonoBehaviour
{
    
    [SerializeField]private Transform targetPos;
    [SerializeField]private GameObject impactObject;
    /*[SerializeField]public float lifeTime = 2f;*/
    
    private float myTime;
    private bool trailReleased = false;
    void Start()
    {
        GetComponent<VisualEffect>().Stop();
        impactObject.SetActive(false);
    }

    
    void ReleaseTrail()
    {
        impactObject.SetActive(false);
        GetComponent<VisualEffect>().SetVector3("TargetPosition",targetPos.position);
        trailReleased = false;
        gameObject.SetActive(true);
        /*GetComponent<VisualEffect>().SetFloat("Life",lifeTime);*/
        
        impactObject.transform.position  = targetPos.transform.position;
        GetComponent<VisualEffect>().Play();
    }
    public void SpawnImpactEffect()
    {
        trailReleased = false;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ReleaseTrail();
            myTime = GetComponent<VisualEffect>().GetFloat("Life");
            trailReleased = true;

        }
        
        if (trailReleased)
        {
            if (myTime > 0 )
            {
                myTime -= Time.deltaTime;
                if (myTime <= 0)
                {
                    impactObject.SetActive(true);
                    impactObject.GetComponent<VisualEffect>().Play();
                    Debug.Log(impactObject.name);
                    trailReleased = false;
                }
            } 
        }
       

    }
}
