using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    public float speed = 5f;
    public float horizontalMultiplier = 3f;
    public float jumpForce = 5f;

    public LayerMask groundMask;
    public Rigidbody rb;  

    float horizontalInput;

    // cached collider for grounded checks
    Collider myCollider;

    private void Awake()
    {
        // try to auto-assign components if they're not set in Inspector
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null) Debug.LogError("[PlayerMovement] Rigidbody not assigned and not found on GameObject.");
        }

        myCollider = GetComponent<Collider>();
        if (myCollider == null) Debug.LogError("[PlayerMovement] Collider not found on player -- IsGrounded will fail.");
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        // preserve rb.position (includes current y) and add forward/horizontal displacement
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalmove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalmove);
    }

    void Update()
    {
        // Movement input (A and D keys) handled by Unity Input Manager
        horizontalInput = Input.GetAxis("Horizontal");

        // Debug: detect the Jump button press
        if (Input.GetButtonDown("Jump"))
        {
            
            if (IsGrounded())
            {
               
                Jump();
            }

        }

        // Fall check
        if (transform.position.y < -5)
        {
            Die();
        }
    }

    void Jump()
    {
        if (rb == null) return;

        // reset vertical velocity so jumps are consistent
        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        // use impulse for instantaneous jump
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        if (myCollider == null)
        {
            // fallback: try to get Collider now
            myCollider = GetComponent<Collider>();
            if (myCollider == null) return false;
        }

        // use collider bounds center and extent for a robust raycast origin / distance
        Vector3 origin = myCollider.bounds.center;
        float distance = myCollider.bounds.extents.y + 0.1f;
        bool grounded = Physics.Raycast(origin, Vector3.down, distance, groundMask);

        // debug optional - comment out if noisy
        Debug.DrawRay(origin, Vector3.down * distance, grounded ? Color.green : Color.red, 0.1f);

        return grounded;
    }

    public void Die()
    {
        alive = false;
        Invoke("Restart", 1);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
