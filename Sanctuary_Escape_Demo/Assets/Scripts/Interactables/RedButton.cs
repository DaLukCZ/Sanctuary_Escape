using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : Interactable
{
    public GameObject enemies;
    protected override void Interact()
    {
            enemies.gameObject.SetActive(true);
    }

}
