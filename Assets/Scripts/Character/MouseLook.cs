using UnityEngine;
using System.Collections;

[AddComponentMenu("JT_Assignment01/Player Scripts/Player Camera")]
public class MouseLook : MonoBehaviour
{
    #region Variables

    [Header("Rotational Axis")]
    public RotationalAxis axis = RotationalAxis.MouseX;

    [Header("Sensitivity")]
    public float sensitivityX = 10f;
    public float sensitivityY = 10f;

    [Header("Y Rotation Clamp")]
    public float minimumY = -60f;
    public float maximumY = 60f;

    float rotationY = 0f;

    #endregion
    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (axis == RotationalAxis.MouseXandY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * Time.timeScale;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.timeScale;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        else if (axis == RotationalAxis.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX * Time.timeScale, 0);
        }

        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.timeScale;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }
    }
}

public enum RotationalAxis
{
    MouseXandY,
    MouseX,
    MouseY
}