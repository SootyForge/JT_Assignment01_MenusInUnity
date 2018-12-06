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

    // 
    public void UpdateController()
    {
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }

    public void Move(float inputH, float inputV)
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(inputH, 0, inputV);

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;
        }
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
    }
}
