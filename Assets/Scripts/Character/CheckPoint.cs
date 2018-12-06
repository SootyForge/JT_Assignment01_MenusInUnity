using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Checkpoint")]
public class CheckPoint : MonoBehaviour
{
    #region Variables
    // Nothing too special here; just references to relevant stuff.
    [Header("CheckPoint")]
    public GameObject currentCheckpoint; // Where the player respawns at on death.
    [Header("Player")]
    public CharacterHandler charH; // The player CharacterHandler (need the right conditions to respawn).
    #endregion

    // Where we fetch and get stuff.
    #region void Start - Get Respawn Components
    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        charH = GetComponent<CharacterHandler>();

        // Check PlayerPrefs to load the player's saved spawn point. 
        if (PlayerPrefs.HasKey("SpawnPoint"))
        {
            // Set currentCheckpoint to the SpawnPoint stored in PlayerPrefs.
            currentCheckpoint = GameObject.Find(PlayerPrefs.GetString("SpawnPoint"));
            // Start the player on the currentCheckpoint position at start
            transform.position = currentCheckpoint.transform.position;
        }
    }
    #endregion

    // Where we check if the player needs to be respawned on the checkpoint or not.
    #region void Update - Execute Respawn Player
    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        // If the player character's health hits 0 (they should be dead (or a zombie?))...
        if (charH.curHealth == 0)
        {
            // Move the player's position back to the currentCheckpoint's position.
            transform.position = currentCheckpoint.transform.position;
            // Oh, and reset a bunch of the player's vitals to make them seem more lively when they come back (I ain't 'fraid of no ghosts).
            charH.curHealth = charH.maxHealth;
            charH.curMana = charH.maxMana;
            charH.curStamina = charH.maxStamina;
            charH.alive = true;
            charH.controller.enabled = true;
        }
    }
    #endregion

    // Where we get our currentCheckpoint from (and where we set it).
    #region -void OnTriggerEnter - Get and Set CheckPoint
    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the player touches a trigger tagged as 'CheckPoint'...
        if (other.CompareTag("CheckPoint"))
        {
            // That's now the player's currentCheckpoint.
            currentCheckpoint = other.gameObject;
            // Save this information to PlayerPrefs (so we start back there on loading the game).
            PlayerPrefs.SetString("SpawnPoint", currentCheckpoint.name);
        }
    } 
    #endregion
}
