using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool crouching;
    private bool sprinting;
    private bool lerpCrouch;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float crouchTimer = 0f;

    public float maxSprintTime = 10f;
    public float currentSprintTime;
    private float lerpTimer;
    public float chipSpeed = 2f;
    public Image frontStaminaBar;
    public Image backStaminaBar;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSprintTime = maxSprintTime;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        currentSprintTime = Mathf.Clamp(currentSprintTime, 0, maxSprintTime);
        UpdateHealthUI();

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

    }

    private void UpdateHealthUI()
    {
        float fillFront = frontStaminaBar.fillAmount;
        float fillBack = backStaminaBar.fillAmount;
        float sFraction = currentSprintTime / maxSprintTime;
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

    public void Relax()
    {
        lerpTimer = 10f;
        currentSprintTime++;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8;
            currentSprintTime--;
        }
        else
        {
            speed = 5;
            Relax();
        }
    }

    //dostane input pro InputManager
    public void ProcessMove(Vector2 input)
    {
        Vector3 movedirection = Vector3.zero;
        movedirection.x = input.x;
        movedirection.z = input.y;
        controller.Move(transform.TransformDirection(movedirection) * speed * Time.deltaTime);
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
}