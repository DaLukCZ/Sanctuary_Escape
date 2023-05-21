using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float staminaRecoveryRate = 1f;
    public float maxStamina = 100f;
    private float currentStamina;
    private bool isSprinting;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public float jumpCooldown;
    private bool readyToJump;
    public float airMultiplier;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool isGrounded;

    [Header("Orientation")]
    public Transform orientation;

    [Header("StaminaBar")]
    public float chipSpeed = 2f;
    public Image frontStaminaBar;
    private float lerpTimer;
    public Image backStaminaBar;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private float staminaTimer;
    private float sprintDuration;
    private float fixedTimeStep = 0.1f;

    private Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Air
    }

    private bool sprintingReady; // Indicates if the player is ready to sprint
    private float sprintCooldownTimer; // Timer to track the delay before stamina starts recharging

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        currentStamina = maxStamina;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        MyInput();
        SpeedControl();
        StateHandler();

        if (isGrounded)
            rb.drag = 5f;
        else
            rb.drag = 0f;

        HandleStamina();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(jumpKey) && isGrounded && readyToJump)
        {
            readyToJump = false;
            HandleJump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            state = MovementState.Crouching;
            movementSpeed = crouchSpeed;
        }
        else if (Input.GetKey(sprintKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            state = MovementState.Sprinting;
            movementSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            state = MovementState.Walking;
            movementSpeed = walkSpeed;
            isSprinting = false;
        }
    }

    private void StateHandler()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            state = MovementState.Crouching;
            movementSpeed = crouchSpeed;
        }
        else if (isGrounded)
        {
            if ((Input.GetKeyDown(sprintKey) || (Input.GetKeyDown(jumpKey) && isSprinting)) && currentStamina > 0.01f)
            {
                state = MovementState.Sprinting;
                movementSpeed = sprintSpeed;
                isSprinting = true;
                sprintCooldownTimer = 0f; // Reset the cooldown timer when sprinting starts
            }
            else
            {
                state = MovementState.Walking;
                movementSpeed = walkSpeed;
                isSprinting = false;
            }
        }
        else
        {
            state = MovementState.Air;
        }
    }

    private void HandleMovement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void HandleJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            if (currentStamina > 0)
            {
                currentStamina -= Time.deltaTime * 10f; // Stamina decreases by 10 per second of sprinting
                sprintDuration += Time.deltaTime;
                if (sprintDuration >= 10f) // Limit the sprinting duration to 10 seconds
                {
                    currentStamina = 0f;
                    isSprinting = false;
                    sprintDuration = 0f;
                }
            }
            else
            {
                currentStamina = 0f;
                isSprinting = false;
                sprintDuration = 0f;
            }
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                staminaTimer += Time.deltaTime;
                if (staminaTimer >= 2f) // Stamina starts recovering after 2 seconds of not sprinting
                {
                    currentStamina += 20f * Time.deltaTime; // Stamina recovers at a rate of 20 per second
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                }
            }
            else
            {
                staminaTimer = 0f;
            }
        }

        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        float sFraction = currentStamina / maxStamina;
        Debug.Log(currentStamina);
        frontStaminaBar.fillAmount = sFraction;
        backStaminaBar.fillAmount = sFraction;
    }
}
