using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movment 
    public float moveSpeed;
    Rigidbody2D rigidBody;

    [HideInInspector]
    public Vector2 moveDirection;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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

        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

    }

    void Move()
    {
        // Move the player character
        rigidBody.MovePosition(rigidBody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

    }
}
