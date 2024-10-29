using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject TrailDriver;

    private Transform DistanceToMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Movetarget;

    public float duration;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //img.material.SetTexture("_MainTex", Texture);
            spawnTrail();

            Debug.Log("TrailSPawned");
        }
    }

    public void spawnTrail()
    {
        GameObject obj = Instantiate(TrailDriver, this.transform.position, this.transform.rotation);
        obj.transform.SetParent(transform);
        obj.AddComponent<Trail>().Setup(Movetarget, duration);
        
    }

}
