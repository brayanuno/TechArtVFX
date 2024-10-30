using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject TrailDriver;

    private Transform DistanceToMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Transform target;
    public float duration;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnTrail()
    {
        GameObject obj = Instantiate(TrailDriver, this.transform.position, this.transform.rotation);
        //obj.AddComponent<Trail>().Setup();

    }


}
