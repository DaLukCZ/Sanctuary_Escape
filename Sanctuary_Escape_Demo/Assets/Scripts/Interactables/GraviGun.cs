using UnityEngine;

public class GraviGun : MonoBehaviour
{
    public float maxDistance = 100f;
    public Camera cam;
    public Color rangeColor = Color.red;
    public float rangeSphereRadius = 1f;

    private bool pressed;
    private PickupObject heldObject;
    private Rigidbody heldObjectRb;
    private bool holdingObject = false;
    private InputManager inputManager;
    private SphereCollider rangeCollider;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();

        // Add a sphere collider to the gun for range visualization
        rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.radius = rangeSphereRadius;
        rangeCollider.isTrigger = true;
    }

    void Update()
    {
        if (inputManager.onFoot.Use.triggered)
        {
            if (holdingObject)
            {
                DropObject();
            }
            else
            {
                PickUpObject();
            }
        }

        if (holdingObject)
        {
            UpdateHeldObjectPosition();
        }
    }


    void PickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            PickupObject interactable = hit.collider.gameObject.GetComponent<PickupObject>();
            if (interactable != null && interactable.isGrabbable && !interactable.isBeingHeld)
            {
                heldObject = interactable;
                heldObject.isBeingHeld = true;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();
                heldObjectRb.useGravity = false;
                holdingObject = true;
            }
        }
    }

    void DropObject()
    {
        heldObjectRb.useGravity = true;
        heldObject.isBeingHeld = false;
        heldObject = null;
        heldObjectRb = null;
        holdingObject = false;
    }


    void UpdateHeldObjectPosition()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, 20.5f, transform.position.z) + transform.forward * 2f;
        Vector3 newPosition = Vector3.Lerp(heldObject.transform.position, targetPosition, Time.deltaTime * 10f);
        heldObjectRb.MovePosition(newPosition);
        heldObject.transform.LookAt(transform.position + transform.forward * 2f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere around the gun for range visualization
        if (rangeCollider != null)
        {
            Gizmos.color = rangeColor;
            Gizmos.DrawWireSphere(transform.position, rangeCollider.radius);
        }
    }
}
