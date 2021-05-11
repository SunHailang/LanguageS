using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerController.Instance.onPlayerAnimatorEvent += OnAnimationEvent;
    }

    private void OnDestroy()
    {
        PlayerController.Instance.onPlayerAnimatorEvent -= OnAnimationEvent;
    }

    private void OnAnimationEvent(bool running, bool jump)
    {
        if (running)
            m_animator.SetInteger("IntSpeed", 1);
        else
            m_animator.SetInteger("IntSpeed", -1);

        m_animator.SetBool("BoolJump", jump);
    }

}
