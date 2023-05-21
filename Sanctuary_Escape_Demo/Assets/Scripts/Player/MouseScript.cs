using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public CameraScript cameraScript;

    float xRotation;
    float yRotation;
    public float YRotation
    {
        get { return yRotation; }
        set { yRotation = value; }
    }

    bool isDead; // New variable to track player's death

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isDead = false;
    }

    void Update()
    {
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;
            yRotation += mouseX;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
