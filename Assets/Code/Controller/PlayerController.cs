using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private CharacterController m_controller;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;

    private float jumpHeight = 1.0f;
    private float gravityValue = -9.8f;
    private float jumpValue = -2.8f;

    public event System.Action<bool, bool> onPlayerAnimatorEvent;
    public event System.Action<SkillActionType> onPlayerSkillEvent;

    private Vector3 m_movePlayer = Vector3.zero;
    private Vector3 m_directionPlayer = Vector3.right;

    private SkillActionType m_playerSkill = SkillActionType.None;

    private bool m_isDeath = false;
    public bool IsDeath { get { return m_isDeath; } }

    private bool m_isRunning = false;
    private float m_angleOffset = 0;


    private void Awake()
    {
        if (Instance != null) DestroyImmediate(Instance);
        Instance = this;

        m_controller = GetComponent<CharacterController>();

        // Regiester Events
    }

    public void Death()
    {
        m_isDeath = true;
        EventManager<Events>.Instance.TriggerEvent(Events.PlayerLifeState, !m_isDeath);
    }
    public void Rebirth()
    {
        m_isDeath = false;
        PlayerData.Instance.SetPlayerData(ReplyType.Blood, 1.0f);
        PlayerData.Instance.SetPlayerData(ReplyType.Magic, 1.0f);
        EventManager<Events>.Instance.TriggerEvent(Events.PlayerLifeState, !m_isDeath);
    }

    public void SetPlayerMove(Vector3 direction)
    {
        if (m_playerSkill != SkillActionType.None || m_isDeath)
        {
            direction = Vector3.zero;
        }

        float x = direction.x * Mathf.Cos(m_angleOffset) + direction.y * Mathf.Sin(m_angleOffset);
        float z = -direction.x * Mathf.Sin(m_angleOffset) + direction.y * Mathf.Cos(m_angleOffset);

        m_movePlayer.x = x;
        m_movePlayer.y = direction.z;
        m_movePlayer.z = z;

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


    private void Update()
    {
        bool jump = false;
        groundedPlayer = m_controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jump = false;
            playerVelocity.y = 0f;
        }

        // Changes the height position of the player..
        if (m_movePlayer.y > 0 && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpValue * gravityValue);
            jump = true;
            // magic
            PlayerData.Instance.SetPlayerData(ReplyType.Magic, -0.1f);
        }
        m_movePlayer.y = 0;

        if (m_isRunning)
        {
            //transform.position += m_movePlayer * playerSpeed * Time.deltaTime;
            m_controller.Move(m_movePlayer.normalized * playerSpeed * Time.deltaTime);
        }

        onPlayerAnimatorEvent?.Invoke(m_isRunning, jump);

        playerVelocity.y += gravityValue * Time.deltaTime;
        m_controller.Move(playerVelocity * Time.deltaTime);
    }
}
