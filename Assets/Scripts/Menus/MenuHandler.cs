using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement; //1 For carrying information over between scene swaps.
using UnityEngine.UI; //1 For enabling interaction with Unity's canvas GUI elements.
using UnityEngine.EventSystems; //1 For enabling interaction with Unity's UI EventSystem (in all its glory...!~).

[AddComponentMenu("JT_Assignment01/Menu Scripts/Main Menu")]
public class MenuHandler : MonoBehaviour
{
    //1 regions! Because they help avoid sad programmers.
    #region Variables
    //1 Header(s)! Because they too help avoid sad programmers (in all seriousness: organizing stuff is good).
    [Header("Options")]
    //1 Toggle options menu on and off.
    public bool showOptions;

    //5 New toggle made for the 'tab'-based options menu design.
    public bool showKeybinds;

    //2 Make an array of all compatible screen resolutions for a 16:9 aspect ratio.
    //2 Current value of chosen resolution setting (pulled from resDropDown).
    //2 "Is the game displayed in fullscreen?" (required when setting resolution (Screen.SetResolution)).
    public Vector2[] res = new Vector2[7];
    public int resIndex;
    public bool isFullscreen;

    //2 Individual field for every control input KeyCode (required to save/load keybindings).
    [Header("Keys")]
    public KeyCode holdingKey;
    public KeyCode forward, backward, left, right, jump, crouch, sprint, interact, inventory, skills;

    [Header("References")]
    //2 Reference to your main AudioSource.
    //2 Reference your scene's directional Light.
    //2 Reference resolution Dropdown menu (this is where resIndex gets its value from).
    //2 Reference sliders (options menu).
    public AudioSource mainAudio;
    public Light dirLight;
    public Dropdown resDropDown;
    public Slider volSlider, brightSlider, ambLightSlider;
    
    //1 Grab the Main Menu and Options Menu from the scene.
    public GameObject mainMenu, optionsMenu;

    //5 Grab these options menu tabs for toggling
    public GameObject optionsGeneral, optionsKeybinds;

    [Header("KeyBind References")]
    //3 Make a Text placeholder for every control input (control input stuff).
    public Text forwardText;
    public Text backwardText, leftText, rightText, jumpText, crouchText, sprintText, interactText, inventoryText, skillsText;

    #endregion

    //1 Amusing Fact: I almost decided to wrap every function in its own region; then I realized how crazy that would be...
    #region Functions 'n' Methods
    //1 I still did it. Kind of. Just more reasonably...

