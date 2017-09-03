using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : Skill
{
    #region Private Member
    private Mewtwo m_reference;
    private Animator m_animator;
    private MoveType m_moveReason;
    private float m_speed;
    #endregion

    #region Enum
    public enum MoveType
    {
        Avoid = 0,
        Chase = 1,
        Discover = 2
    }
    #endregion

    #region Constructor
    public Walk(Mewtwo reference, MoveType mt = MoveType.Discover)
    {
        m_reference = reference;
        m_animator = m_reference.GetComponent<Animator>();
        m_remainingCooldown = 0f;

        m_defaultCooldown = 0f;

        m_moveReason = mt;
        m_speed = 0.8f;
    }
    #endregion

    #region Helper Functions
    private void FacePlayer()
    {
        //Flip Mewtwo to face player
        IPlayer player = m_reference.GetTarget;
        if (player != null)
        {
            //If player is left to us and we are facing right
            if (player.GetObject.transform.position.x < m_reference.gameObject.transform.position.x &&
               m_reference.IsFacingRight)
            {
                m_reference.Flip();
            }
            //If player is right to us and we are facing left
            else if (player.GetObject.transform.position.x > m_reference.gameObject.transform.position.x &&
               !m_reference.IsFacingRight)
            {
                m_reference.Flip();
            }
        }
    }

    private void FaceMovingDirection()
    {
        //If moving left and facing right
        if (m_reference.GetComponent<Rigidbody2D>().velocity.x < 0 && m_reference.IsFacingRight)
        {
            m_reference.Flip();
        }
        //If moving right and facing left
        else if (m_reference.GetComponent<Rigidbody2D>().velocity.x > 0 && !m_reference.IsFacingRight)
        {
            m_reference.Flip();
        }
    }

    private void MoveMyself()
    {
        IPlayer p = m_reference.GetTarget;
        GameObject player = (p == null)? null : p.GetObject;
        Rigidbody2D parentRB = m_reference.transform.parent.gameObject.GetComponent<Rigidbody2D>();

        if (player == null)
        {
            m_moveReason = MoveType.Discover;
        }

        //Random Chance to do nothing
        if(UnityEngine.Random.Range(0f, 3f) < 1f)
        {
            return;
        }

        //Handle Moving
        if(m_moveReason == MoveType.Avoid)
        {
            //If player is left to us and we are facing right
            if (player.transform.position.x < m_reference.gameObject.transform.position.x)
            {
                parentRB.velocity = new Vector3(m_speed, parentRB.velocity.y, 0);
            }
            //If player is right to us and we are facing left
            else if (player.transform.position.x > m_reference.gameObject.transform.position.x)
            {
                parentRB.velocity = new Vector3(-1* m_speed, parentRB.velocity.y, 0);
            }
        }

        else if(m_moveReason == MoveType.Chase)
        {
            //If player is left to us and we are facing right
            if (player.transform.position.x < m_reference.gameObject.transform.position.x)
            {
                parentRB.velocity = new Vector3(-1*m_speed, parentRB.velocity.y, 0);
            }
            //If player is right to us and we are facing left
            else if (player.transform.position.x > m_reference.gameObject.transform.position.x)
            {
                parentRB.velocity = new Vector3(m_speed, parentRB.velocity.y, 0);
            }
        }

        else
        {
            int ran = UnityEngine.Random.Range(0, 1);
            if (ran == 0)
            {
                parentRB.velocity = new Vector3(-1 * m_speed, parentRB.velocity.y, 0);
            }
            else
            {
                parentRB.velocity = new Vector3(m_speed, parentRB.velocity.y, 0);
            }
        }
        FaceMovingDirection();
    }
    #endregion

    #region Interface
    public override bool IsUsable()
    {
        return true;
    }

    public override void InitAttack(Action<bool> action = null)
    {
        //Random the Cast Time
        m_castTime = UnityEngine.Random.Range(1f, 1.5f);

        //Face the Player when using Skill
        FacePlayer();

        //Move According to Moving Reason
        MoveMyself();

        //Put move on cooldown
        PutOnCooldown();
    }
    #endregion
}
