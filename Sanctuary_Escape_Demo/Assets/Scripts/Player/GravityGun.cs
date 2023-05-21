using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float maxGrabDistance = 10f;
    [SerializeField] float throwForce = 20f;
    [SerializeField] float lerpSpeed = 20f;
    [SerializeField] Transform objectHolder;
    [SerializeField] float catchRadius = 3f;
    [SerializeField] Color catchRadiusColor = Color.red;

    Rigidbody grabbedRB;
    bool isLerping = false;
    public bool isGravityGunPickedUp = false;
    Vector3 previousMousePosition;

    void Update()
    {
        if (isGravityGunPickedUp)
        { 
            if (grabbedRB)
            {
                if (isLerping)
                {
                    grabbedRB.position = Vector3.Lerp(grabbedRB.position, objectHolder.position, Time.deltaTime * lerpSpeed);
                    if (Vector3.Distance(grabbedRB.position, objectHolder.position) < 0.01f)
                    {
                        isLerping = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    ThrowObject();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (grabbedRB)
                {
                    ReleaseObject();
                }
                else
                {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, catchRadius);
                    foreach (Collider collider in colliders)
                    {
                        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                        if (rb)
                        {
                            grabbedRB = rb;
                            grabbedRB.isKinematic = true;
                            grabbedRB.gameObject.layer = LayerMask.NameToLayer("GrabbedObject");

                            // Check if the grabbed object is a smallEnemyBall
                            BallProjectile smallEnemyBall = grabbedRB.gameObject.GetComponent<BallProjectile>();
                            if (smallEnemyBall != null && smallEnemyBall.isPlayerBall)
                            {
                                // Set the isPlayerBall property to true to indicate it's the player's ball
                                smallEnemyBall.isPlayerBall = true;
                            }

                            isLerping = true;

                            break; // Break out of the loop after grabbing the first object
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (grabbedRB && !isLerping)
        {
            grabbedRB.MovePosition(objectHolder.position);
        }
    }

    void ReleaseObject()
    {
        if (grabbedRB)
        {
            grabbedRB.isKinematic = false;
            grabbedRB.velocity = Vector3.zero;
            grabbedRB.angularVelocity = Vector3.zero;
            grabbedRB.gameObject.layer = LayerMask.NameToLayer("Default");
            grabbedRB = null;
            isLerping = false;
        }
    }

    void ThrowObject()
    {
        if (grabbedRB)
        {
            grabbedRB.isKinematic = false;

            // Perform raycast to check for obstacles
            Vector3 throwDirection = cam.transform.forward;
            Ray throwRay = new Ray(objectHolder.position, throwDirection);
            RaycastHit obstacleHit;
            if (Physics.Raycast(throwRay, out obstacleHit, maxGrabDistance))
            {
                float obstacleDistance = Vector3.Distance(objectHolder.position, obstacleHit.point);
                if (obstacleDistance < maxGrabDistance)
                {
                    // Adjust throw direction based on obstacle hit
                    throwDirection = obstacleHit.point - objectHolder.position;
                    throwDirection.Normalize();
                }
            }

            grabbedRB.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
            grabbedRB.gameObject.layer = LayerMask.NameToLayer("Default");

            // Check if the thrown object is a box and if it collides with an enemy
            BoxCollider boxCollider = grabbedRB.gameObject.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                Collider[] colliders = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, boxCollider.transform.rotation, LayerMask.GetMask("Enemy"));
                foreach (Collider collider in colliders)
                {
                    // Get the EnemyHealth component from the collided enemy
                    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        // Calculate the damage amount based on the object's speed and mass
                        float damageAmount = grabbedRB.velocity.magnitude * grabbedRB.mass;

                        // Deal damage to the enemy
                        enemyHealth.TakeDamage(damageAmount);
                        Debug.Log(enemyHealth.currentHealth);
                    }
                }
            }

            // Set the owner of the thrown ball
            BallProjectile ballProjectile = grabbedRB.gameObject.GetComponent<BallProjectile>();
            if (ballProjectile != null)
            {
                ballProjectile.isPlayerBall = true; // Set the owner to the player
            }

            grabbedRB = null;
            isLerping = false;
        }
    }

}

