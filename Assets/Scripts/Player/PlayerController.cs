using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementForce = 1300f;
    [Header("Jump Controls")]
    public float jumpForce = 34f;
    public float buttonTime = 0.3f; // The time alloted for the player to hold down the jump button to reach maximum height
    float jumpTime;
    bool jumping;

    [Space(5)]
    [Range(0f, 100f)] public float raycastDistance = 5f;
    public LayerMask whatIsGround;

    [Header("Animation")]
    public Animator anim;
    public Transform head;

    [Header("One Way Platform Controller")]
    private GameObject currentOneWayPlatform;
    [SerializeField] private BoxCollider2D playerCollider;

    private Rigidbody2D rb;
 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // One way platform functionality
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    if (currentOneWayPlatform != null)
        //    {
        //        StartCoroutine(DisableCollision());
        //    }
        //}

        Movement();
    }

    private void Update()
    {
        Jump();
    }

    private void Movement()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xDir * (movementForce * Time.deltaTime), rb.velocity.y);

        // animation

        if (xDir != 0)
        {
            head.localScale = new Vector3(xDir, 1f, 1f);
        }
        if (xDir > 0)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("WalkBack", false);
        }
        if (xDir < 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("WalkBack", true);
        }
        if (xDir == 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("WalkBack", false);
        }
    }

    private void Jump()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    if (IsGrounded())
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        //    }
        //}

        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                jumping = true;
                jumpTime = 0;
            }
        }
        if (jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || jumpTime > buttonTime)
        {
            jumping = false;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, whatIsGround);
        return hit.collider != null;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("OneWayPlatform"))
    //    {
    //        currentOneWayPlatform = collision.gameObject;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("OneWayPlatform"))
    //    {
    //        currentOneWayPlatform = null;
    //    }
    //}

    //private IEnumerator DisableCollision()
    //{
    //    BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

    //    Physics2D.IgnoreCollision(playerCollider, platformCollider);
    //    yield return new WaitForSeconds(0.25f);
    //    Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    //}
}
