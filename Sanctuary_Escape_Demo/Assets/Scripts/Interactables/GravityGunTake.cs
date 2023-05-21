using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGunTake : Interactable
{
    public GravityGun gravityGunScript;
    public GameObject gravityGunObject;
    protected override void Interact()
    {
        if (gravityGunScript != null && gravityGunObject != null)
        {
            gravityGunObject.gameObject.SetActive(true); // Hide the gravity gun
            gravityGunScript.isGravityGunPickedUp = true; // Set the picked up flag to true
            Destroy(transform.gameObject);
        }
    }

}
