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

    private Vector3 m_moveDirection = Vector3.zero;
    private SkillActionType m_playerSkill = SkillActionType.None;


    private void Awake()
    {
        if (Instance != null) DestroyImmediate(Instance);
        Instance = this;

        m_controller = GetComponent<CharacterController>();

        // Regiest Events

    }

    private void Start()
    {
        
    }

    private void OnlayerSkillEvent(SkillActionType type)
    {

    }

    public void SetPlayerMoveDirection(Vector3 direction)
    {
        if (m_playerSkill != SkillActionType.None)
        {
            m_moveDirection = Vector3.zero;
            return;
        }

        m_moveDirection.x = direction.x;
        m_moveDirection.y = direction.z;
        m_moveDirection.z = direction.y;
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

        Vector3 move = new Vector3(m_moveDirection.x, 0, m_moveDirection.z);
        m_controller.Move(move * Time.deltaTime * playerSpeed);

        //float distance = Vector3.Distance();
        bool running = move != Vector3.zero;
        if (move != Vector3.zero)
            gameObject.transform.forward = move.normalized;

        // Changes the height position of the player..
        if (m_moveDirection.y > 0 && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpValue * gravityValue);
            jump = true;
            m_moveDirection.y = 0;
        }

        onPlayerAnimatorEvent?.Invoke(running, jump);

        playerVelocity.y += gravityValue * Time.deltaTime;
        m_controller.Move(playerVelocity * Time.deltaTime);
    }
}
