using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator m_animator;

    private Vector3 m_forward = Vector3.forward;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void PlayLookForwardEvent(Vector3 direction)
    {
        if (direction.x != 0 || direction.z != 0)
        {
            m_forward.x = direction.x;
            m_forward.z = direction.z;
            transform.forward = m_forward;
        }
    }

    public void PlayAnimationEvent(bool running, bool jump)
    {
        if (running)
            m_animator.SetInteger("IntSpeed", 1);
        else
            m_animator.SetInteger("IntSpeed", -1);

        m_animator.SetBool("BoolJump", jump);
    }

}
