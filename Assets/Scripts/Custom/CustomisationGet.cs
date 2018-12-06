using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("JT_Assignment01/Customization Scripts/Load Custom")]
public class CustomisationGet : MonoBehaviour
{
    [Header("Character")]
    public Renderer charMesh;
    public CharacterHandler charH;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        charMesh = GameObject.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        LoadTexture();
    }

    void LoadTexture()
    {
        if (!PlayerPrefs.HasKey("CharacterName"))
        {
            SceneManager.LoadScene(1);
        }

        SetTexture("Skin", PlayerPrefs.GetInt("SkinIndex"));
        SetTexture("Mouth", PlayerPrefs.GetInt("MouthIndex"));
        SetTexture("Eyes", PlayerPrefs.GetInt("EyesIndex"));
        SetTexture("Hair", PlayerPrefs.GetInt("HairIndex"));
        SetTexture("Clothes", PlayerPrefs.GetInt("ClothesIndex"));
        SetTexture("Armour", PlayerPrefs.GetInt("ArmourIndex"));

        gameObject.name = PlayerPrefs.GetString("CharacterName");
    }

    void SetTexture(string type, int dir)
    {
        Texture2D tex = null;
        int matIndex = 0;

        switch (type)
        {
            case "Skin":
                tex = Resources.Load("Character/Skin_" + dir.ToString()) as Texture2D;
                matIndex = 1;
                break;
            
            case "Mouth":
                tex = Resources.Load("Character/Mouth_" + dir.ToString()) as Texture2D;
                matIndex = 2;
                break;
            
            case "Eyes":
                tex = Resources.Load("Character/Eyes_" + dir.ToString()) as Texture2D;
                matIndex = 3;
                break;
            
            case "Hair":
                tex = Resources.Load("Character/Hair_" + dir.ToString()) as Texture2D;
                matIndex = 4;
                break;
            
            case "Clothes":
                tex = Resources.Load("Character/Clothes_" + dir.ToString()) as Texture2D;
                matIndex = 5;
                break;
            
            case "Armour":
                tex = Resources.Load("Character/Armour_" + dir.ToString()) as Texture2D;
                matIndex = 6;
                break;
        }

        Material[] mats = charMesh.materials;
        mats[matIndex].mainTexture = tex;
        charMesh.materials = mats;
    }
}
