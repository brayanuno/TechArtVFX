using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    private ParticleSystem particleEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particleEffect = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(particleEffect)
        {
            if ( !particleEffect.IsAlive())
            {   
                Destroy(gameObject);
            }
        }
    }
}
