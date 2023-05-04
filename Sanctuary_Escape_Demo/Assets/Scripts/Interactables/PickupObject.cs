using UnityEngine;

public class PickupObject : MonoBehaviour
{ 
    public bool isGrabbable = true;
    public bool isBeingHeld = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Drop()
    {
        isBeingHeld = false;
        rb.isKinematic = false;
    }
}
