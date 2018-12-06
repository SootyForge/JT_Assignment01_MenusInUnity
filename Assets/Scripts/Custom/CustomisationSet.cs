using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //1 Transfer saved character settings.

[AddComponentMenu("JT_Assignment01/Customization Scripts/Save Custom")]
public class CustomisationSet : MonoBehaviour
{
    #region Variables

    [Header("Texture List")]
    //1 Need a List for each custom character texture to be saved (armour, clothes, hair, etc...).
    public List<Texture2D> skin = new List<Texture2D>();
    public List<Texture2D> hair = new List<Texture2D>();
    public List<Texture2D> mouth = new List<Texture2D>();
    public List<Texture2D> eyes = new List<Texture2D>();
    public List<Texture2D> clothes = new List<Texture2D>();
    public List<Texture2D> armour = new List<Texture2D>();

    [Header("Index")]
    //1 Need to store a number to set the current selected/displayed Texture from an index.
    public int skinIndex;
    public int hairIndex, mouthIndex, eyesIndex, clothesIndex, armourIndex;

    [Header("Renderer")]
    //1 Need to find the character's SkinnedMeshRenderer 
    public Renderer character;

    [Header("Max Index")]
    //1 Define the max range of the Index (prevents switching the List outside the range of available choices).
    public int skinMax;
    public int hairMax, mouthMax, eyesMax, clothesMax, armourMax;

    [Header("Character Name")]
    //1 A string for the Character Name (that's all).
    public string charName = "Adventurer";

    [Header("Stats")]
    //1 Variables for the six character stats (including stat name, base, and modified (skill points spent) stats).
    //1 Number of skill points available to spend.
    public string[] statArray = new string[6];
    public int[] stats = new int[6];
    public int[] tempStats = new int[6];
    public int points = 10;

    [Header("Character Class")]
    //2 Need lots of stuff here to make choosing a character class possible. More specifically:
    
    //2 The CharacterClass itself (stored as an enum).
    //2 The name(s) (string(s)) of the selectedClass.
    //2 The currently selected class' enum value (int).
    public CharacterClass charClass = CharacterClass.Barbarian;
    public string[] selectedClass = new string[8];
    public int selectedIndex = 0;

    //2 Menu handling stuff for the 'Choose Class' button.
    public string button = "Choose Class";
    public bool showDropdown, showCreate;
    private Vector2 scrollPos;

    [Header("GUI Styles")]
    //8 Header says it all.
    public GUIStyle backButton;
    public GUIStyle fieldButton, fieldButtonLocked, leftArrow, rightArrow, startButton, startLocked, charCreateBox, charInfoBox;

    #endregion

    //3 Amusing Fact: I don't really know what the difference is between a Function and a Method. Is there a difference?
    #region Functions 'n' Methods

    //3 Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        //3 Can we use the cursor on this screen (well... yeah).
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //3 Create names for those string arrays you made before, please.
        statArray = new string[] { "Strength", "Dexterity", "Constitution", "Wisdom", "Intelligence", "Charisma" };
        selectedClass = new string[] { "Barbarian", "Bard", "Druid", "Monk", "Paladin", "Ranger", "Sorcerer", "Warlock" };

        //3 Need to set up several for loops in order to load textures available from the Resources folder.
        #region for loop(s) - Pull Textures from Resources

        //3 NOTE: Because all six 'for loop's function identically, here's a general overview of what this is actually doing.
        #region LOGIC BREAKDOWN - for loop example

        // A 'for loop' is designed to hold lines of code you want to execute for multiple, similar items at once.
        // Basically: This means you can execute the same rule/function for every item in an index of near-identical items.
        // Without for loops, if you wanted to add several items to a list, you would need the same code for every item.
        // Individually.

        // In this case, we want to load the skin texture file from the project folder and add it to the List, so...
        // The for loop needs three things: A starting value; a maximum value, and; a direction.
        // This tells it where it first runs its contained code, where it runs next (0, 1, 2...), and where it runs last.
        
        // In practice, ["Skin_" + i] means the for loop will Load: "Skin_0", then "Skin_1", "Skin_2", and so on until
        // every iteration of the "Skin_" texture file has been added to the List of Texture2D items.

        // for (int i = 0; i < skinMax; i++)
        // {
        //     Texture2D temp = Resources.Load("Character/Skin_" + i) as Texture2D;
        //     skin.Add(temp);
        // }

