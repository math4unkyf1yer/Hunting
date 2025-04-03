using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Normal speed
    public float jumpForce = 8f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpRequested;
    private float normalSpeed;
    private bool isSlowed;
    private bool isAttacking;
    private float slowDuration = 2f;  // How long the slow lasts
    private float slowTimeRemaining;  // Tracks the time remaining for slow

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalSpeed = moveSpeed;  // Store normal speed
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isAttacking)
        {
            return;  // No movement when attacking
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }

        // Handle speed restoration after slow-down time ends
        if (isSlowed)
        {
            slowTimeRemaining -= Time.deltaTime;  // Reduce the remaining slow-down time
            if (slowTimeRemaining <= 0f)
            {
                RestoreSpeed();  // Restore the player's speed after the slow-down period
            }
        }
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            return; // Stop movement if attacking
        }

        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpRequested = false;
        }

        // Handle movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply slowed movement if the player is slowed
        if (isSlowed)
        {
            rb.MovePosition(rb.position + move * (moveSpeed * 0.5f) * Time.fixedDeltaTime);  // 50% speed
        }
        else
        {
            rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // This method slows down the player (cuts movement speed in half)
    public void SlowDown()
    {
        if (!isSlowed)
        {
            isSlowed = true;
            slowTimeRemaining = slowDuration;  // Set slow duration
            moveSpeed *= 0.5f;  // Cut the movement speed in half
        }
    }

    // Set whether the player is attacking (this stops movement temporarily)
    public void SetAttacking(bool value)
    {
        isAttacking = value;
    }

    // This method restores the player's speed back to normal
    private void RestoreSpeed()
    {
        moveSpeed = normalSpeed;  // Restore normal speed
        isSlowed = false;  // Reset the slow-down flag
    }
}