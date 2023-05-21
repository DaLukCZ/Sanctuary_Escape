using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scale : MonoBehaviour
{
    float forceToMass;

    public float combinedForce;
    public float calculatedMass;
    public ScaleNumberUI scaleNumberUI;
    public int registeredRigidbodies;

    Dictionary<Rigidbody, float> impulsePerRigidBody = new Dictionary<Rigidbody, float>();

    float currentDeltaTime;
    float lastDeltaTime;

    public GameObject doors;

    private bool isPlayerOnScale = false;

    private void Awake()
    {
        forceToMass = 1f / Physics.gravity.magnitude;
    }

    private void FixedUpdate()
    {
        lastDeltaTime = currentDeltaTime;
        currentDeltaTime = Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerOnScale = true;
            }

            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
                impulsePerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;
            else
                impulsePerRigidBody.Add(collision.rigidbody, collision.impulse.y / lastDeltaTime);

            UpdateWeight();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
                impulsePerRigidBody[collision.rigidbody] = collision.impulse.y / lastDeltaTime;
            else
                impulsePerRigidBody.Add(collision.rigidbody, collision.impulse.y / lastDeltaTime);

            UpdateWeight();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerOnScale = false;
            }

            impulsePerRigidBody.Remove(collision.rigidbody);
            UpdateWeight();
        }
    }

    void UpdateWeight()
    {
        registeredRigidbodies = impulsePerRigidBody.Count;
        combinedForce = 0;

        foreach (var force in impulsePerRigidBody.Values)
        {
            combinedForce += force;
        }

        calculatedMass = (float)(combinedForce * forceToMass);
        if (scaleNumberUI != null)
        {
            float totalMass = calculatedMass;

            // Check if the player is on the scale
            if (isPlayerOnScale)
            {
                totalMass += 68f;
            }

            scaleNumberUI.UpdateScaleNumber(totalMass);
            if (totalMass >= 100f)
            {
                Destroy(doors);
            }
        }
    }
}
