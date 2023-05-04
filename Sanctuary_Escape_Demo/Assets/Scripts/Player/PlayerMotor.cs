using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    private float movementSpeed;
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
    public KeyCode spintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;

    public float groundDrag;

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

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

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
            rb.drag = groundDrag;
        else
            rb.drag = 0;
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
            state = MovementState.crouching;
            movementSpeed = crouchSpeed;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            state = isGrounded && Input.GetKey(spintKey) ? MovementState.sprinting : MovementState.walking;
            movementSpeed = isSprinting ? sprintSpeed : walkSpeed;
        }
    }

    private void StateHandler()
    {
        
        if (Input.GetKeyDown(crouchKey))
        {
            state = MovementState.crouching;
            movementSpeed = crouchSpeed;
        }

        if (isGrounded && Input.GetKeyDown(spintKey))
        {
            state = MovementState.sprinting;
            movementSpeed = sprintSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            movementSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void HandleMovement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(isGrounded)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);

        else if(!isGrounded)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > movementSpeed)
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
        if (currentStamina < maxStamina && !isSprinting)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }


    private void UpdateHealthUI()
    {
        float fillFront = frontStaminaBar.fillAmount;
        float fillBack = backStaminaBar.fillAmount;
        float sFraction = currentStamina / maxStamina;
        if (fillBack > sFraction)
        {
            frontStaminaBar.fillAmount = sFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backStaminaBar.fillAmount = Mathf.Lerp(fillBack, sFraction, percentComplete);
        }
        if (fillFront < sFraction)
        {
            backStaminaBar.fillAmount = sFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillFront, backStaminaBar.fillAmount, percentComplete);
        }
    }
}