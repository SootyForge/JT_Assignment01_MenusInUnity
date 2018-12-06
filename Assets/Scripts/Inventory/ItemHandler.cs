using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    #region Variables
    public int itemId = 0; // Change this number in the Inspector to tell the game what the item we're picking up is.
    public ItemTypes itemType; // Seen this already. It's in 'Item.cs'. Umm... it's the item's category. We set it. Yes.
    public int amount; // Change this in the Inspector (how many of this item are we picking up).
    #endregion

    // Where we make it possible to pick up and add an item to your inventory (it's used in Interact.cs).
    #region +void OnCollection() - Pick Up / Add To Inventory
        /// Foreword
        /// After carefully reading and re-reading the Craftable / Consumables section,
        /// I can safely say I understand what's happening there (it's pretty clever).
    public void OnCollection()
    {
        #region Money
        // If we're picking up Money... increase our character's amount of money in their Inventory by amount's value.
        if (itemType == ItemTypes.Money)
        {
            Inventory.money += amount;
        }
        #endregion
        #region Craftable / Consumables
        // Or else if we're picking up a craftable or a consumable...
        else if (itemType == ItemTypes.Craftable || itemType == ItemTypes.Consumables)
        {
            // These two ints are used pretty much like bools to check if...
            int found = 0; // ... we do not have any of this item...
            int addIndex = 0; // ... how much of this item we have (we don't have any of the item).

            #region Mark All Items in Inventory as Valid
            // For everything from (the first element; to the last element (in the Inventory List); 0 → 1 → 2...), do this:
            for (int i = 0; i < Inventory.inv.Count; i++)
            {
                // If itemId matches a valid itemId from the Inventory Item List...
                if (itemId == Inventory.inv[i].Id)
                {
                    // Then we already have an instance of this item in our Inventory with an additive quantity (addIndex).
                    found = 1;
                    addIndex = 1;
                    break;
                }
            }
            #endregion
            #region When we DO have an Item
            // If we have an instance of this item in our Inventory...
            if (found == 1)
            {
                // Increase our item's current quantity (addIndex) by amount.
                Inventory.inv[addIndex].Amount += amount;
            }
            #endregion
            #region When we DON'T have an Item
            // Otherwise (found == 0 (we DON'T have an any of this item in our Inventory))...
            else
            {
                // Create an instance of this item in our Inventory.
                Inventory.inv.Add(ItemData.CreateItem(itemId));

                // if the item we're picking up is a stacked item (it has more than one)...
                if (amount > 1) // Without this, you can only go from 0 to 1.
                {
                    // That for loop from before (check through everything in the Inventory).
                    for (int i = 0; i < Inventory.inv.Count; i++)
                    {
                        // If itemId matches a valid itemId from the Inventory List...
                        if (itemId == Inventory.inv[i].Id)
                        {
                            // Then we get more than one of this brand new item.
                            Inventory.inv[i].Amount = amount;
                        }
                    }
                }
            } 
            #endregion
        }
        #endregion
        #region Everything Else (Non-Stackables)
        // Otherwise (it's a weapon, piece of armour, or something, so...)...
        else
        {
            // Create an instance of this item in our Inventory.
            Inventory.inv.Add(ItemData.CreateItem(itemId));
        } 
        #endregion
        //DragAndDropInventory.AddItem(itemId); // Add to a slot in the DragAndDropInventory.
        Destroy(gameObject); // Delete the item from the world (it's in our pocket-universe - err, inventory now).
    }
    #endregion
}
