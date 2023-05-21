using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    public float destructionDelay = 10f;
    public float damageAmount = 15f;
    public bool isPlayerBall = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayerBall)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Handle ball collision with an enemy as the player's ball
                Debug.Log("Player's ball hit the enemy!");

                // Get the EnemyHealth component from the collided object
                EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

                // Deal damage to the enemy
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                    Debug.Log(enemyHealth.currentHealth);
                }
                // Destroy the ball upon collision with the enemy
                Destroy(gameObject);
            }
            else
            {
                // Invoke destruction after a delay if the ball doesn't hit an enemy
                Invoke("DestroyBall", destructionDelay);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Handle ball collision with the player as an enemy's ball
                Debug.Log("Enemy's ball hit the player!");

                // Get the PlayerHealth component from the collided object
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

                // Deal damage to the player
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    Debug.Log(playerHealth.currentHealth);
                }

                // Destroy the ball upon collision with the player
                Destroy(gameObject);
            }
            else
            {
                // Invoke destruction after a delay if the ball doesn't hit the player
                Invoke("DestroyBall", destructionDelay);
            }
        }
    }

    private void DestroyBall()
    {
        // Destroy the ball if it doesn't hit the intended target within the specified delay
        Destroy(gameObject);
    }

}
