using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Items : ScriptableObject
{

    public string itemName;
    [Header("Only Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public EquipmentType equipType;
    public ConsumableType consumeType;
    public int id;
    public int hp;
    public int mp;
    public int def;
    public int att;
    public int rad;
    public int enx;
    public int sol;
    public int chr;
    public int mpcost;
    public int hpcost;
    public int buycost;
    public int sellcost;
    public string desc;
    public Vector2Int range = new Vector2Int(5, 4);
    [Header("Only UI")]
    public bool stackable = true;

    [Header("Only Weapons")]
    public string[] skillName;
    public int[] skillLevel;
    public string[] skillElement;
    public float[] skillPower;
    public int[] mpvalue;
    public int[] hpvalue;

    [Header("Both")]
    public Sprite image;

}

public enum ItemName
{
    itemName
}

public enum ConsumableType
{
    HP,
    MP,
    None
}
public enum ItemType
{
    Equipment,
    Consumable
}

public enum EquipmentType
{
    Hat,
    Shirt,
    Pants,
    Weapon,
    None
}

public enum ActionType
{
    Equip,
    Use
}