    #region Start
    //2 Start is called just before any of the Update methods is called the first time
    void Start()
    {
        //2 Get relevant component(s) to 'mainAudio' and 'dirLight' (allows internal values to be linked to sliders later).
        mainAudio = GameObject.Find("MainMusic").GetComponent<AudioSource>();
        dirLight = GameObject.Find("Directional Light").GetComponent<Light>();

        //2 Try to load in KeyCode(s) stored in the System's save file, otherwise fallback to PlayerPrefs for default keys.
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "LeftControl"));
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "E"));
        inventory = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inventory", "Tab"));
        skills = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Skills", "K"));

        //4(?) Make the currently assigned KeyCode display on the keybind buttons (otherwise it'll be blank on start).
        forwardText.text = forward.ToString();
        backwardText.text = backward.ToString();
        leftText.text = left.ToString();
        rightText.text = right.ToString();
        jumpText.text = jump.ToString();
        crouchText.text = crouch.ToString();
        sprintText.text = sprint.ToString();
        interactText.text = interact.ToString();
        inventoryText.text = inventory.ToString();
        skillsText.text = skills.ToString();

        //4(?) Loading other saved settings.
        mainAudio.volume = PlayerPrefs.GetFloat("Volume", mainAudio.volume);
        dirLight.intensity = PlayerPrefs.GetFloat("Light", dirLight.intensity);
        RenderSettings.ambientIntensity = PlayerPrefs.GetFloat("Ambient", RenderSettings.ambientIntensity);
    }
    #endregion

    #region Main Menu Stuff
    //1 Each of these functions are connected to the relevant canvas buttons to make the canvas UI actually functional.
    #region Load Game
    //4 Method to load game scene.
    public void PlayGame()
    {
        //4 Mhmm.
        SceneManager.LoadScene(1);
    }
    #endregion
    /// Garbage.
    /// // This used to be a 'continue' button, but found it to be out of place from the final intent.
    /// #region Load Game
    /// //1 Method to load game scene.
    /// public void LoadGame()
    /// {
    ///     //1 See?
    ///     SceneManager.LoadScene(2);
    /// }
    /// #endregion
    #region Quit Game
    //4 Method to exit game (return to main menu).
    public void QuitGame()
    {
        CharacterHandler saveStat = FindObjectOfType<CharacterHandler>();
        PlayerPrefs.SetInt("CharacterLevel", saveStat.level);
        //4 I mean, it works.
        SceneManager.LoadScene(0);
    }
    #endregion
    #region Exit Execultable
    //1 Method to exit game ('Alt + F4' is what... turkey's... do?).
    public void ExitExe()
    {
        //1 Quit Application/Editor's play mode.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    #endregion
    #region Toggle Options
    //1 Method to toggle options menu (acts upon the bool below).
    public void ToggleOptions()
    {
        OptionToggle();
    }
    #endregion
    #region Options Toggle
    //1 bool to return conditions on when to display or hide option's menu (and do a few other things).
    bool OptionToggle()
    {
        //1 if 'showOptions' is true... close the options menu and return to the main menu.
        //5 Oh, and set the options menu back to the General tab (to prevent a null reference in the else state).
        if (showOptions)
        {
            showOptions = false;
            showKeybinds = false;
            optionsGeneral.SetActive(true);
            optionsKeybinds.SetActive(false);
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            return true;
        }
        //1 otherwise (false)... open the options menu and hide the main menu (and do those other things we talked about).
        else
        {
            showOptions = true;
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);

            //2 Now for those other things:
            //2 Upon opening the options menu...
            //2 Get each relevant canvas GUI component from their respective GameObject.
            //2 Set each slider position to their respective control element's value (prevents spooky spikes upon adjustment).
            volSlider = GameObject.Find("Slider (Volume)").GetComponent<Slider>();
            volSlider.value = mainAudio.volume;

            brightSlider = GameObject.Find("Slider (Brightness)").GetComponent<Slider>();
            brightSlider.value = dirLight.intensity;

            ambLightSlider = GameObject.Find("Slider (Ambient Light)").GetComponent<Slider>();
            ambLightSlider.value = RenderSettings.ambientIntensity;

            resDropDown = GameObject.Find("Dropdown (Resolution)").GetComponent<Dropdown>();

            return false;
        }
    }
    #endregion
    //5 Method to toggle Keybinds tab (acts upon the bool set below).
    public void ToggleKeybinds()
    {
        KeybindToggle();
    }

    //5 A bool that's almost identical to 'OptionToggle()' (toggles between the 'General' tab and the 'Keybinds' tab.
    bool KeybindToggle()
    {
        if (showKeybinds)
        {
            showKeybinds = false;
            optionsGeneral.SetActive(true);
            optionsKeybinds.SetActive(false);
            return true;
        }
        else
        {
            showKeybinds = true;
            optionsGeneral.SetActive(false);
            optionsKeybinds.SetActive(true);
            return false;
        }
    }

    #endregion

    #region Options Menu Stuff
    //2 All of these Methods are assigned to their respective Unity canvas elements and are... hmm... déjà vu...
    #region +void Volume() - Volume Slider
    //2 Method to control mainAudio volume via the options menu volSlider.
    public void Volume()
    {
        mainAudio.volume = volSlider.value;
    }
    #endregion
    #region +void Brightness() - Directional Light Slider
    //2 Method to control dirLight brightness (intesity) via the brightSlider.
    public void Brightness()
    {
        dirLight.intensity = brightSlider.value;
    }
    #endregion
    #region +void Ambient() - Ambient Light Slider
    //2 Method to control ambient light intensity (ambientIntensity) via the ambLightSlider.
    public void Ambient()
    {
        RenderSettings.ambientIntensity = ambLightSlider.value;
    }
    #endregion
    #region +void Resolution() - Resolution Dropdown
    //2 Method to change the screen resolution via the resDropDown.
    public void Resolution()
    {
        resIndex = resDropDown.value;
        Screen.SetResolution((int)res[resIndex].x, (int)res[resIndex].y, isFullscreen);
    }
    #endregion
    #region +void Save() - Save Button
    //2 Method to save all KeyCodes to strings in the PlayerPrefs.
    public void Save()
    {
        PlayerPrefs.SetString("Forward", forward.ToString());
        PlayerPrefs.SetString("Backward", backward.ToString());
        PlayerPrefs.SetString("Left", left.ToString());
        PlayerPrefs.SetString("Right", right.ToString());
        PlayerPrefs.SetString("Jump", jump.ToString());
        PlayerPrefs.SetString("Crouch", crouch.ToString());
        PlayerPrefs.SetString("Sprint", sprint.ToString());
        PlayerPrefs.SetString("Interact", interact.ToString());

        //4(?) Saving other changed settings
        PlayerPrefs.SetFloat("Volume", mainAudio.volume);
        PlayerPrefs.SetFloat("Light", dirLight.intensity);
        PlayerPrefs.SetFloat("Ambient", RenderSettings.ambientIntensity);
    }
    #endregion
    #endregion

    /// Garbage (old heading).
    /// //3 Where the OnGUI() events are handled for switching and rebinding KeyCodes.
    //3 Where we interact with the GUI to setup our KeyCodes
    #region -void OnGUI() - KeyCode Setup
    //3 OnGUI is called for rendering and handling GUI events
    private void OnGUI()
    {
        #region Keycodes
        //3 Get the current Event being processed right now.
        Event e = Event.current;

        #region Forward
        //3 "if 'forward' has NO KeyCode BOUND..."
        if (forward == KeyCode.None)
        {
            //3 "... Log a warning in the console until a new keyCode is received."
            Debug.Log("KeyCode: " + e.keyCode);

            //3 "if the current Event keyCode is NOT empty..." (or: "if I input a new KeyCode as the current Event...")
            if (e.keyCode != KeyCode.None)
            {
                //3 "... if the current keyCode Event is NOT already in use by any of these other controls..."
                if (!(e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    //3 "... set 'forward' to its new keyCode, empty holdingKey, and display the new key as a string."
                    forward = e.keyCode;
                    holdingKey = KeyCode.None;
                    forwardText.text = forward.ToString();
                }
                //3 "else..." (or: "if the new KeyCode you're trying to set IS already in use by another control...")
                else
                {
                    //3 "... do nothing/keep waiting until a free/available KeyCode is pressed." (Prevents double-binds.)
                    forward = holdingKey;
                    holdingKey = KeyCode.None;
                    forwardText.text = forward.ToString();
                }
            }
        }
        #endregion
        #region Backward
        //3 No; I am not going to repeat all of that. You're welcome.
        if (backward == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    backward = e.keyCode;
                    holdingKey = KeyCode.None;
                    backwardText.text = backward.ToString();
                }
                else
                {
                    backward = holdingKey;
                    holdingKey = KeyCode.None;
                    backwardText.text = backward.ToString();
                }
            }
        }
        #endregion
        #region Left
        if (left == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    left = e.keyCode;
                    holdingKey = KeyCode.None;
                    leftText.text = left.ToString();
                }
                else
                {
                    left = holdingKey;
                    holdingKey = KeyCode.None;
                    leftText.text = left.ToString();
                }
            }
        }
        #endregion
        #region Right
        if (right == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    right = e.keyCode;
                    holdingKey = KeyCode.None;
                    rightText.text = right.ToString();
                }
                else
                {
                    right = holdingKey;
                    holdingKey = KeyCode.None;
                    rightText.text = right.ToString();
                }
            }
        }
        #endregion
        #region Jump
        if (jump == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    jump = e.keyCode;
                    holdingKey = KeyCode.None;
                    jumpText.text = jump.ToString();
                }
                else
                {
                    jump = holdingKey;
                    holdingKey = KeyCode.None;
                    jumpText.text = jump.ToString();
                }
            }
        }
        #endregion
        #region Crouch
        if (crouch == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    crouch = e.keyCode;
                    holdingKey = KeyCode.None;
                    crouchText.text = crouch.ToString();
                }
                else
                {
                    crouch = holdingKey;
                    holdingKey = KeyCode.None;
                    crouchText.text = crouch.ToString();
                }
            }
        }
        #endregion
        #region Sprint
        if (sprint == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == interact || e.keyCode == inventory || e.keyCode == skills))
                {
                    sprint = e.keyCode;
                    holdingKey = KeyCode.None;
                    sprintText.text = sprint.ToString();
                }
                else
                {
                    sprint = holdingKey;
                    holdingKey = KeyCode.None;
                    sprintText.text = sprint.ToString();
                }
            }
        }
        #endregion
        #region Interact
        if (interact == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == inventory || e.keyCode == skills))
                {
                    interact = e.keyCode;
                    holdingKey = KeyCode.None;
                    interactText.text = interact.ToString();
                }
                else
                {
                    interact = holdingKey;
                    holdingKey = KeyCode.None;
                    interactText.text = interact.ToString();
                }
            }
        }
        #endregion
        #region Inventory
        if (inventory == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == skills))
                {
                    inventory = e.keyCode;
                    holdingKey = KeyCode.None;
                    inventoryText.text = inventory.ToString();
                }
                else
                {
                    inventory = holdingKey;
                    holdingKey = KeyCode.None;
                    inventoryText.text = inventory.ToString();
                }
            }
        }
        #endregion
        #region Skills
        if (skills == KeyCode.None)
        {
            Debug.Log("KeyCode: " + e.keyCode);

            if (e.keyCode != KeyCode.None)
            {
                if (!(e.keyCode == forward || e.keyCode == backward || e.keyCode == left || e.keyCode == right || e.keyCode == jump || e.keyCode == crouch || e.keyCode == sprint || e.keyCode == interact || e.keyCode == inventory))
                {
                    skills = e.keyCode;
                    holdingKey = KeyCode.None;
                    skillsText.text = skills.ToString();
                }
                else
                {
                    skills = holdingKey;
                    holdingKey = KeyCode.None;
                    skillsText.text = skills.ToString();
                }
            }
        }
        #endregion
        #endregion
    }

    //3 I couldn't think of a name for this. Everything here is what makes the canvas keybind buttons functional.
    #region Canvas-KeyCode Interact-Link
    //3 Each Key Function uses the naming convention 'Key*Action' to organize it when implementing to Canvas element.
    #region +void KeyAForward()
    public void KeyAForward()
    {
        if (!(backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = forward;
            forward = KeyCode.None;
            forwardText.text = forward.ToString();
        }
    }
    #endregion
    #region +void KeyBBackward()
    public void KeyBBackward()
    {
        if (!(forward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = backward;
            backward = KeyCode.None;
            backwardText.text = backward.ToString();
        }
    }
    #endregion
    #region +void KeyCLeft()
    public void KeyCLeft()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = left;
            left = KeyCode.None;
            leftText.text = left.ToString();
        }
    }
    #endregion
    #region +void KeyDRight()
    public void KeyDRight()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = right;
            right = KeyCode.None;
            rightText.text = right.ToString();
        }
    }
    #endregion
    #region +void KeyEJump()
    public void KeyEJump()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = jump;
            jump = KeyCode.None;
            jumpText.text = jump.ToString();
        }
    }
    #endregion
    #region +void KeyFCrouch()
    public void KeyFCrouch()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = crouch;
            crouch = KeyCode.None;
            crouchText.text = crouch.ToString();
        }
    }
    #endregion
    #region +void KeyGSprint()
    public void KeyGSprint()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || interact == KeyCode.None))
        {
            holdingKey = sprint;
            sprint = KeyCode.None;
            sprintText.text = sprint.ToString();
        }
    }
    #endregion
    #region +void KeyHInteract()
    public void KeyHInteract()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None))
        {
            holdingKey = interact;
            interact = KeyCode.None;
            interactText.text = interact.ToString();
        }
    }
    #endregion
    #region +void KeyIInventory()
    public void KeyIInventory()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None || skills == KeyCode.None))
        {
            holdingKey = inventory;
            inventory = KeyCode.None;
            inventoryText.text = inventory.ToString();
        }
    }
    #endregion
    #region +void KeyJSkills()
    public void KeyJSkills()
    {
        if (!(forward == KeyCode.None || backward == KeyCode.None || left == KeyCode.None || right == KeyCode.None || jump == KeyCode.None || crouch == KeyCode.None || sprint == KeyCode.None || interact == KeyCode.None || inventory == KeyCode.None))
        {
            holdingKey = skills;
            skills = KeyCode.None;
            skillsText.text = skills.ToString();
        }
    }
    #endregion
    #endregion

    #endregion

    #endregion
}
