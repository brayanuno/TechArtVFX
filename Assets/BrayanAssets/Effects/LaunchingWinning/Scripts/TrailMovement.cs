using UnityEngine;
using UnityEngine.VFX;


public class TrailMovement : MonoBehaviour
{
    public VisualEffect trailEffect;
    public GameObject impactEffect;
    public float lifeTime = 2f;
    private float myTime;
    void Start()
    {
        /*impactEffect.gameObject.SetActive(false);*/
        if (trailEffect == null) return;
        trailEffect.SetFloat("Life",lifeTime);
        
        trailEffect.Play();
        myTime = lifeTime;
    }

    public void SpawnImpactEffect()
    {
        impactEffect.GetComponent<VisualEffect>().Play();
        /*GameObject obj = Instantiate(impactEffect,Vector3.zero, transform.rotation);
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.parent = transform;*/
        /*impactEffect.gameObject.SetActive(true);*/
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (myTime > 0 )
        {
            myTime -= Time.deltaTime;
            if (myTime <= 0)
            {
                SpawnImpactEffect();
            }
        } 

    }
}
