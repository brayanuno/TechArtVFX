using UnityEngine;

public class Skull : MonoBehaviour
{

    private Animator anim;

    [Header("Transforms")]
    public Transform TrailSpawner;
    public Transform eyesSpawner;

    [Header("Effects")]
    public GameObject EffectTrail;
    public GameObject EffectEyes;

    [Header("Mesh")]
    public GameObject target;
    public GameObject SkullMesh;


    public float duration;

   
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //img.material.SetTexture("_MainTex", Texture);
            ScorePoint();



        }
    }


    public void ScorePoint()
    {
        SpawnTrail();

        GameObject effectEyes = Instantiate(EffectEyes,eyesSpawner.position, eyesSpawner.rotation);
        effectEyes.transform.SetParent(eyesSpawner);
    }

    

    public void SpawnTrail()
    {
        //SpawnTrail
        GameObject obj = Instantiate(EffectTrail, TrailSpawner.position , this.transform.rotation);
        obj.GetComponent<Trail>().Setup(target, duration);
        obj.transform.SetParent(transform);
        


    }

}
