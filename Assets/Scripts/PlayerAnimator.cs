using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement.moveDirection.x != 0 || playerMovement.moveDirection.y != 0)
        {
            animator.SetBool("Moving", true);
            SpriteDirection();
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        
    }

    void SpriteDirection()
    {
        if (playerMovement.moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
