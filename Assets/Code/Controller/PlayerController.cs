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

    public event System.Action<Vector3> onPlayerMoveDirectionEvent;

    private Vector3 m_moveDirection = Vector3.zero;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(Instance);
        Instance = this;

        m_controller = GetComponent<CharacterController>();
    }

    public void SetPlayerMoveDirection(Vector3 direction)
    {
        m_moveDirection.x = direction.x;
        m_moveDirection.y = 0;
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

        Vector3 move = m_moveDirection;//new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_controller.Move(move * Time.deltaTime * playerSpeed);


        bool running = Mathf.Abs(Vector3.Distance(move, Vector3.zero)) > 0;
        if (move != Vector3.zero)
            gameObject.transform.forward = move.normalized;

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * jumpValue * gravityValue);
            jump = true;
        }

        onPlayerAnimatorEvent?.Invoke(running, jump);

        playerVelocity.y += gravityValue * Time.deltaTime;
        m_controller.Move(playerVelocity * Time.deltaTime);
    }
}
