using UnityEngine;

public class ProjectileThunder : MonoBehaviour
{
    public float launchVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3
                                               (launchVelocity, 0, 0));
    }


}
