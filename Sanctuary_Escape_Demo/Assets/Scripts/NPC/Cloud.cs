using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Transform mouth;
    public GameObject smallBallPrefab;
    public float rotationSpeed = 5f;
    public float shootingInterval = 2f;
    public float originalBallSpeed = 10f;
    private float ballSpeed;
    public float destructionDelay = 2f;
    public float maxViewRange = 10f;

    private float shootingTimer;
    public GameObject targetPositionObject;

    private void Update()
    {
        RotateCloudTowardsTarget();

        // Update shooting timer
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval + Random.Range(-0.5f, 0.5f))
        {
            Shoot();
            shootingTimer = 0f;
        }

        // Get the EnemyHealth component from the same object
        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

        // Check if the cloud's health has reached zero or below
        if (enemyHealth.currentHealth <= 0)
        {
            DestroyCloud();
        }
    }

    private void RotateCloudTowardsTarget()
    {
        if (targetPositionObject != null)
        {
            // Calculate direction towards the target position
            Vector3 direction = targetPositionObject.transform.position - transform.position;

            // Check if there is an obstacle between the cloud and the target
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, maxViewRange))
            {
                if (hit.collider.gameObject != targetPositionObject)
                {
                    // There is an obstacle, so don't rotate towards the target
                    return;
                }
            }

            // Rotate towards the target position
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        if (targetPositionObject != null)
        {
            // Calculate direction towards the target position
            Vector3 direction = targetPositionObject.transform.position - mouth.position;

            // Instantiate a small ball prefab and set its position to match the mouth
            GameObject ball = Instantiate(smallBallPrefab, mouth.position, Quaternion.identity);

            // Apply a stronger force to the ball towards the target position
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.mass = 0.5f;
            ballSpeed = originalBallSpeed + Random.Range(-2, 3);
            ballRigidbody.AddForce(direction.normalized * ballSpeed * 1.5f, ForceMode.Impulse);
        }
    }

    private void DestroyCloud()
    {
        // Destroy the cloud after the specified destruction delay
        Destroy(gameObject, destructionDelay);
    }
}
