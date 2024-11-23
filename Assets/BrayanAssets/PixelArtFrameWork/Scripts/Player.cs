using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 12f;

    public float jumpForce = 3f;

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

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
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");

        moveState = new PlayerMoveState(this, stateMachine, "Move");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");

        airState = new PlayerAirState(this, stateMachine, "Jump");

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();
        
        //Set defaul statemachine
        stateMachine.Initialize(idleState);

    }

    public float timer;
    public float cooldown = 4;

    // Update is called once per frame
    private void Update()
    {
        stateMachine.currentSate.update();

        timer -= Time.deltaTime;

        if (timer < 0 && Input.GetKeyDown(KeyCode.R))
        {
            timer = cooldown;
        }

        
    }

    public void SetVelocity (float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    

    private void OnDrawGizmos()
    {
        Color color = new Color(0, 1, 0);
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));


    }
    public void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

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
