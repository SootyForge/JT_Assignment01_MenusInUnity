using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public MenuHandler key;
    public CharacterMovement controller;
    public Interact interact;
    public Inventory inv;
    public LevelUp skillWindow;

    private void Start()
    {
        key = FindObjectOfType<MenuHandler>();
        controller = FindObjectOfType<CharacterMovement>();
        interact = FindObjectOfType<Interact>();
        inv = FindObjectOfType<Inventory>();
        skillWindow = FindObjectOfType<LevelUp>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        // Ternary operator (courtesy of Manny); it's a new (fake) axis using the saved keys.
        float inputH = Input.GetKey(key.right) ? 1f : Input.GetKey(key.left) ? -1f : 0;
        float inputV = Input.GetKey(key.forward) ? 1f : Input.GetKey(key.backward) ? -1f : 0;

        // Execute 'CharacterMovement.Move()' from here.
        controller.Move(inputH, inputV);

        // Same as above, but for 'CharacterMovement.Jump()'.
        if (Input.GetKeyDown(key.jump))
        {
            controller.Jump();
        }

        controller.UpdateController();
        #endregion

        #region Interact
        if (Input.GetKeyDown(key.interact))
        {
            interact.Interaction();
        }
        #endregion

        #region Inventory
        if (Input.GetKeyDown(key.inventory))
        {
            inv.InventoryToggle();
        }
        #endregion

        #region Skills Window
        if (Input.GetKeyDown(key.skills))
        {
            skillWindow.SkillsToggle();
        } 
        #endregion
    }
}
