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

    Rigidbody grabbedRB;
    bool isLerping = false;
    Vector3 previousMousePosition;

    void Update()
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
                RaycastHit hit;
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                if (Physics.Raycast(ray, out hit, maxGrabDistance))
                {
                    if (Vector3.Distance(hit.collider.transform.position, transform.position) <= maxGrabDistance)
                    {
                        grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                        if (grabbedRB)
                        {
                            grabbedRB.isKinematic = true;
                            grabbedRB.gameObject.layer = LayerMask.NameToLayer("GrabbedObject");
                            isLerping = true;
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
            grabbedRB = null;
            isLerping = false;
        }
    }
}
