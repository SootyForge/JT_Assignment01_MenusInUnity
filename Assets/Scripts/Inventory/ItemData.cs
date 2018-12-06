using UnityEngine;

[AddComponentMenu("JT_Assignment01/Inventory Scripts/Item Data")]
public static class ItemData
{
    public static Item CreateItem(int ItemID)
    {
        string name = "";
        string description = "";
        int value = 0;
        int damage = 0;
        int armour = 0;
        int amount = 0;
        int heal = 0;
        string icon = "";
        string mesh = "";
        ItemTypes type = ItemTypes.Armour;

        // Where we keep/set all of our item's data.
        switch(ItemID)
        {
            #region Consumables 000-099
            case 000:
                name = "Pinecone";
                description = "You won't enjoy this one bit, but some spectators might find your painful snacking amusing?";
                value = 0;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = -1;
                icon = "Pinecone_Icon";
                mesh = "Pinecone_Mesh";
                type = ItemTypes.Consumables;
                break;
            case 001:
                name = "Toothpick";
                description = "Perfect for getting food (or Pinecone splinters) out of your teeth.";
                value = 0;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 1;
                icon = "Toothpick_Icon";
                mesh = "Toothpick_Mesh";
                type = ItemTypes.Consumables;
                break;
            case 002:
                name = "Apple";
                description = "Juicy.";
                value = 5;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 5;
                icon = "Apple_Icon";
                mesh = "Apple_Mesh";
                type = ItemTypes.Consumables;
                break;
            /// Placeholder
            /// case 000:
            ///     name = "";
            ///     description = "";
            ///     value = 0;
            ///     damage = 0;
            ///     armour = 0;
            ///     amount = 1;
            ///     heal = 0;
            ///     icon = "_Icon";
            ///     mesh = "_Mesh";
            ///     type = ItemTypes.Consumables;
            ///     break;

            #endregion
            #region Armour 100-199
            case 100:
                name = "Handkerchief";
                description = "DOCTUH!";
                value = 1;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "Handkerchief_Icon";
                mesh = "Handkerchief_Mesh";
                type = ItemTypes.Armour;
                break;
            case 101:
                name = "Wooden Helmet";
                description = "The merchant said it doubles as a bucket.";
                value = 8;
                damage = 0;
                armour = 2;
                amount = 1;
                heal = 0;
                icon = "WoodenHelmet_Icon";
                mesh = "WoodenHelmet_Mesh";
                type = ItemTypes.Armour;
                break;
            case 102:
                name = "Iron Helmet";
                description = "It may rattle when you run, but an aching head is better than a caved-in head.";
                value = 40;
                damage = 0;
                armour = 10;
                amount = 1;
                heal = 0;
                icon = "IronHelmet_Icon";
                mesh = "IronHelmet_Mesh";
                type = ItemTypes.Armour;
                break;
            /// Placeholder
            /// case 100:
            ///     name = "";
            ///     description = "";
            ///     value = 0;
            ///     damage = 0;
            ///     armour = 0;
            ///     amount = 1;
            ///     heal = 0;
            ///     icon = "_Icon";
            ///     mesh = "_Mesh";
            ///     type = ItemTypes.Armour;
            ///     break;
            #endregion
            #region Weapon 200-299
            case 200:
                name = "Wooden Spoon";
                description = "Naughty children will flee in terror.";
                value = 1;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "WoodenSpoon_Icon";
                mesh = "WoodenSpoon_Mesh";
                type = ItemTypes.Weapon;
                break;
            case 201:
                name = "Splintered Oak Club";
                description = "A damaged club.";
                value = 10;
                damage = 4;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "_Icon";
                mesh = "_Mesh";
                type = ItemTypes.Weapon;
                break;
            case 202:
                name = "Rusty Copper Hand Axe";
                description = "This Hand Axe can barely chop through firewood.";
                value = 30;
                damage = 10;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "CopperHandAxe_Icon";
                mesh = "CopperHandAxe_Mesh";
                type = ItemTypes.Weapon;
                break;
            /// Placeholder
            /// case 200:
            ///     name = "";
            ///     description = "";
            ///     value = 0;
            ///     damage = 0;
            ///     armour = 0;
            ///     amount = 1;
            ///     heal = 0;
            ///     icon = "_Icon";
            ///     mesh = "_Mesh";
            ///     type = ItemTypes.Weapon;
            ///     break;
            #endregion
            #region Crafting 300-399
            case 300:
                name = "Twigs";
                description = "Good for campfires.";
                value = 0;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "Twigs_Icon";
                mesh = "Twigs_Mesh";
                type = ItemTypes.Craftable;
                break;
            case 301:
                name = "Iron Ore";
                description = "A lump of raw iron.";
                value = 3;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "IronOre_Icon";
                mesh = "IronOre_Mesh";
                type = ItemTypes.Craftable;
                break;
            case 302:
                name = "Iron Ingot";
                description = "A bar of smelted iron.";
                value = 10;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "IronIngot_Icon";
                mesh = "IronIngot_Mesh";
                type = ItemTypes.Craftable;
                break;
            /// Placeholder
            /// case 300:
            ///     name = "";
            ///     description = "";
            ///     value = 0;
            ///     damage = 0;
            ///     armour = 0;
            ///     amount = 1;
            ///     heal = 0;
            ///     icon = "_Icon";
            ///     mesh = "_Mesh";
            ///     type = ItemTypes.Craftable;
            ///     break;
            #endregion
            #region Misc 400-499
            case 400:
                name = "Cat.";
                description = "Meow.";
                value = 0;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = 0;
                icon = "Cat_Icon";
                mesh = "Cat_Mesh";
                type = ItemTypes.Misc;
                break;
            /// Placeholder
            /// case 400:
            ///     name = "";
            ///     description = "";
            ///     value = 0;
            ///     damage = 0;
            ///     armour = 0;
            ///     amount = 1;
            ///     heal = 0;
            ///     icon = "_Icon";
            ///     mesh = "_Mesh";
            ///     type = ItemTypes.Misc;
            ///     break;
            #endregion
            default:
                ItemID = 000;
                name = "Pinecone";
                description = "You won't enjoy this one bit, but some spectators might find your painful snacking amusing?";
                value = 0;
                damage = 0;
                armour = 0;
                amount = 1;
                heal = -1;
                icon = "Pinecone_Icon";
                mesh = "Pinecone_Mesh";
                type = ItemTypes.Consumables;
                break;
        }

        // Where we create the item
        Item temp = new Item
        {
            Name = name,
            Description = description,
            Id = ItemID,
            Value = value,
            Damage = damage,
            Armour = armour,
            Amount = amount,
            Heal = heal,
            Type = type,
            Icon = Resources.Load("Icons/" + icon) as Texture2D,
            MeshName = mesh
        };
        return temp;
    }
}
