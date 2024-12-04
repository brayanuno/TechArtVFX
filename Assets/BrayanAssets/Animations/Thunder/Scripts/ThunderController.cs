using UnityEngine;

public class ThunderController : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject projectile;

    public float launchVelocity = 1200f;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //primary attack
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            //anim.SetBool("isWalking", true);
            anim.SetTrigger("projectileAttack");

            
        }
        
        
    }
    public void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectile, SpawnPoint.position, Quaternion.identity);
        clone.GetComponent<ProjectileThunder>().launchVelocity = this.launchVelocity * -1;
    }
}
