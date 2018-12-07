using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Leveling")]
public class LevelUp : MonoBehaviour
{
    #region Variables
    // All the skill point stuff. Uh... yeah.
    public int points;
    public string[] statArray = new string[6];
    public int[] tempStats = new int[6];
    public int[] stats = new int[6];

    // Player (calling it 'levelRef', since only the player's current level is being used here (kind of)).
    public CharacterHandler levelRef;

    // A bool for toggling the Skill Window.
    public bool showSkills;

    // I get the feeling that having both 'levelRef' and 'level' may be a bit redundant?
    public int level = 1;

    // For OnGUI() screen ratio.
    private float scrW, scrH;
    #endregion

    // Where we fetch and load saved player stats.
    #region -void Start() - Get Current Player Stats
    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        // Just setting stuff up. Nothing really new or interesting going on here.
        levelRef = GetComponent<CharacterHandler>();
        statArray = new string[] { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };

        level = levelRef.level;

        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = PlayerPrefs.GetInt(statArray[i]);
        }
        /// Garbage.
        /// // This was an old way I was getting stats without a for loop.
        /// stats = new int[] { levelRef.statSTR, levelRef.statDEX , levelRef.statCON , levelRef.statINT , levelRef.statWIS , levelRef.statCHA };
    }
    #endregion

    // Where we execute some of our self-contained bits (i.e. 'ToggleSkills()' and 'OnLevelUp()').
    #region -void Update() - Execute Stuff
    // Update is called every frame, if the MonoBehaviour is enabled
    public void SkillsToggle()
    {
        // And if the game is not paused...
        if (!Pause.paused)
        {
            // Run 'ToggleSkills()' (Turn on or off (Show or Hide the Skill Window)).
            ToggleSkills();
        }
        /// Garbage.
        /// This isn't broken, but it's solved elsewhere; this was when trying to get the Skill Window hidden when paused.
        /// // If the game is paused...
        /// if (Pause.paused)
        /// {
        ///     // Turn off Skill Window.
        ///     showSkills = false;
        /// }
        /// // Check to run 'OnLevelUp()'.
        OnLevelUp();
    }
    #endregion

    // Where we setup the function to toggle... umm, the name is pretty self explanatory, honestly.
    #region +bool ToggleSkills() - Show/Hide Skill Window
    public bool ToggleSkills()
    {
        // If the Skill Window is Open...
        if (showSkills)
        {
            // Close the Skill Window, unfreeze Time, Lock the Cursor to the centre of the screen, and Hide the Cursor.
            showSkills = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // return (false); ← Set 'public bool ToggleSkills()' to false.
            return (false);
        }
        // Otherwise (if the Skill Window is Closed...)...
        else
        {
            // Open the Skill Window, unfreeze Time, Unlock the Cursor to the screen, and Show the Cursor.
            showSkills = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // return (true); ← Set 'public bool ToggleSkills()' to true.
            return (true);
        }
    }
    #endregion

    // Where we create the GUI stuff.
    #region -void OnGUI - GUI Rendering / Interaction
    // OnGUI is called for rendering and handling GUI events
    private void OnGUI()
    {
        scrW = Screen.width / 16;
        scrH = Screen.height / 9;

        if (!Pause.paused)
        {
            if (showSkills)
            {
                GUI.Box(new Rect(4f * scrW, 4f * scrH, 2f * scrW, 0.5f * scrH), "Points: " + points);

                for (int i = 0; i < 6; i++)
                {
                    if (points > 0)
                    {
                        if (GUI.Button(new Rect(6f * scrW, 4.5f * scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "+"))
                        {
                            points--;
                            tempStats[i]++;
                        }
                    }

                    GUI.Box(new Rect(4f * scrW, 4.5f * scrH + i * (0.5f * scrH), 2f * scrW, 0.5f * scrH), statArray[i] + ": " + (stats[i] + tempStats[i]));

                    if (points < 10 && tempStats[i] > 0)
                    {
                        if (GUI.Button(new Rect(3.5f * scrW, 4.5f * scrH + i * (0.5f * scrH), 0.5f * scrW, 0.5f * scrH), "-"))
                        {
                            points++;
                            tempStats[i]--;
                        }
                    }
                }
            }
        }
    }
    #endregion

    // Where we make the act of leveling up actually do something.
    #region -void OnLevelUp - Add Skill Points
    private void OnLevelUp()
    {
        if (levelRef.level > level)
        {
            points += 5;
            level = levelRef.level;
        }
        PlayerPrefs.SetInt("CharacterLevel", level);
    }
    #endregion
}
