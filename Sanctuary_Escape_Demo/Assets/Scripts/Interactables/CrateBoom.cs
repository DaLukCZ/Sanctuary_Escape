using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoom : Interactable
{
    Animator animator;
    private string startMessage;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startMessage = promptMessage;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("closedCreate"))
        {
            promptMessage = startMessage;
        }
        else
        {
            promptMessage = "Opening..";
        }
    }

    protected override void Interact()
    {
        this.GetComponent<Animator>().SetBool("opened", true);
    }
}
