using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorCube : Interactable
{
    private MeshRenderer mesh;
    private System.Random rnd = new System.Random();
    private int colorChanges = 0;
    private bool exploding = false;
    private float rotationSpeed = 50f;
    private float sizeMultiplier = 1f;
    private float sizeChangeSpeed = 0.5f;
    private Vector3 rotationDirection = new Vector3(1f, 1f, 1f);
    private float surpriseTime = 10f;
    private bool surpriseActive = false;
    private float colorChangeInterval = 2f; // Change color every 2 seconds
    private float slowColorChangeSpeed = 0.1f; // Slow color change speed
    private float fastColorChangeSpeed = 0.5f; // Fast color change speed
    private bool isSpinning = false;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.white;
        InvokeRepeating("ChangeColor", colorChangeInterval, colorChangeInterval);
    }

    void Update()
    {
        if (exploding)
        {
            // Rotate the cube as it explodes
            transform.Rotate(Vector3.up, Time.deltaTime * 360);
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 2;
        }
        else
        {
            // Rotate the cube in all directions
            transform.Rotate(rotationDirection * Time.deltaTime * rotationSpeed);

            // Change the size of the cube from small to big and back to small
            if (sizeMultiplier >= 2f)
            {
                sizeChangeSpeed = -0.5f;
            }
            else if (sizeMultiplier <= 1f)
            {
                sizeChangeSpeed = 0.5f;
            }
            sizeMultiplier += Time.deltaTime * sizeChangeSpeed;
            transform.localScale = Vector3.one * sizeMultiplier;
        }

        // Activate the surprise after a certain time
        if (!surpriseActive && Time.timeSinceLevelLoad >= surpriseTime)
        {
            StartCoroutine(ActivateSurprise());
            surpriseActive = true;
        }

        // If it's spinning, increase the rotation speed and color change speed
        if (isSpinning)
        {
            rotationSpeed += Time.deltaTime * 100f;
            colorChangeInterval -= Time.deltaTime * 0.1f;
        }
    }

    protected override void Interact()
    {
        if (exploding)
        {
            return;
        }

        // Start spinning and increase the rotation and color change speed
        isSpinning = true;
        StartCoroutine(SlowDown());

        // Change the color of the cube
        ChangeColor();
    }

    void ChangeColor()
    {
        float r = rnd.Next(0, 256) / 255f;
        float g = rnd.Next(0, 256) / 255f;
        float b = rnd.Next(0, 256) / 255f;
        Color c = new Color(r, g, b);
        mesh.material.color = c;
        colorChanges++;
    }

    IEnumerator SlowDown()
    {
        yield return new WaitForSeconds(5);

        // Stop spinning and reset rotation and color change speed
        isSpinning = false;
        rotationSpeed = 50f;
        colorChangeInterval = 2f;
    }

    IEnumerator ActivateSurprise()
    {
        // Slow down color change speed
        float colorChangeDelay = 0.1f;

        // Make the cube spin faster
        float originalRotationSpeed = rotationSpeed;
        float surpriseDuration = 10f;
        float elapsedTime = 0f;

        // Gradually increase the color change delay
        float maxColorChangeDelay = 1f;
        float colorChangeDelayIncrement = (maxColorChangeDelay - colorChangeDelay) / (surpriseDuration / colorChangeDelay);
        float currentColorChangeDelay = colorChangeDelay;

        while (elapsedTime < surpriseDuration)
        {
            // Change the rotation speed randomly
            float newRotationSpeed = originalRotationSpeed + Random.Range(-100f, 100f);
            rotationSpeed = Mathf.Clamp(newRotationSpeed, 0f, 500f);

            // Gradually increase the color change delay
            if (currentColorChangeDelay < maxColorChangeDelay)
            {
                currentColorChangeDelay += colorChangeDelayIncrement * Time.deltaTime;
            }

            // Change the color of the cube
            if (Time.timeSinceLevelLoad % currentColorChangeDelay < Time.deltaTime)
            {
                ChangeColor();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Play a surprise sound
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Reset the rotation speed and color change delay
        rotationSpeed = originalRotationSpeed;
        currentColorChangeDelay = colorChangeDelay;

        // Gradually decrease the color change delay to its original value
        while (currentColorChangeDelay > colorChangeDelay)
        {
            currentColorChangeDelay -= colorChangeDelayIncrement * Time.deltaTime;
            yield return null;
        }

        // Set the color of the cube back to white
        mesh.material.color = Color.white;

        // Reset exploding flag and color change counter
        exploding = false;
        colorChanges = 0;
    }
}