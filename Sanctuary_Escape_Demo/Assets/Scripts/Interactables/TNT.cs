using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : Interactable
{
    Animator animator;
    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        promptMessage = "heal";
    }

    protected override void Interact()
    {
        // Get the PlayerHealth component on the player object
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        // Deal damage to the player
        playerHealth.TakeDamage(50f);

        // Play the explosion animation
        animator.SetTrigger("Explode");
    }
}
