using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimationController m_animationController;

    private float m_speed = 0.0f;

    private bool m_move = true;

    Vector3 m_moveDir = Vector3.zero;

    private void Awake()
    {
        //m_animationController.GetComponentInChildren<PlayerAnimationController>();
    }

    public void Init()
    {
        m_speed = UnityEngine.Random.Range(3.0f, 10.0f);
    }

    private void Update()
    {
        if (PlayerController.Instance.IsDeath) return;

    }

    private void FixedUpdate()
    {
        if (PlayerController.Instance.IsDeath) return;
        float dis = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
        if (m_move)
        {
            Vector3 dir = PlayerController.Instance.transform.position - transform.position;
            if (dir != Vector3.zero)
            {
                m_animationController.PlayLookForwardEvent(dir.normalized);
                m_moveDir.x = dir.x;
                m_moveDir.z = dir.z;
            }
            bool jump = false;
            dir.y = 0.0f;
            transform.position += m_moveDir.normalized * Time.fixedDeltaTime * m_speed;

            m_animationController.PlayAnimationEvent(dir != Vector3.zero && m_speed > 0.0f, jump);
        }
        else
        {
            m_animationController.PlayAnimationEvent(false, false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Enemy":
                    Destroy(gameObject);
                    break;
                case "Mushroom":
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    break;
                case "Player":
                    m_move = false;
                    break;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Player":
                    PlayerData.Instance.SetPlayerData(ReplyType.Blood, -Time.deltaTime / 100);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Player":
                    m_move = true;
                    break;
            }
        }
    }

}
