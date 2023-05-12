using UnityEngine;

public class Poison : Interactable
{
    [SerializeField]
    private float dmgAmount = 40f;
    [SerializeField]
    private PlayerHealth playerHealth;

    protected override void Interact()
    {
        // Heal the player
        playerHealth.TakeDamage(dmgAmount);

        // Disable the medkit object
        //gameObject.SetActive(false);
    }
}
