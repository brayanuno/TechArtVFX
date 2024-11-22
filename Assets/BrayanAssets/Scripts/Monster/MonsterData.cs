using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData_", menuName = "UnitData/Monster")]
public class MonsterData : ScriptableObject
{

    [SerializeField]
    private string name = "....";

    [SerializeField]
    private MonsterType monsterType = MonsterType.None;

    [SerializeField]
    [Range(0, 100)]
    private float chanceToDropItem = 20f;

    [SerializeField]
    private float rangeOfAwareness = 10f;

    [SerializeField]
    private bool canEnterCombat = true;

    //[Header("ComboStats")]
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private int health = 1;

    [SerializeField]
    private int speed = 1;

    //[Header("Dialogue")]
    [SerializeField]
    [Tooltip("Speak dialogue when entering combat")]

    [TextArea()]
    private string battlecry = "..." ;

    [SerializeField]
    private MonsterAbility[] abilities;

    public string Name => name; 

    public MonsterType MonsterType { get => monsterType;}
    public float ChangeToDropItem { get => chanceToDropItem;}
    public float RangeOfAwareness { get => rangeOfAwareness;}

    public bool CanEnterCombat => canEnterCombat;

    public int Damage { get => damage;}
    public int Health { get => health;}
    public int Speed { get => speed;  }
    public MonsterAbility[] Abilities  => abilities; 
}
