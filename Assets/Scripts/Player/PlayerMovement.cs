using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movment 
    public float MoveSpeed;
    private Rigidbody2D _rigidBody;

    [HideInInspector]
    public Vector2 MoveDirection;
    public Vector2 LastMoveVector;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

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
        // Move the player character
        _rigidBody.MovePosition(_rigidBody.position + MoveDirection * MoveSpeed * Time.fixedDeltaTime);

    }
}
