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

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.white;
        InvokeRepeating("ChangeColor", 0.5f, 0.5f);
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
    }

    protected override void Interact()
    {
        if (exploding)
        {
            return;
        }

        // Accelerate rotation speed and color changing for 6 seconds
        StartCoroutine(AccelerateRotationAndColor());

        if (colorChanges < 10)
        {
            // Change the color of the cube
            ChangeColor();
        }
        else
        {
            // Start the explosion
            exploding = true;
            StartCoroutine(Explode());
        }
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

    IEnumerator AccelerateRotationAndColor()
    {
        float initialRotationSpeed = rotationSpeed;
        float accelerationTime = 6f;
        float elapsedTime = 0f;

        while (elapsedTime < accelerationTime)
        {
            // Accelerate the rotation speed and color changing
            rotationSpeed += Time.deltaTime * 100f;
            ChangeColor();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the rotation speed to its initial value
        rotationSpeed = initialRotationSpeed;
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    IEnumerator ActivateSurprise()
    {
        // Make the cube spin faster
        float originalRotationSpeed = rotationSpeed;
        Vector3 originalScale = transform.localScale;
        float surpriseDuration = 10f;
        float elapsedTime = 0f;

        while (elapsedTime < surpriseDuration)
        {
            // Change the rotation speed randomly
            float newRotationSpeed = originalRotationSpeed + Random.Range(-50f, 50f);
            rotationSpeed = Mathf.Clamp(newRotationSpeed, 0f, 200f);

            // Change the scale of the cube periodically
            float sizeMultiplier = Mathf.PingPong(elapsedTime * 2f, 1f) * 2f;
            transform.localScale = originalScale * sizeMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Play a surprise sound and reset the cube
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        rotationSpeed = originalRotationSpeed;
        transform.localScale = originalScale;
    }
}
