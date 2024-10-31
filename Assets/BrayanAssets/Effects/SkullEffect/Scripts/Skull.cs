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
        anim = SkullMesh.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !anim.GetBool("Attack"))
        {
            //img.material.SetTexture("_MainTex", Texture);
            Attack();

        }
    }


    public void UpdateScore()
    {
       
        GameObject effectEyes = Instantiate(EffectEyes,eyesSpawner.position, eyesSpawner.rotation);
        effectEyes.transform.SetParent(eyesSpawner);
    }

    public void SpawnEnergy()
    {
        //SpawnTrail
        GameObject obj = Instantiate(EffectTrail, TrailSpawner.position , this.transform.rotation);
        obj.GetComponent<Trail>().Setup(target, duration);
        obj.transform.SetParent(transform);

    }

    public void Attack()
    {
        anim.SetBool("Attack", true);
        UpdateScore();
    }

    public void StopAttack()
    {
        anim.SetBool("Attack", false);
    }


}
