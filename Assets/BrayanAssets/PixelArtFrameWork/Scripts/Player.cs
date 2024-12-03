using System.Threading;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 12f;

    public float jumpForce = 3f;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration = 5f;

    [SerializeField] private float dashCooldown = 4f;
    private float dashUsageTimer;
    public float dashDirection { get; private set; }

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    //Wall
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
     

    public int facingDirection { get; private set; } = 1;
    private bool facingRight = true;

    #region PlayerComponents
    public Animator anim { get; private set; }

    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }

    public PlayerMoveState moveState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }

    public PlayerAirState airState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerWallSlideState wallSlideState { get; private set; }

    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");

        moveState = new PlayerMoveState(this, stateMachine, "Move");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");

        airState = new PlayerAirState(this, stateMachine, "Jump");

        dashState = new PlayerDashState(this, stateMachine , "Dash");

        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();
        
        //Set defaul statemachine
        stateMachine.Initialize(idleState);

    }


    // Update is called once per frame
    private void Update()
    {
        stateMachine.currentSate.update();

        
        Debug.Log(isWallDetected());
        checkForDashInput();
    }

    public void SetVelocity (float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool isWallDetected() => Physics2D.Raycast(groundCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Color color = new Color(0, 1, 0);
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));


    }
    

    public void checkForDashInput ()
    {
        dashUsageTimer -= Time.deltaTime;

        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDirection = UnityEngine.Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);
        }
    }

    //Flipping the character always
    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    //Control how the Flipping Character Works 
    public void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        } 
        else if(x < 0 && facingRight)
        {
            Flip();
        }
    }
}
