using UnityEngine;

public class Poison : Interactable
{
    [SerializeField]
    private float dmgAmount = 40f;
    [SerializeField]
    private PlayerHealth playerHealth;

    protected override void Interact()
    {
        playerHealth.TakeDamage(dmgAmount);
    }
}
