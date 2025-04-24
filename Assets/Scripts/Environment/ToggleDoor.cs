using Assets.Scripts;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;

public class ToggleDoor : Interactable
{
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();


        StartCoroutine(Do());

    }

    private IEnumerator Do()
    {
        yield return new WaitForSeconds(2f);

        m_Animator.SetBool("isOpen", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var tag = collision.gameObject.CompareTag("Player");

        if (tag)
        {
            CanInteract = true;
            return;
        }

    }

    public override void Interact()
    {
        if (CanInteract == false)
            return;

        base.Interact();

        StartCoroutine(Do());
    }


}
