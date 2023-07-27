using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameObjects/Shop Item", order = 0)]
public class ShopItem : ScriptableObject
{
    //shop item properties
    public string Name = "Default";
    public int Price;
    public ObjectType Type;
    public Sprite Icon;
    public GameObject Prefab;
}

//types of objects - also tabs
public enum ObjectType
{
    Plot,
    Seed,
    Decorations
}
