using UnityEngine;

public class SpriteGolderDragon : MonoBehaviour
{
    public GameObject BorderFire;
    public GameObject SpawnGoldenDragon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnGoldenDragonActive()
    {
        GameObject activeGoldenDragon = Instantiate(SpawnGoldenDragon, Vector3.zero, Quaternion.identity);
        activeGoldenDragon.transform.parent = transform.parent;
        activeGoldenDragon.transform.localPosition = Vector3.zero;
        activeGoldenDragon.transform.localScale = Vector3.one;
        activeGoldenDragon.transform.localRotation = Quaternion.identity;
    }

    void SpawnEffectFireBorder()
    {
        Debug.Log("Spawning Effect");
        
        GameObject obj = Instantiate(BorderFire, Vector3.zero, Quaternion.identity);
        
        obj.transform.parent = transform.parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        
    }
}
