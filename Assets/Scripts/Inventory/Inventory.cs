using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Variables
    public static List<Item> inv = new List<Item>();
    public static bool showInv;
    public Item selectedItem;
    public static int money;

    public Vector2 scr = Vector2.zero;
    public Vector2 scrollPos = Vector2.zero;

    public string sortType = "All";

    public Transform dropLocation;
    public Transform[] equippedLocation; // 0 = Right Hand (Weapon) | 1 = Head (Helmet)
    public GameObject curWeapon;
    public GameObject curHelm;
    #endregion

    // Where we... urgh... forget it; you already know what this.
    #region void Start()
    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        // Give us some starting items in our inventory to play around with (You lucky thing you...!~).
        inv.Add(ItemData.CreateItem(000));
        inv.Add(ItemData.CreateItem(002));
        inv.Add(ItemData.CreateItem(102));
        inv.Add(ItemData.CreateItem(202));
        inv.Add(ItemData.CreateItem(302));

        // Debug (print item names in console).
        for (int i = 0; i < inv.Count; i++)
        {
            Debug.Log(inv[i].Name);
        }
    }
    #endregion

    /// I'm listening to Filk Music while I write this.
    /// I like the songs in 'Carmen Miranda's Ghost' best, but 'Minus 10 and Counting' is a good album too.
    // Where we check input to execute 'ToggleInv()'. Pretty much the same way Pause is done.
    #region void Update()
    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!Pause.paused)
            {
                ToggleInv();
            }
        }
    }
    #endregion

    /// Carmen Miranda's Ghost is Haunting Space Station Three.
    /// Half the staff has seen here, plus the Port Master and me.
    /// And if you think we've had too much of Cookie's homemade rum.
    /// Just tell me where these basket hats of fruit keep coming from?
    // Where we toggle the inventory window on and off. This is 'TogglePause()' all over again.
    #region +bool ToggleInv()
    public bool ToggleInv()
    {
        // Like I said: "This is 'TogglePause()' all over again."
        if (showInv)
        {
            showInv = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return (false);
        }
        // If I knew how to do ASCII art (and if I wasn't sensible), I'd insert a picture of a horse here.
        else
        {
            showInv = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return (true);
        }
    }
    #endregion

    /// Our captain built a model of the palace at Versailles
    /// Across the recreation room and no one could get by!
    /// One night we heard what sounded like a mighty cannon's roar;
    /// Versailles had blown to kingdom come with half the recroom floor!
    /// 
    /// All hail the manatee as up she's passing by!
    /// 'Tis true she's the strangest ship that ever tried to fly!
    // Where we do EVERYTHING. I might have a few things to say here at least...
    #region -void OnGUI()
    // OnGUI is called for rendering and handling GUI events
    private void OnGUI()
    {
        /// FOREWORD:
        /// Fun Fact: Throughout writing these scripts, I never just copy-pasted the scripts - not once. I always re-typed everything.
        /// Then I saw the wall of GUI stuff here and everything in DisplayInv(), and... no. I couldn't make myself type all of that out.
        /// I'm sorry.
        /// I still read through it and made the code comments at least...
        // If we're NOT paused...
        if (!Pause.paused)
        {
            // If we are showing our Inventory Window...
            if (showInv)
            {
                // If our aspect ratio is NOT 16:9... then it... is 16:9? Umm...
                // FUTURE JETT! Ask Jamie what this is about again.
                if (scr.x != Screen.width / 16 || scr.y != Screen.height / 9)
                {
                    scr.x = Screen.width / 16;
                    scr.y = Screen.height / 9;
                }

                // Draw background of Inventory Window.
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Inventory");

                // Draw Inventory sorting buttons (clicking each one switches to its corresponding sortType).
                #region Sorting Buttons
                if (GUI.Button(new Rect(5.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "All"))
                {
                    sortType = "All";
                }
                if (GUI.Button(new Rect(6.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Consumables"))
                {
                    sortType = "Consumables";
                }
                if (GUI.Button(new Rect(7.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Craftables"))
                {
                    sortType = "Craftable";
                }
                if (GUI.Button(new Rect(8.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Weapons"))
                {
                    sortType = "Weapon";
                }
                if (GUI.Button(new Rect(9.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Armour"))
                {
                    sortType = "Armour";
                }
                if (GUI.Button(new Rect(10.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Misc"))
                {
                    sortType = "Misc";
                }
                if (GUI.Button(new Rect(11.5f * scr.x, 0.25f * scr.y, scr.x, 0.25f * scr.y), "Quest"))
                {
                    sortType = "Quest";
                } 
                #endregion

                // When we run any interaction with OnGUI(), execute DisplayInv(sortType) ← We changed the sortType when we clicked on the Sorting Buttons.
                DisplayInv(sortType);

                if (selectedItem != null)
                {
                    GUI.DrawTexture(new Rect(11 * scr.x, 1.5f * scr.y, 2 * scr.x, 2 * scr.y), selectedItem.Icon);
                    if (GUI.Button(new Rect(14 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Discard"))
                    {
                        if (curWeapon != null && selectedItem.MeshName == curWeapon.name)
                        {
                            Destroy(curWeapon);
                        }
                        else if (curHelm != null && selectedItem.MeshName == curHelm.name)
                        {
                            Destroy(curHelm);
                        }
                        
                        GameObject clone = Instantiate(Resources.Load("Prefab/" + selectedItem.MeshName) as GameObject, dropLocation.position, Quaternion.identity);
                        clone.AddComponent<Rigidbody>().useGravity = true;

                        if (selectedItem.Amount > 1)
                        {
                            selectedItem.Amount--;
                        }
                        else
                        {
                            inv.Remove(selectedItem);
                            selectedItem = null;
                        }
                        return;
                    }
                    switch (selectedItem.Type)
                    {
                        case ItemTypes.Consumables:
                            GUI.Box(new Rect(8 * scr.x, 5 * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value + "\nHeal: " + selectedItem.Heal + "\nAmount: " + selectedItem.Amount);

                            //if (CharacterHandler.curHealth < CharacterHandler.maxHealth)
                            //{
                            //    if (GUI.Button(new Rect(15 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Eat"))
                            //    {
                            //        CharacterHandler.curHealth += selectedItem.Heal;
                            //        if (selectedItem.Amount > 1)
                            //        {
                            //            selectedItem.Amount--;
                            //        }
                            //        else
                            //        {
                            //            inv.Remove(selectedItem);
                            //            selectedItem = null;
                            //        }
                            //        return;
                            //    }
                            //}
                            break;
                        case ItemTypes.Craftable:
                            GUI.Box(new Rect(8 * scr.x, 5 * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value);
                            if (GUI.Button(new Rect(15 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                            {
                                //craft system a +b = c
                            }
                            break;
                        case ItemTypes.Armour:
                            GUI.Box(new Rect(8 * scr.x, 5 * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value + "\nArmour: " + selectedItem.Armour);
                            if (GUI.Button(new Rect(15 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Wear"))
                            {
                                //attach and wear on character
                            }
                            break;
                        case ItemTypes.Weapon:
                            GUI.Box(new Rect(8 * scr.x, 5 * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value + "\nDamage: " + selectedItem.Damage);
                            if (curWeapon == null || selectedItem.MeshName != curWeapon.name)
                            {
                                if (GUI.Button(new Rect(15 * scr.x, 8.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                                {
                                    if (curWeapon != null)
                                    {
                                        Destroy(curWeapon);
                                    }
                                    curWeapon = Instantiate(Resources.Load("Prefab/" + selectedItem.MeshName) as GameObject, equippedLocation[0]);

                                    curWeapon.GetComponent<ItemHandler>().enabled = false;
                                    curWeapon.name = selectedItem.MeshName;
                                }
                            }
                            break;
                        case ItemTypes.Misc:
                            GUI.Box(new Rect(8 * scr.x, 5 * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value);
                            break;
                        case ItemTypes.Quest:
                            GUI.Box(new Rect(8 * scr.x, 5 * scr.y, 8 * scr.x, 3 * scr.y), selectedItem.Name + "\n" + selectedItem.Description + "\nValue: " + selectedItem.Value);
                            break;
                    }
                }
            }
        }
    }
    #endregion
    void DisplayInv(string sortType)
    {
        if (!(sortType == "All" || sortType == ""))
        {
            ItemTypes type = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), sortType);
            int a = 0;
            int s = 0;
            for (int i = 0; i < inv.Count; i++)
            {
                if (inv[i].Type == type)
                {
                    a++;
                }
            }
            if (a <= 35)
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    if (inv[i].Type == type)
                    {
                        if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + s * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                        {
                            selectedItem = inv[i];
                            Debug.Log(selectedItem.Name);
                        }
                        s++;
                    }
                }
            }
            else
            {
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.75f * scr.y), scrollPos, new Rect(0, 0, 0, 8.75f * scr.y + ((a - 35) * (0.25f * scr.y))), false, true);
                #region Items in Viewing Area
                for (int i = 0; i < inv.Count; i++)
                {
                    if (inv[i].Type == type)
                    {
                        if (GUI.Button(new Rect(0.5f * scr.x, 0 * scr.y + s * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                        {
                            selectedItem = inv[i];
                            Debug.Log(selectedItem.Name);
                        }
                        s++;
                    }
                }
                #endregion
                GUI.EndScrollView();
            }
        }
        else
        {
            #region Non Scroll Inventory
            if (inv.Count <= 35)
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                        Debug.Log(selectedItem.Name);
                    }
                }
            }
            #endregion
            #region Scroll Inventory
            else
            {
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.75f * scr.y), scrollPos, new Rect(0, 0, 0, 8.75f * scr.y + ((inv.Count - 35) * (0.25f * scr.y))), false, true);
                #region Items in Viewing Area
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, 0 * scr.y + i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                        Debug.Log(selectedItem.Name);
                    }
                }
                #endregion
                GUI.EndScrollView();
            }
            #endregion
        }
    }
}
