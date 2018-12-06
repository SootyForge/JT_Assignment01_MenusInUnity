using UnityEngine;
using System.Collections;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Stats")]
public class CharacterHandler : MonoBehaviour
{
    #region Variables
    [Header("Character")]
    public bool alive;
    public CharacterController controller;

    [Header("===VITALS===")]
    [Space(-10)]
    [Header("Health")]
    public float maxHealth;
    public float curHealth;
    public GUIStyle healthBar;

    [Header("Mana")]
    public float maxMana;
    public float curMana;
    public GUIStyle manaBar;

    [Header("Stamina")]
    public float maxStamina;
    public float curStamina;
    public GUIStyle staminaBar;

    [Header("===STATS & LEVELING===")]
    [Space(-10)]
    [Header("Stats and Class")]
    public string[] statsName = new string[6];
    public int[] stats = new int[6];
    /// Garbage.
    /// // I wasn't using arrays before.
    /// public int statSTR;
    /// public int statDEX, statCON, statINT, statWIS, statCHA;
    public CharacterClass charClass;

    [Header("Levels and Exp")]
    public int level;
    public int maxExp, curExp;

    [Header("Connection")]
    public RenderTexture miniMap;
    #endregion

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        /// Garbage.
        /// // See the 'Garbage.' comment in 'Variables'.
        /// statSTR = PlayerPrefs.GetInt("Strength", 10);
        /// statDEX = PlayerPrefs.GetInt("Dexterity", 10);
        /// statCON = PlayerPrefs.GetInt("Constitution", 10);
        /// statINT = PlayerPrefs.GetInt("Wisdom", 10);
        /// statWIS = PlayerPrefs.GetInt("Intelligence", 10);
        /// statCHA = PlayerPrefs.GetInt("Charisma", 10);
        statsName = new string[] { "Strength", "Dexterity", "Constitution", "Wisdom", "Intelligence", "Charisma" };
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = PlayerPrefs.GetInt(statsName[i]);
        }
        charClass = (CharacterClass)System.Enum.Parse(typeof(CharacterClass), PlayerPrefs.GetString("CharacterClass", "Barbarian"));

        maxHealth = 100f + (stats[3] * 5f);
        curHealth = maxHealth;

        alive = true;

        maxMana = 100f + (stats[4] * 5f);
        curMana = maxMana;

        maxStamina = 100f + (stats[2] * 5f);
        curStamina = maxStamina;

        maxExp = 60;

        controller = GetComponent<CharacterController>();
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (curExp >= maxExp)
        {
            curExp -= maxExp;
            level++;
            maxExp += 50;
        }
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        if (curHealth < 0 || !alive)
        {
            curHealth = 0;
            Debug.Log("ZOMBIE!");
        }

        if (alive && curHealth == 0)
        {
            alive = false;
            controller.enabled = false;
            Debug.Log("Controls disabled.");
        }

        if (curMana > maxMana)
        {
            curMana = maxMana;
        }

        if (curMana < 0)
        {
            curMana = 0;
        }

        if (curStamina > maxStamina)
        {
            curStamina = maxStamina;
        }

        if (curStamina < 0)
        {
            curStamina = 0;
        }
    }

    // OnGUI is called for rendering and handling GUI events
    private void OnGUI()
    {
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;

        if (!Pause.paused)
        {
            GUI.Box(new Rect(6f * scrW, 0.25f * scrH, 4f * scrW, 0.5f * scrH), "");
            GUI.Box(new Rect(6f * scrW, 0.25f * scrH, curHealth * (4f * scrW) / maxHealth, 0.5f * scrH), "", healthBar);

            GUI.Box(new Rect(6f * scrW, 0.75f * scrH, 4f * scrW, 0.5f * scrH), "");
            GUI.Box(new Rect(6f * scrW, 0.75f * scrH, curMana * (4f * scrW) / maxMana, 0.5f * scrH), "", manaBar);

            GUI.Box(new Rect(6f * scrW, 1.25f * scrH, 4f * scrW, 0.5f * scrH), "");
            GUI.Box(new Rect(6f * scrW, 1.25f * scrH, curStamina * (4f * scrW) / maxStamina, 0.5f * scrH), "", staminaBar);

            GUI.Box(new Rect(6f * scrW, 1.75f * scrH, 4f * scrW, 0.25f * scrH), "");
            GUI.Box(new Rect(6f * scrW, 1.75f * scrH, curExp * (4f * scrW) / maxExp, 0.25f * scrH), "");

            GUI.DrawTexture(new Rect(13.75f * scrW, 0.25f * scrH, 2f * scrW, 2f * scrH), miniMap);
        }
    }

}
