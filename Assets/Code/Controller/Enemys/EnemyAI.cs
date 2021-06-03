using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimationController m_animationController = null;
    [Space]
    [SerializeField]
    private CharacterController m_character = null;
    [SerializeField]
    private Transform m_shootPos = null;

    private BulletAI m_bulletPrefab;

    private float m_speed = 0.0f;
    private float m_hurt = 0.0f;

    private float m_hurtTime = 0.0f;
    private float m_hurtInterval = 0.0f;

    private bool m_move = true;

    Vector3 m_moveDir = Vector3.zero;

    private bool m_init = false;

    private void Awake()
    {
        // Register
        EventManager<Events>.Instance.RegisterEvent(Events.PlayerLifeState, OnPlayerLifeState);
    }

    private void OnDestroy()
    {
        // Deregister
        EventManager<Events>.Instance.DeregisterEvent(Events.PlayerLifeState, OnPlayerLifeState);
    }

    private void OnPlayerLifeState(Events arg1, object[] arg2)
    {
        bool death = !Convert.ToBoolean(arg2[0]);
        if (!death)
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        m_speed = UnityEngine.Random.Range(5.0f, 8.0f);
        m_hurt = UnityEngine.Random.Range(120f, 140f);

        m_hurtTime = 0.0f;
        m_hurtInterval = UnityEngine.Random.Range(6.0f, 10.0f);

        m_init = true;
    }

    private void Update()
    {
        if (!m_init || PlayerController.Instance.IsDeath) return;
        if (m_hurtTime >= m_hurtInterval)
        {
            // Short
            Shoot();
            m_hurtTime = 0.0f;
        }
        m_hurtTime += Time.deltaTime;
    }

    private void Shoot()
    {
        float hurt = m_hurt;
        float speed = UnityEngine.Random.Range(12.0f, 16.0f);
        Vector3 dir = (PlayerController.Instance.transform.position - transform.position).normalized;
        BulletController.Instance.CreateBullet(m_shootPos.position, "Enemy", hurt, speed, dir);
    }

    private void FixedUpdate()
    {
        if (PlayerController.Instance.IsDeath) return;
        //float dis = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
        if (m_move)
        {
            Vector3 dir = PlayerController.Instance.transform.position - transform.position;
            if (dir != Vector3.zero)
            {
                m_animationController.PlayLookForwardEvent(dir.normalized);
                m_moveDir.x = dir.x;
                m_moveDir.z = dir.z;
            }
            dir.y = 0.0f;
            m_character.Move(m_moveDir.normalized * Time.fixedDeltaTime * m_speed);
            //transform.position += ;

            m_animationController.PlayAnimationRunning(dir != Vector3.zero && m_speed > 0.0f);
        }
        else
        {
            m_animationController.PlayAnimationRunning(false);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit != null && hit.collider.tag == "Player")
        {
            PlayerData.Instance.SetPlayerData(ReplyType.Blood, -PlayerData.Instance.playerMaxBlood);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            switch (other.tag)
            {
                case "Mushroom":
                    PlayerData.Instance.SetScore(1);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

}
