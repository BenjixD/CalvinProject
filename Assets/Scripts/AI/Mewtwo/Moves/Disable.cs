using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : ISkill
{
    #region Private Member
    private float m_castTime = 0.5f;
    private float m_defaultCooldown = 1f;
    private float m_remainingCooldown;

    private Mewtwo m_reference;
    private Animator m_animator;

    private Vector3 m_colliderOffset = new Vector3(0.65f, 0.3f, 0f);
    #endregion

    #region Constructor
    public Disable(Mewtwo reference)
    {
        m_reference = reference;
        m_animator = m_reference.GetComponent<Animator>();
        m_remainingCooldown = 0f;
    }
    #endregion

    #region Properties
    public float CastTime
    {
        get
        {
            return m_castTime;
        }
    }
    #endregion

    #region Helper Functions
    protected void PutOnCooldown()
    {
        m_remainingCooldown = m_defaultCooldown;
    }
    #endregion

    #region Interface
    public float GetCooldown()
    {
        return m_defaultCooldown;
    }

    public float GetRemainingCooldown()
    {
        return m_remainingCooldown;
    }

    public bool IsReady()
    {
        return (m_remainingCooldown <= 0.0f);
    }

    public bool IsUsable()
    {
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(m_reference.gameObject.transform.position + m_colliderOffset,
                                                             m_reference.gameObject.transform.position - m_colliderOffset,
                                                             1 << LayerMask.NameToLayer("Player"));

        return (hitColliders.Length > 0);
    }

    public void InitAttack(Action<bool> action = null)
    {
        //Flip Mewtwo to face player
        IPlayer player = m_reference.GetTarget;
        if(player != null)
        {
            //If player is left to us and we are facing right
            if(player.GetObject.transform.position.x < m_reference.gameObject.transform.position.x &&
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

        //Use the attack
        foreach (Transform child in m_reference.gameObject.transform)
        {
            if(child.name == "DisableHitBox")
            {
                child.gameObject.GetComponent<DisableMoveHitBox>().Action = action;
                child.gameObject.SetActive(true);
                break;
            }
        }

        //Set Animations
        m_animator.SetTrigger("UseDisable");

        //Put move on cooldown
        PutOnCooldown();
    }
    #endregion

    public void DecrementCooldown(float decrement)
    {
        if(m_remainingCooldown > 0.0f)
        {
            m_remainingCooldown -= decrement;
        }
        
    }
}
