using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Transform mouth;
    public GameObject smallBallPrefab;
    public float rotationSpeed = 5f;
    public float shootingInterval = 2f;
    public float ballSpeed = 10f;
    public float destructionDelay = 2f;

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
            Vector3 direction = targetPositionObject.transform.position - transform.position;
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
            ballRigidbody.AddForce(direction.normalized * ballSpeed * 1.5f, ForceMode.Impulse);
        }
    }

    private void DestroyCloud()
    {
        // Destroy the cloud after the specified destruction delay
        Destroy(gameObject, destructionDelay);
    }
}
