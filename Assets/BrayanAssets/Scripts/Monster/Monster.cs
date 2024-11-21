using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterData data;


    [Separator(1,2)]
    [SerializeField]
    private int health;
    private void Awake()
    {
        
        Debug.Log("print name: " + data.Name);

        Debug.Log("print Damage: " + data.Damage);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.RangeOfAwareness);

        Gizmos.color = Color.green;
        Vector3 direction = transform.forward * data.RangeOfAwareness;
        Gizmos.DrawRay(transform.position, direction);
    }   
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