        #endregion

        //3 SKIN
        for (int i = 0; i < skinMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Skin_" + i) as Texture2D;
            skin.Add(temp);
        }

        //3 HAIR
        for (int i = 0; i < hairMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Hair_" + i) as Texture2D;
            hair.Add(temp);
        }

        //3 MOUTH
        for (int i = 0; i < mouthMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Mouth_" + i) as Texture2D;
            mouth.Add(temp);
        }

        //3 EYES
        for (int i = 0; i < eyesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Eyes_" + i) as Texture2D;
            eyes.Add(temp);
        }

        //3 CLOTHES
        for (int i = 0; i < clothesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Clothes_" + i) as Texture2D;
            clothes.Add(temp);
        }

        //3 ARMOUR
        for (int i = 0; i < armourMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Armour_" + i) as Texture2D;
            armour.Add(temp);
        }

        #endregion

        //3 Find and connect the SkinnedMeshRenderer from the player's character Mesh in the scene.
        character = GameObject.Find("Mesh").GetComponent<SkinnedMeshRenderer>();

        //3-4 Set the default textures (REQUIRES SetTexture FUNCTION) and character class.
        #region Set Default Character Appearance + Class

        //4 Set default textures.
        SetTexture("Skin", 0);
        SetTexture("Hair", 0);
        SetTexture("Mouth", 0);
        SetTexture("Eyes", 0);
        SetTexture("Clothes", 0);
        SetTexture("Armour", 0);

        //3 Set default class.
        ChooseClass(selectedIndex);

