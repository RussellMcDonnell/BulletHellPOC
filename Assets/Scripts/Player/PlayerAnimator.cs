using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerMovement.MoveDirection.x != 0 || _playerMovement.MoveDirection.y != 0)
        {
            _animator.SetBool("Moving", true);
            SpriteDirection();
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
        
    }

    void SpriteDirection()
    {
        if (_playerMovement.MoveDirection.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_playerMovement.MoveDirection.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
