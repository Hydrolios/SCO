using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Items : ScriptableObject
{


    [Header("Only Gameplay")]
    public ItemType type;
    public ActionType actionType;
    public int id;
    public int hp;
    public Vector2Int range = new Vector2Int(5, 4);
    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;

}

public enum ItemType
{
    Equipment,
    Consumable
}

public enum ActionType
{
    Equip,
    Use
}