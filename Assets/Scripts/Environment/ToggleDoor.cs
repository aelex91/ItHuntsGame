using System;
using System.Collections;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
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
}
