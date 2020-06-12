using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Sensisivity")]
    public float sensX;
    public float sensY;

    [Header("Clamping")]
    public float minY;
    public float maxY;
    [Header("Spectator")]
    public float spectatormoveSpeed;
    private float rotX;
    private float rotY;

    [SerializeField]
    private bool isSpectator;

    private void Start()
    {
        // lock the cursor to the middle the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void LateUpdate()
    {// get the mouse movment inputs
        rotX += Input.GetAxis("Mouse X") * sensX;
        rotY += Input.GetAxis("Mouse Y") * sensY;

        // clamp the vertical rotation
        rotY = Mathf.Clamp(rotY, minY, maxY);
        //angle to look up and down camera
        if(isSpectator)
        {
            //!rotate cam ver
            transform.rotation = Quaternion.Euler(-rotY, rotX, 0);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float y = 0;
            if (Input.GetKey(KeyCode.E))
                y = 1;
            else if (Input.GetKey(KeyCode.Q))
                y = -1;

            Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
            // move cam by vector
            transform.position += dir * spectatormoveSpeed * Time.deltaTime;
        }
        else
        {
            // rorate camera verticlly
            transform.localRotation = Quaternion.Euler(-rotY, 0, 0);
            // roatet palyer horizontally
            transform.parent.rotation = Quaternion.Euler(0, rotX, 0);
        }
    }

}
