using System.Collections;
using UnityEngine;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Movement")]
public class CharacterMovement : MonoBehaviour
{
    #region Variables

    [Range(0f, 10f)]
    public float speed = 6f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    private Vector3 moveDirection = Vector3.zero;

    public CharacterController controller;

    #endregion

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
