using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pOS : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            animator.SetFloat("motion", 0f);
        }
    }
}
