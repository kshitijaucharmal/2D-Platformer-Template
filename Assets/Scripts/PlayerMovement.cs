using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement Controls")]
    [SerializeField] float speed = 450f;
    [SerializeField] float jump_force = 10f;
    [Range(0f, 1f)] [SerializeField] float stop_factor = 0.5f; // Controlled jump stopping factor
    [SerializeField] Transform feetPos; // feet position to check if grounded
    [SerializeField] Vector3 groundCheckSize = new Vector3(1, 0.05f, 1);
    [SerializeField] float gravity = 10; // gravity to apply when in air
    [SerializeField] LayerMask whatIsGround; // can only jump if on ground layer

    [Header("Better Platformer")]
    [SerializeField] private float hangTime = 0.1f;
    private float hangTimeCtr = 0;
    [SerializeField] private float jumpBufferLength = 0.1f;
    private float jumpBufferCtr = 0;
    
    // Private variables
    private Rigidbody2D rb;
    private float movement; // movement input
    private bool isGrounded = false;
    private bool can_jump = false;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void GetInput(){
        movement = Input.GetAxisRaw("Horizontal");
        
        // Check if grounded by a physics check
        isGrounded = Physics2D.OverlapBoxAll(feetPos.position, groundCheckSize, 0f, whatIsGround).Length > 0;
        // Set gravity to 0 if on ground
        rb.gravityScale = isGrounded ? 0 : gravity;

        // Hang/Coyote Time
        if(isGrounded){ hangTimeCtr = hangTime; }
        else hangTimeCtr -= Time.deltaTime;

        // Jump Buffer
        if(Input.GetButtonDown("Jump")) { jumpBufferCtr = jumpBufferLength; }
        else jumpBufferCtr -= Time.deltaTime;

        // Jump Detection
        if(hangTimeCtr > 0f && jumpBufferCtr > 0){
            can_jump = true;
            jumpBufferCtr = 0;
        }
        
        // Controlled Jump
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * stop_factor);
        }
    }

    // Update is called once per frame
    void Update() {
        GetInput();
    }

    void FixedUpdate(){
        // Movement
        var vel = rb.velocity;
        // Changing the velocity directly
        vel.x = movement * speed * Time.fixedDeltaTime;
        rb.velocity = vel;

        // Jumping
        if(can_jump){
            rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
            can_jump = false;
        }
    }

    // Debugging
    void OnDrawGizmos(){
        // Groundcheckbox
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetPos.position, groundCheckSize);
    }
}
