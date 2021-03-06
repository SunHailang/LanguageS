using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private CharacterController m_controller;

    [SerializeField]
    private Transform m_shootPos = null;

    private PlayerAnimationController m_animationController;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 8.0f;

    private float jumpHeight = 1.0f;
    private float gravityValue = -9.8f;
    private float jumpValue = -2.8f;

    [Range(1.0f, 2.0f)]
    public float m_downSpeed = 1.5f;

    private Vector3 m_movePlayer = Vector3.zero;

    private SkillActionType m_playerSkill = SkillActionType.None;

    private bool m_isRunning = false;
    private float m_angleOffset = 0;

    private float m_shootTime = 0.0f;

    public bool IsDeath { get => App.Instance.IsDeath; }

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(Instance);
        Instance = this;

        m_controller = GetComponent<CharacterController>();
        m_animationController = GetComponentInChildren<PlayerAnimationController>();
        // Regiester Events
    }

    public void Death()
    {
        App.Instance.IsDeath = true;
        EventManager<Events>.Instance.TriggerEvent(Events.PlayerLifeState, !App.Instance.IsDeath);
    }
    public void Rebirth()
    {
        App.Instance.IsDeath = false;
        PlayerData.Instance.Rebirth();
        EventManager<Events>.Instance.TriggerEvent(Events.PlayerLifeState, !App.Instance.IsDeath);
    }

    public void SetPlayerMove(Vector3 direction)
    {
        if (m_playerSkill != SkillActionType.None || App.Instance.IsDeath)
        {
            direction = Vector3.zero;
        }

        float x = direction.x * Mathf.Cos(m_angleOffset) + direction.y * Mathf.Sin(m_angleOffset);
        float z = -direction.x * Mathf.Sin(m_angleOffset) + direction.y * Mathf.Cos(m_angleOffset);

        m_movePlayer.x = x;
        m_movePlayer.y = 0;
        m_movePlayer.z = z;
        m_animationController.PlayLookForwardEvent(m_movePlayer);

        m_movePlayer.y = direction.z;
        m_isRunning = x != 0 || z != 0;
    }

    public Vector3 SetPlayerDirection(Vector2 direction)
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += direction.x;
        transform.rotation = Quaternion.Euler(rotation);

        m_angleOffset = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;//弧度

        return transform.rotation.eulerAngles;
    }

    public void ShootBullet()
    {
        if (m_shootTime >= PlayerData.Instance.playerShootSpeed)
        {
            float hurt = UnityEngine.Random.Range(20f, 40f);
            float speed = UnityEngine.Random.Range(12f, 16f);
            BulletController.Instance.CreateBullet(m_shootPos.position, "Player", hurt, speed, m_shootPos.forward);
            m_shootTime = 0.0f;
        }
    }


    private void Update()
    {
        m_shootTime += Time.deltaTime;
        PlayerData.Instance.OnUpdate(Time.deltaTime);
    }



    private void FixedUpdate()
    {
        groundedPlayer = m_controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            m_animationController.PlayAnimationJump(false);
        }

        // Changes the height position of the player..
        if (m_movePlayer.y > 0 && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpValue * gravityValue);
            // magic
            PlayerData.Instance.SetPlayerData(ReplyType.Magic, -80f);

            m_animationController.PlayAnimationJump(true);
        }
        m_movePlayer.y = 0;

        if (m_isRunning) m_controller.Move(m_movePlayer.normalized * playerSpeed * Time.fixedDeltaTime);

        m_animationController.PlayAnimationRunning(m_isRunning);

        playerVelocity.y += gravityValue * Time.fixedDeltaTime * 1.785f;
        m_controller.Move(playerVelocity * Time.fixedDeltaTime);
    }
}
