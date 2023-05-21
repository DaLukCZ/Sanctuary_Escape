using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public float weakPointMultiplier = 2f;
    public float destructionDelay = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        BallProjectile ballProjectile = collision.gameObject.GetComponent<BallProjectile>();

        // Check if the collided object has a BallProjectile component and if it's a player ball
        if (ballProjectile != null && ballProjectile.isPlayerBall)
        {
            // Handle ball collision with the weak point
            Debug.Log("Ball hit the weak point!");

            // Get the EnemyHealth component from the parent object
            EnemyHealth enemyHealth = transform.parent.gameObject.GetComponent<EnemyHealth>();

            // Deal damage to the enemy using the weak point multiplier
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(ballProjectile.damageAmount * weakPointMultiplier);
            }

            Debug.Log(enemyHealth.currentHealth);

            // Destroy the ball with a delay
            Destroy(collision.gameObject, destructionDelay);
        }
    }
}
