using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData_", menuName = "UnitData/Monster")]
public class MonsterData : ScriptableObject
{

    [Header("GeneralStats")]
    [SerializeField]
    private string name = "....";

    [SerializeField]
    private MonsterType monsterType = MonsterType.None;

    [SerializeField]
    [Range(0, 100)]
    private float changeToDropItem = 20f;

    [SerializeField]
    private float rangeOfAwareness = 10f;

    [SerializeField]
    private bool _canEnterCombat = true;

    [Header("ComboStats")]
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private int health = 1;

    [SerializeField]
    private int speed = 1;

    [Header("Dialogue")]
    [SerializeField]
    [Tooltip("Speak dialogue when entering combat")]
    [TextArea()]
    private string battlecry = "..." ;


    public string Name => name; 

    public MonsterType MonsterType { get => monsterType;}
    public float ChangeToDropItem { get => changeToDropItem;}
    public float RangeOfAwareness { get => rangeOfAwareness;}

    public bool canEnterCombat => canEnterCombat;

    public int Damage { get => damage; set => damage = value; }
    public int Health { get => health; set => health = value; }
    public int Speed { get => speed; set => speed = value; }
}
