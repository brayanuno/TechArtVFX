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

    public MonsterData Data { get => data;}

    private void Awake()
    {
        
        Debug.Log("print name: " + Data.Name);

        Debug.Log("print Damage: " + Data.Damage);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.RangeOfAwareness);

        Gizmos.color = Color.green;
        Vector3 direction = transform.forward * Data.RangeOfAwareness;
        Gizmos.DrawRay(transform.position, direction);
    }   
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
