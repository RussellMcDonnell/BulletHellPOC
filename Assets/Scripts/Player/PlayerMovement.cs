using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private PlayerStats _playerStats;
    
    [HideInInspector]
    public Vector2 MoveDirection;
    public Vector2 LastMoveVector;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerStats = GetComponent<PlayerStats>();

        LastMoveVector = new Vector2(1f, 0f); // Set a starting direction so projectiles will still work even if they player never moves
    }

    void Update()
    {
        InputManagment();
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManagment()
    {
        // Check if the game is over or paused
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver ||
            GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        MoveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        if (MoveDirection.x != 0)
        {
            LastMoveVector = new Vector2(MoveDirection.x, 0f); // Last moved X
        }
        if (MoveDirection.y != 0)
        {
            LastMoveVector = new Vector2(0f, MoveDirection.y); // Last moved Y
        }

        if(MoveDirection.x != 0 || MoveDirection.y != 0)
        {
            LastMoveVector = new Vector2(MoveDirection.x, MoveDirection.y); // Last moved X and Y
        }

    }

    void Move()
    {
        // Check if the game is over or paused
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver ||
            GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            return;

        // Move the player character
        _rigidBody.linearVelocity = new Vector2(MoveDirection.x * _playerStats.CurrentMovementSpeed, MoveDirection.y * _playerStats.CurrentMovementSpeed);
    }
}
