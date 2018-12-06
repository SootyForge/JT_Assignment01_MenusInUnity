using System.Collections;
using UnityEngine;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Interact")]
public class Interact : MonoBehaviour
{
    #region Variables
    // Reference GameObject(s) for your Player and Main Camera.
    public GameObject player;
    public GameObject mainCam;
    #endregion

    // Where we get our two GameObjects and set our starting cursor state.
    #region void Start - Get and Set Components
    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        // Lock the cursor to the centre of the screen, and hide the cursor on the screen.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get and Set our two GameObjects.
        player = GameObject.Find(PlayerPrefs.GetString("CharacterName")); // Find by player's CharacterName from PlayerPrefs.
        mainCam = GameObject.FindGameObjectWithTag("MainCamera"); // Find first tag matching "MainCamera".
    }
    #endregion

    // Where we check input to shoot a raycast and run all of our Interaction things.
    #region void Update - Execute Interaction Properties
    public void Interaction()
    {
        // Cast a Ray(cast)¹.
        // Set the Ray's origin to the centre² of the screen from the Main Camera.
        // Get back info on what it hit.
        Ray interact;
        interact = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hitInfo;

        // If the Raycast hits anything that's within the Ray's max range (ten units)...
        if (Physics.Raycast(interact, out hitInfo, 10.0f))
        {
            #region NPC Dialogue
            // If the Raycast hits an NPC...
            if (hitInfo.collider.CompareTag("NPC"))
            {
                // Check if the NPC has a 'Dialogue' script to interact with.
                Dialogue dlg = hitInfo.transform.GetComponent<Dialogue>();
                // If Dialogue is NOT found to be empty (there is Dialogue to interact with)...
                if (dlg != null)
                {
                    // Open the dialogue window.
                    dlg.showDlg = true;
                    // Disable (lock) all of the player's movement scripts.
                    player.GetComponent<CharacterMovement>().enabled = false;
                    player.GetComponent<MouseLook>().enabled = false;
                    mainCam.GetComponent<MouseLook>().enabled = false;
                    // Unlock and reveal our cursor on the screen.
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            #endregion
            #region Item
            // If the Raycast hits an Item...
            if (hitInfo.collider.CompareTag("Item"))
            {
                // Check if the Item has a 'ItemHandler' script to interact with.
                ItemHandler handler = hitInfo.transform.GetComponent<ItemHandler>();
                // If ItemHandler is NOT found to be emp- wow, that statement sounds REALLY stupid in English, doesn't it?
                if (handler != null)
                {
                    // Erm... Regardless: We execute the OnCollection() function.
                    handler.OnCollection();
                }
            }
            #endregion

            // ¹ This doesn't tell the WHERE to cast the Ray; it just does it.
            // ² 'Screen.(width or height) / 2' = Half of the total Screen dimension(s) in pixels (1920×1080 → 960×540 (the centre)).
        }
    }
    #endregion
}
