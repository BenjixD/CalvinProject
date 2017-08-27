using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Public Members
    // Change to proper ground layer number if needed
    public LayerMask groundLayer;
    public float horizontalSpd;
    public float verticalSpd;
    public float flashJumpSpd;
    public Vector3 HorizontalOffset;
    #endregion

    #region Private Members
    Rigidbody2D m_playerRB;

    float m_direction;
    float distToGround;

    bool m_grounded;
    bool m_facingRight;

    JumpingState CurrentJumpingState;
    bool m_currentlyJumping;
    bool m_currentlyAttacking;
    bool m_triggerAttack;
    #endregion

    #region Properties
    public bool Grounded
    {
        get
        {
            return m_grounded;
        }
    }

    public bool Moving
    {
        get
        {
            return (m_direction != 0.0f);
        }
    }

    public bool Attacking
    {
        get
        {
            return m_triggerAttack;
        }
    }
    #endregion

    #region enum
    enum JumpingState
    {
        Grounded = 0,
        SingleJump = 1,
        DoubleJump = 2
    }
    #endregion

    // Use this for initialization
    void Start () {
        m_playerRB = GetComponent<Rigidbody2D>();
        CurrentJumpingState = JumpingState.Grounded;
        distToGround = GetComponent<BoxCollider2D>().bounds.extents.y - GetComponent<BoxCollider2D>().offset.y/2;

        m_currentlyAttacking = false;
        m_currentlyJumping = false;
        m_triggerAttack = false;
    }
	
	// Update is called once per frame
	void Update () {
        m_currentlyJumping = false;
        m_triggerAttack = false;

        //Handledirectional Input
        float m_jump = HandleJump();
        m_jump = m_jump > 0 ? m_jump : m_playerRB.velocity.y/verticalSpd;
        m_direction = Input.GetAxisRaw("Horizontal");

        //Handle Attacking
        HandleAttack();

        //Handle Movement
        m_grounded = IsGrounded();
        m_playerRB.velocity = CalculateMovement(m_direction, m_jump);
    }

    void FixedUpdate()
    {
        //Facing Directions
        if(m_direction < 0 && m_facingRight && !m_currentlyAttacking) //Moving Left, facing right (flip)
        {
            Flip();
        }
        else if(m_direction > 0 && !m_facingRight && !m_currentlyAttacking)
        {
            Flip();
        }
    }

    #region Helper Functions
    // Checks if on ground through Raycast. Also changes drag
    bool IsGrounded()
    {
        bool res = Physics2D.Raycast(transform.position, -Vector2.up, distance: distToGround, layerMask: groundLayer.value);
        return res;
    }

    void Flip()
    {
        //Flip the character Model
        m_facingRight = !m_facingRight;

        //Set the new localScale to be inverted
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if(m_facingRight)
        {
            transform.position += HorizontalOffset;
        }
        else
        {
            transform.position -= HorizontalOffset;
        }
    }

    float HandleJump()
    {
        float val = Input.GetButtonDown("Vertical") ? 1.0f : 0.0f;

        if (IsGrounded())
        {
            CurrentJumpingState = JumpingState.Grounded;
        }
        else if (CurrentJumpingState == JumpingState.Grounded && !IsGrounded())
        {
            CurrentJumpingState = JumpingState.SingleJump;
        }

        if (val > 0)
        {
            if (CurrentJumpingState == JumpingState.Grounded ||
                    CurrentJumpingState == JumpingState.SingleJump)
            {
                CurrentJumpingState++;
                m_currentlyJumping = true;
            }
            else
            {
                val = 0.0f;
            }
        }
        return val;
    }

    void HandleAttack()
    {
        if(!m_currentlyAttacking && Input.GetButtonDown("Attack"))
        {
            StartCoroutine("Attack");
        }
    }

    Vector2 CalculateMovement(float m_direction, float m_jump)
    {
        //Calculate X
        float m_x;
        if(CurrentJumpingState == JumpingState.Grounded)
        {
            if(!m_currentlyAttacking)
            {
                m_x = m_direction * horizontalSpd;
            }
            else
            {
                m_x = 0.0f;
            }
        }
        else if(m_currentlyJumping && CurrentJumpingState == JumpingState.DoubleJump)
        {
            m_x = (m_facingRight? 1.0f:-1.0f) * flashJumpSpd;
        }
        else
        {
            m_x = m_playerRB.velocity.x;
        }

        //Calculate Y
        var m_y = m_jump * verticalSpd;
        if(m_currentlyJumping && CurrentJumpingState == JumpingState.DoubleJump)
        {
            m_y /= 2;
        }

        return new Vector2(m_x, m_y);
    }    
    #endregion

    #region Coroutines
    IEnumerator Attack()
    {
        m_currentlyAttacking = true;
        m_triggerAttack = true;
        yield return new WaitForSeconds(.3f);

        m_currentlyAttacking = false;
    }
    #endregion
}
