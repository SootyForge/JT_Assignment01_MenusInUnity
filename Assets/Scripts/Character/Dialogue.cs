using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Dialogue")]
public class Dialogue : MonoBehaviour
{
    #region Variables
    // A toggle bool.
    public bool showDlg;

    // Reference scripts. Nothing special here (used in a similar manner to 'Interact.cs').
    [Header("References")]
    public CharacterMovement playerMovement;
    public MouseLook camLook, charLook;

    // Index for counting current line of dialogue and an index for which lines the player gets different responses.
    [Header("Dialogue Index")]
    public int index, optionIndex;

    // I feel like the Header is pretty explanatory here. One of them's an array (so you can add as many lines as you want).
    [Header("NPC Name and Dialogue")]
    public string npcName;
    public string[] dialogueText;

    [Header("Screen Ratio")]
    public Vector2 scr;
    #endregion

    // Where we assign player control scripts to relevant script components (and disable them later).
    #region -void Start - Fetch Character Control Scripts
    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        // Doing that... thing I said.
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        charLook = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();
        camLook = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
    }
    #endregion

    // Where we draw our dialogue window.
    #region -void OnGUI - Draw Dialogue Window
    // OnGUI is called for rendering and handling GUI events
    private void OnGUI()
    {
        // If we are showing Dialogue (toggled/checked in 'Update()' within 'Interact.cs')...
        if (showDlg)
        {
            // Set screen ratio measurements to 16:9. Pretty sure this is more of a failsafe for if it isn't in 16:9 for some reason.
            if (scr.x != Screen.width / 16 || scr.y != Screen.height / 9)
            {
                scr.x = Screen.width / 16;
                scr.y = Screen.height / 9;
            }
            // Draw the dialogue box across the entire bottom-third section of the screen.
            /// Rect string example:
            /// if 'npcName' = "Mr. Pants", and dialogueText[0] = "Meow."...
            /// 'npcName + " : " + dialogueText[index]' = "Mr. Pants : Meow."
            GUI.Box(new Rect(0, 6 * scr.y, Screen.width, 3 * scr.y), npcName + " : " + dialogueText[index]);

            #region GUI Buttons
            // Move to next line of dialogue.
            #region Next Button
            // If the index is NOT beyond or equal to dialogueText.Length - 1 (the last line of dialogue)...
            if (!(index >= dialogueText.Length - 1 || index == optionIndex))
            {
                // Show the 'Next' Button (clicking it increases index by +1 (moves to next line of dialogue)).
                if (GUI.Button(new Rect(15 * scr.x, 8.5f * scr.y, scr.x, 0.5f * scr.y), "Next"))
                {
                    index++;
                }
            }
            #endregion
            // Accept = Move to next line of dialogue / Decline = Skip to the last line of dialogue (Goodbye).
            #region Accept / Decline Button
            // Or if the index is at the line where the player can 'Accept' or 'Decline' a quest...
            else if (index == optionIndex)
            {
                // Show the 'Accept' Button (it's the 'Next' button with a new name).
                if (GUI.Button(new Rect(13 * scr.x, 8.5f * scr.y, scr.x, 0.5f * scr.y), "Accept"))
                {
                    index++;
                }
                // Show the 'Decline' Button (clicking it moves the index to the last line of dialogue).
                if (GUI.Button(new Rect(14 * scr.x, 8.5f * scr.y, scr.x, 0.5f * scr.y), "Decline"))
                {
                    index = dialogueText.Length - 1;
                }
            }
            #endregion
            // End (close) dialogue GUI.
            #region 'Bye.' (End Dialogue) Button
            // Otherwise (if we are at the last line of dialogue)...
            else
            {
                // Show the 'Bye.' Button (clicking it closes dialogue box and 'returns' to the game).
                if (GUI.Button(new Rect(15 * scr.x, 8.5f * scr.y, scr.x, 0.5f * scr.y), "Bye."))
                {
                    // Close dialogue box, reset dialogue to line 0, turn character movement scripts back on, and lock and hide the cursor.
                    // Possibly not in that order.
                    showDlg = false;
                    index = 0;
                    camLook.enabled = true;
                    charLook.enabled = true;
                    playerMovement.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    // But probably in that order.
                }
            } 
            #endregion
            #endregion
        }
    } 
    #endregion
}
