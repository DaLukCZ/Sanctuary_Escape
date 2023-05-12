using UnityEngine;

public class Medkit : Interactable
{
    [SerializeField]
    private float healAmount = 50f;
    [SerializeField]
    private PlayerHealth playerHealth;

    protected override void Interact()
    {
        // Heal the player
        playerHealth.Heal(healAmount);

        // Disable the medkit object
        gameObject.SetActive(false);
    }
}
