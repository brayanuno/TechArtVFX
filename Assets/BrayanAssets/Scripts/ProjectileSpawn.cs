using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    // Start is called once before the first
    // execution of Update after the MonoBehaviour is created



    public float launchVelocity = 400f;
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

        GetComponent<Rigidbody>().AddRelativeForce(new Vector3
                                               (0, launchVelocity, 0));
    }

    void MoveObject()
    {

    }
}
