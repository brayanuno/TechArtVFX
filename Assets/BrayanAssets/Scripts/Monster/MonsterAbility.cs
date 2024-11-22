using UnityEngine;

public enum ElementType
{
    None,
    Earth,
    Fire,
    Water,
    Poison 
}

[System.Serializable]
public class MonsterAbility 
{
    [SerializeField]
    private string name = "";

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private ElementType elementType = ElementType.None;
}
