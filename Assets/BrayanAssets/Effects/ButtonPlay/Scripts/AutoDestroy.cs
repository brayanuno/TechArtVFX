using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    private ParticleSystem particleEffect;

    public float destroyTimer;

    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particleEffect = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (particleEffect)
        {
            if (!particleEffect.IsAlive())
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (timer > destroyTimer && gameObject)
            {
                Destroy(gameObject);
            }
        }


    }
}
