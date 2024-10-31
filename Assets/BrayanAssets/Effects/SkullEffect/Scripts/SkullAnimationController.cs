using UnityEngine;

public class SkullAnimationController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopAttack()
    {
        this.transform.parent.gameObject.GetComponent<Skull>().StopAttack();

    }

    public void SpawnEffectEnergy()
    {
        this.transform.parent.gameObject.GetComponent<Skull>().SpawnEnergy();
    }

}
