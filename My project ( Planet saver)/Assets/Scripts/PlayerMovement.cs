using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
private float horizontal;
private float speed = 8f;
private float jumpingPower = 16f;
private bool isFacingRight = true;
private bool isWallSliding;
private float wallSlidingSpeed = 2f;

private bool canDash = true;
private bool isDashing;
private float dashingPower = 24f;
private float dashingTime = 0.2f;
private float dashingCooldown = 1f;

private bool isWallJumping;
private float wallJumpingDirection;
private float wallJumpingTime = 0.2f;
private float wallJumpingCounter;
private float wallJumpingDuration = 0.4f;
private Vector2 wallJumpingPower = new Vector2(8f, 16f);

[SerializeField] private Rigidbody2D rb;
[SerializeField] private Transform groundCheck;
[SerializeField] private LayerMask groundLayer;
[SerializeField] private TrailRenderer tr;
[SerializeField] private Transform wallCheck;
[SerializeField] private LayerMask wallLayer;


private void Update()
{
    if (isDashing)
    {
        return;
    }

    horizontal = Input.GetAxisRaw("Horizontal");

    if (Input.GetButtonDown("Jump") && IsGrounded())
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
    }

    if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
    {
        StartCoroutine(Dash());
    }

    WallSlide();
    WallJump();

    if (!isWallJumping)
    {
        Flip();
    }
}


private void FixedUpdate()
{
    if (isDashing)
    {
        return;
    }

    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
}

private bool IsGrounded()
{
    return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
}

private bool IsWalled()
{
    return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
}

private void WallSlide()
{
    if (IsWalled() && !IsGrounded() && horizontal != 0f)
    {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }
    else
    {
        isWallSliding = false;
    }
}

private void WallJump()
{
    if (isWallSliding)
    {
        isWallJumping = false;
        wallJumpingDirection = -transform.localScale.x;
        wallJumpingCounter = wallJumpingTime;

        CancelInvoke(nameof(StopWallJumping));
    }
    else
    {
        wallJumpingCounter -= Time.deltaTime;
    }

    if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
    {
        isWallJumping = true;
        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
        wallJumpingCounter = 0f;

        if (transform.localScale.x != wallJumpingDirection)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    }
}

private void StopWallJumping()
{
    isWallJumping = false;
}

private void Flip()
{
    if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
    {
        Vector3 localScale = transform.localScale;
        isFacingRight = !isFacingRight;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}

private IEnumerator Dash()
{
    canDash = false;
    isDashing = true;
    float originalGravity = rb.gravityScale;
    rb.gravityScale = 0f;
    rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
    tr.emitting = true;
    yield return new WaitForSeconds(dashingTime);
    tr.emitting = false;
    rb.gravityScale = originalGravity;
    isDashing = false;
    yield return new WaitForSeconds(dashingCooldown);
    canDash = true;
}
}