using UnityEngine;

public class Item
{
    #region Variables
    private int _id;
    private string _name;
    private string _description;
    private int _value;
    private int _damage;
    private int _armour;
    private int _amount;
    private int _heal;
    private Texture2D _icon;
    private string _mesh;
    private ItemTypes _type;
    #endregion

    public Item()
    {
        //_id = 0;
        //_name = "Unknown";
        //_description = "???";
        //_value = 0;
        //_mesh = "MeshName";
        //_type = ItemTypes.Quest;
    }
    public Item(int id, string name, int value, string description, ItemTypes type, string meshName)
    {
        _id = id;
        _name = name;
        _value = value;
        _description = description;
        _type = type;
        _mesh = meshName;
    }

    #region Properties
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }
    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    public int Amount
    {
        get { return _amount; }
        set { _amount = value; }
    }
    public int Armour
    {
        get { return _armour; }
        set { _armour = value; }
    }
    public int Heal
    {
        get { return _heal; }
        set { _heal = value; }
    }
    public string MeshName
    {
        get { return _mesh; }
        set { _mesh = value; }
    }
    public Texture2D Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }
    public ItemTypes Type
    {
        get { return _type; }
        set { _type = value; }
    }
    #endregion
}
public enum ItemTypes
{
    Consumables,
    Armour,
    Weapon,
    Craftable,
    Money,
    Quest,
    Misc
}