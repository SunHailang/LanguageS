using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator m_animator;

    private Vector3 m_forward = Vector3.forward;

    private bool m_running = false;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        AnimationClip[] clips = m_animator.runtimeAnimatorController.animationClips;
        foreach (var item in clips)
        {
            switch (item.name)
            {
                case "Root|Idle":

                    break;
                case "Root|Jump":

                    break;
                case "Root|Run":

                    break;
            }
        }
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

    public void PlayAnimationRunning(bool running)
    {
        if (m_running != running)
        {
            m_running = running;
            m_animator.SetInteger("IntSpeed", m_running ? 1 : -1);
        }
    }

    public void PlayAnimationJump(bool jump)
    {
        m_animator.SetBool("BoolJump", jump);
    }

}
