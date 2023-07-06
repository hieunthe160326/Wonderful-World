using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private SpriteRenderer rbSprite;
    private Animator animator;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask jumpableGround;
    private enum MovementState { idle, running, jumping, falling }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rbSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            rbSprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            rbSprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;

        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;

        }

        animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
       return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
