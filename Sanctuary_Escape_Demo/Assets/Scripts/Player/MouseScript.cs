using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform playerCamTransform;
    public Transform orientation;

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
        if (!isDead)
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

    public void SetCameraTilt(float tiltAngle)
    {
        playerCamTransform.rotation = Quaternion.Euler(0f, 0f, -tiltAngle);
        playerCamTransform.position = new Vector3(playerCamTransform.position.x, playerCamTransform.position.y - 1f, playerCamTransform.position.z);
    }

    public void SetDead(bool dead)
    {
        isDead = dead;

        if (isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            sensX = 0f;
            sensY = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        SetCameraTilt(45);
    }
}
