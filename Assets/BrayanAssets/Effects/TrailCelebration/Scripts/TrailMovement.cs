using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;


public class TrailMovement : MonoBehaviour
{
    public Transform TrailStartingPosition;
    public Transform TrailTargetToMove;
    public GameObject driverTrail;
    
    public GameObject impactEffect;
    public float lifeTime = 2f;
    private float myTime;
    private bool trailReleased = false;
    void Start()
    {
        driverTrail.gameObject.SetActive(false);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void InitializeValues()
    {
        trailReleased = false;
        driverTrail.SetActive(true);
        driverTrail.GetComponent<VisualEffect>().SetFloat("Life",lifeTime);
        driverTrail.transform.position = TrailStartingPosition.transform.position;
        impactEffect.transform.position  = TrailTargetToMove.transform.position;
        
    }
    public void SpawnImpactEffect()
    {

        /*impactSpawned.transform.parent = transform;
        impactSpawned.transform.position = TrailTargetToMove.position;
        impactSpawned.transform.localScale = Vector3.one;
        impactSpawned.transform.localRotation = Quaternion.identity;*/
        
        trailReleased = false;
        /*impactEffect.gameObject.SetActive(true);*/
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            InitializeValues();
            driverTrail.GetComponent<VisualEffect>().Play();
            myTime = lifeTime;
            trailReleased = true;
            //Spawned Objet
            /*GameObject trailSpawned = Instantiate(driverTrail, TrailStartingPosition.position, TrailStartingPosition.rotation);
            driverTrail.GetComponent<VisualEffect>().SetFloat("Life",lifeTime);


            trailSpawned.transform.parent = transform;
            trailSpawned.transform.position = TrailStartingPosition.position;
            trailSpawned.transform.localScale = Vector3.one;
            trailSpawned.transform.localRotation = Quaternion.identity;
            /*Debug.Log("space key was pressed"); #1#
            myTime = lifeTime;
            trailReleased = true;*/
            /*trailEffect.SetFloat("Life",lifeTime);*/


        }
        
        if (trailReleased)
        {
            if (myTime > 0 )
            {
                myTime -= Time.deltaTime;
                if (myTime <= 0)
                {
                    impactEffect.GetComponent<VisualEffect>().Play();
                    Debug.Log(impactEffect.name);
                    trailReleased = false;
                }
            } 
        }
       

    }
}
