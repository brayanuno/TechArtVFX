using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileSpawn : MonoBehaviour
{
    // Start is called once before the first
    // execution of Update after the MonoBehaviour is created

    public GameObject ProjectileToSpawn;
    public Transform ProjectileSpawnedLocation;

    public float projectileSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnProjectile();
          
        }

        

    }

    void SpawnProjectile()
    {
        if (ProjectileToSpawn != null)
        {
            GameObject clone = Instantiate(ProjectileToSpawn, ProjectileSpawnedLocation.transform.position, Quaternion.identity);
            clone.GetComponent<Rigidbody>().linearVelocity = ProjectileSpawnedLocation.transform.forward * projectileSpeed;

        }
        
    }

    void MoveObject()
    {

    }
}
    