        #endregion
    }

    //4 Function for switching between different available textures. 'type' = material name, 'dir' = direction.
    void SetTexture(string type, int dir)
    {
        //4 Add internal variables here for a material index, and a Texture2D array.
        int index = 0, max = 0, matIndex = 0;
        Texture2D[] textures = new Texture2D[0];

        //4 Create a switch statement to swap 'type' in 'string type' with each material's name
        #region Switch Material

        switch (type)
        {
            // case "[MATERIAL NAME HERE]":
                // index = [RELEVANT INDEX HERE];
                // max = [RELEVANT indexMAX HERE];
                // textures = [RELEVANT LIST NAME HERE].ToArray();
                // matIndex = [int (each case's matIndex value must succeed its predecesor (case "A" = 1, case "B" = 2))];
                // break;

            case "Skin":
                index = skinIndex;
                max = skinMax;
                textures = skin.ToArray();
                matIndex = 1;
                break;

            case "Mouth":
                index = mouthIndex;
                max = mouthMax;
                textures = mouth.ToArray();
                matIndex = 2;
                break;

            case "Eyes":
                index = eyesIndex;
                max = eyesMax;
                textures = eyes.ToArray();
                matIndex = 3;
                break;

            case "Hair":
                index = hairIndex;
                max = hairMax;
                textures = hair.ToArray();
                matIndex = 4;
                break;

            case "Clothes":
                index = clothesIndex;
                max = clothesMax;
                textures = clothes.ToArray();
                matIndex = 5;
                break;

            case "Armour":
                index = armourIndex;
                max = armourMax;
                textures = armour.ToArray();
                matIndex = 6;
                break;
        }

        #endregion

        //4 Now that we can switch through each material's textures, we need to make sure the index can't fall out of range.
        #region Switch Clamp

        //4 Add direction changes to the index value (THIS IS IMPORTANT FOR SOMETHING IN 'OnGUI()' LATER).
        index += dir;

        //4 Clamp the index so it can't go out of range
        if (index < 0)
        {
            index = max - 1;
        }
        if (index > max - 1)
        {
            index = 0;
        }

        //4 Create a Material array that's connected to the character's material list.
        //4 Then, link 'mat' array's mainTexture to the whole 'textures' array's index.
        //4 Lastly, connect the character.materials back to the 'mat' array ('back to the'...? That rhymed unintentionally.).
        Material[] mat = character.materials;
        mat[matIndex].mainTexture = textures[index];
        character.materials = mat;

        #endregion

        //4 With the index range successfully clamped, create another switch statement to link [RELEVANT INDEX HERE] to index.
        #region Switch Material Set

        switch (type)
        {
            // case "[MATERIAL NAME HERE]":
                // [RELEVANT INDEX HERE] = index;
                // break;

            case "Skin":
                skinIndex = index;
                break;

            case "Mouth":
                mouthIndex = index;
                break;

            case "Eyes":
                eyesIndex = index;
                break;

            case "Hair":
                hairIndex = index;
                break;

            case "Clothes":
                clothesIndex = index;
                break;

            case "Armour":
                armourIndex = index;
                break;
        }
        
        #endregion

    }

    //6 Function to Save our chosen index settings to PlayerPrefs. It sure does save stuff.
    void Save()
    {
        //6 Need to SetInt (appearance and stats) and SetString(name and class).
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
        PlayerPrefs.SetInt("HairIndex", hairIndex);
        PlayerPrefs.SetInt("MouthIndex", mouthIndex);
        PlayerPrefs.SetInt("EyesIndex", eyesIndex);
        PlayerPrefs.SetInt("ClothesIndex", clothesIndex);
        PlayerPrefs.SetInt("ArmourIndex", armourIndex);
        PlayerPrefs.SetString("CharacterName", charName);

        //6 Set stats with a for loop; it's much better this way.
        for (int i = 0; i < stats.Length; i++)
        {
            PlayerPrefs.SetInt(statArray[i], (stats[i] + tempStats[i]));
        }

        //6 This is setting CharacterClass. Moving along.
        PlayerPrefs.SetString("CharacterClass", selectedClass[selectedIndex]);
    }

    //7 OnGUI is called for rendering and handling GUI events
    private void OnGUI()
    {
        //7 Need two floats to help with scaling to Screen aspect ratio
        //7 Also need an int to help make manipulation (moving/shuffling) of GUI elements easier.
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;

        int i = 0;

        #region Background Stuff
        // Character Create Box.
        GUI.Box(new Rect(1.1f * scrW, 7.575f * scrH, 7.575f * scrW, 0.825f * scrH), "", charCreateBox);
        // Character Info Box.
        GUI.Box(new Rect(2.491667f * scrW, 7.716667f * scrH, 4.791667f * scrW, 0.333333f * scrH), "", charInfoBox);
        #endregion

        

        //7 I can't comment all of this. I'm sorry.
        #region Create Button

        //7 Button to execute the Save() Function.
        if (GUI.Button(new Rect(7.541667f * scrW, 8.175f * scrH, 0.75f * scrW, 0.183333f * scrH), "Create", fieldButton))
        {
            showCreate = !showCreate;
        }

        if (showCreate)
        {
            //7 Where we create our GUI elements to make use of the SetTexture() Fuction, amongst other things.
            #region Player Appearance Buttons

            #region SetTexture Buttons

            #region Skin

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                SetTexture("Skin", -1);
            }

            GUI.Box(new Rect(6.75f * scrW, scrH + i * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Skin");

            if (GUI.Button(new Rect(7.75f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                SetTexture("Skin", 1);
            }

            i++;

            #endregion

            #region Mouth

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                SetTexture("Mouth", -1);
            }

            GUI.Box(new Rect(6.75f * scrW, scrH + i * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Mouth");

            if (GUI.Button(new Rect(7.75f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                SetTexture("Mouth", 1);
            }

            i++;

            #endregion

            #region Eyes

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                SetTexture("Eyes", -1);
            }

            GUI.Box(new Rect(6.75f * scrW, scrH + i * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Eyes");

            if (GUI.Button(new Rect(7.75f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                SetTexture("Eyes", 1);
            }

            i++;

            #endregion

            #region Hair

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                SetTexture("Hair", -1);
            }

            GUI.Box(new Rect(6.75f * scrW, scrH + i * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Hair");

            if (GUI.Button(new Rect(7.75f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                SetTexture("Hair", 1);
            }

            i++;

            #endregion

            #region Clothes

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                SetTexture("Clothes", -1);
            }

            GUI.Box(new Rect(6.75f * scrW, scrH + i * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Clothes");

            if (GUI.Button(new Rect(7.75f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                SetTexture("Clothes", 1);
            }

            i++;

            #endregion

            #region Armour

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "<"))
            {
                SetTexture("Armour", -1);
            }

            GUI.Box(new Rect(6.75f * scrW, scrH + i * (0.5f * scrH), 1f * scrW, 0.5f * scrH), "Armour");

            if (GUI.Button(new Rect(7.75f * scrW, scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), ">"))
            {
                SetTexture("Armour", 1);
            }

            i++;

            #endregion

            #endregion

            #region SetTexture Random Button

            if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), scrW, 0.5f * scrH), "Random"))
            {
                SetTexture("Skin", Random.Range(0, skinMax - 1));
                SetTexture("Mouth", Random.Range(0, mouthMax - 1));
                SetTexture("Eyes", Random.Range(0, eyesMax - 1));
                SetTexture("Hair", Random.Range(0, hairMax - 1));
                SetTexture("Clothes", Random.Range(0, clothesMax - 1));
                SetTexture("Armour", Random.Range(0, armourMax - 1));
            }

            #endregion

            #region SetTexture Reset Button

            if (GUI.Button(new Rect(7.25f * scrW, scrH + i * (0.5f * scrH), scrW, 0.5f * scrH), "Reset"))
            {
                if (GUI.Button(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), scrW, 0.5f * scrH), "Random"))
                {
                    SetTexture("Skin", skinIndex = 0);
                    SetTexture("Mouth", mouthIndex = 0);
                    SetTexture("Eyes", eyesIndex = 0);
                    SetTexture("Hair", hairIndex = 0);
                    SetTexture("Clothes", clothesIndex = 0);
                    SetTexture("Armour", armourIndex = 0);
                }
            }

            i++;

            #endregion

            #endregion

            //7 I think I may have gone a little overboard with regions here, but... oh well.
            #region Character Name TextField

            charName = GUI.TextField(new Rect(6.25f * scrW, scrH + i * (0.5f * scrH), 2f * scrW, 0.5f * scrH), charName, 16);

            i++;

            #endregion

            #region Player Stat Point Buttons

            #region Stat Point +/- Buttons

            GUI.Box(new Rect(9.25f * scrW, 1f * scrH, 2f * scrW, 0.5f * scrH), "Points: " + points);

            for (int s = 0; s < 6; s++)
            {
                if (points > 0)
                {
                    if (GUI.Button(new Rect(11.25f * scrW, 1.5f * scrH + s * (0.5f * scrH), 0.5f * scrH, 0.5f * scrH), "+"))
                    {
                        points--;
                        tempStats[s]++;
                    }
                }

                GUI.Box(new Rect(9.25f * scrW, 1.5f * scrH + s * (0.5f * scrH), 2f * scrW, 0.5f * scrH), statArray[s] + ":" + (stats[s] + tempStats[s]));

                if (points < 10 && tempStats[s] > 0)
                {
                    if (GUI.Button(new Rect(8.75f * scrW, 1.5f * scrH + s * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "-"))
                    {
                        points++;
                        tempStats[s]--;
                    }
                }
            }

            #endregion

            #region Stat Point Reset Button

            if (points <= 10)
            {
                if (GUI.Button(new Rect(9.25f * scrW, scrH + 7f * (0.5f * scrH), 2f * scrW, 0.5f * scrH), "Reset"))
                {
                    points = 10;
                    tempStats[0] = 0;
                    tempStats[1] = 0;
                    tempStats[2] = 0;
                    tempStats[3] = 0;
                    tempStats[4] = 0;
                    tempStats[5] = 0;
                }
            }

            #endregion

            #endregion

            #region Player Class Select Dropdown

            if (GUI.Button(new Rect(12.25f * scrW, 2f * scrH, 2f * scrW, 0.5f * scrH), button))
            {
                showDropdown = !showDropdown;
            }

            if (showDropdown)
            {
                if (selectedClass.Length <= 6)
                {
                    scrollPos = GUI.BeginScrollView(new Rect(12.25f * scrW, 3f * scrH, 3f * scrW, 3f * scrH), scrollPos, new Rect(0, 0, 2f * scrW, 3f * scrH), false, true);
                }
                else
                {
                    scrollPos = GUI.BeginScrollView(new Rect(12.25f * scrW, 3f * scrH, 3f * scrW, 3f * scrH), scrollPos, new Rect(0, 0, 2f * scrW, 3f * scrH + ((selectedClass.Length - 6) * (scrH * 0.5f))), false, true);
                }

                for (int c = 0; c < selectedClass.Length; c++)
                {
                    if (GUI.Button(new Rect(0f, 0f + c * (scrH * 0.5f), 3f * scrW, 0.5f * scrH), selectedClass[c]))
                    {
                        selectedIndex = c;
                        ChooseClass(selectedIndex);
                        button = selectedClass[c];
                    }
                }
                GUI.EndScrollView();
            }

            #endregion

            if (GUI.Button(new Rect(7.541667f * scrW, 5f * scrH, 0.75f * scrW, 0.183333f * scrH), "Accept", fieldButton))
            {
                Save();
                showCreate = false;
            }
        }

        #endregion

        #region Delete Button
        if (!PlayerPrefs.HasKey("CharacterName"))
        {
            GUI.Box(new Rect(1.508333f * scrW, 8.175f * scrH, 0.75f * scrW, 0.183333f * scrH), "Delete", fieldButtonLocked);
        }
        if (PlayerPrefs.HasKey("CharacterName"))
        {
            if (GUI.Button(new Rect(1.508333f * scrW, 8.175f * scrH, 0.75f * scrW, 0.183333f * scrH), "Delete", fieldButton))
            {
                PlayerPrefs.DeleteKey("CharacterName");
            } 
        }
        #endregion

        #region Back Button
        if (GUI.Button(new Rect(0.425f * scrW, 8.625f * scrH, 1.25f * scrW, 0.3f * scrH), "Back", backButton))
        {
            SceneManager.LoadScene(0);
        }
        #endregion

        #region Start Button
        if (!PlayerPrefs.HasKey("CharacterName"))
        {
            GUI.Box(new Rect(14.275f * scrW, 8.625f * scrH, 1.25f * scrW, 0.3f * scrH), "Start", startLocked);
        }

        if (PlayerPrefs.HasKey("CharacterName"))
        {
            if (GUI.Button(new Rect(14.275f * scrW, 8.625f * scrH, 1.25f * scrW, 0.3f * scrH), "Start", startButton))
            {
                SceneManager.LoadScene(2);
            }
        }
        #endregion
        
    }

    //5 Method to make choosing a different class actually mean something (erm... sort of.).
    void ChooseClass(int className)
    {
        //5 Create a switch statement to switch the value of 'className' (each case has its own stats and CharacterClass).
        switch (className)
        {
            // case [int]:
                // stats[0] = int;
                // stats[1] = int;
                // ...
                // stats[5] = int;
                // charClass = CharacterClass.[CLASS NAME HERE];
                // break;
                
            case 0:
                stats[0] = 15;
                stats[1] = 10;
                stats[2] = 10;
                stats[3] = 10;
                stats[4] = 10;
                stats[5] = 5;
                charClass = CharacterClass.Barbarian;
                break;
                
            case 1:
                stats[0] = 5;
                stats[1] = 10;
                stats[2] = 10;
                stats[3] = 10;
                stats[4] = 10;
                stats[5] = 15;
                charClass = CharacterClass.Bard;
                break;
                
            case 2:
                stats[0] = 10;
                stats[1] = 10;
                stats[2] = 10;
                stats[3] = 10;
                stats[4] = 10;
                stats[5] = 10;
                charClass = CharacterClass.Druid;
                break;
                
            case 3:
                stats[0] = 5;
                stats[1] = 15;
                stats[2] = 15;
                stats[3] = 10;
                stats[4] = 10;
                stats[5] = 5;
                charClass = CharacterClass.Monk;
                break;
                
            case 4:
                stats[0] = 15;
                stats[1] = 10;
                stats[2] = 15;
                stats[3] = 5;
                stats[4] = 5;
                stats[5] = 10;
                charClass = CharacterClass.Paladin;
                break;
                
            case 5:
                stats[0] = 5;
                stats[1] = 15;
                stats[2] = 10;
                stats[3] = 15;
                stats[4] = 10;
                stats[5] = 5;
                charClass = CharacterClass.Ranger;
                break;
                
            case 6:
                stats[0] = 10;
                stats[1] = 10;
                stats[2] = 10;
                stats[3] = 15;
                stats[4] = 10;
                stats[5] = 5;
                charClass = CharacterClass.Sorcerer;
                break;
                
            case 7:
                stats[0] = 5;
                stats[1] = 5;
                stats[2] = 5;
                stats[3] = 15;
                stats[4] = 15;
                stats[5] = 15;
                charClass = CharacterClass.Warlock;
                break;
        }
    }

    #endregion
}

//2 enum of choices available for the player's Character Class.
public enum CharacterClass
{
    Barbarian,
    Bard,
    Druid,
    Monk,
    Paladin,
    Ranger,
    Sorcerer,
    Warlock
}
