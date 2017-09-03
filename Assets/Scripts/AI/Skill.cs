using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ISkill
{
    #region Private Member
    protected float m_castTime;
    protected float m_defaultCooldown;
    protected float m_remainingCooldown;
    #endregion

    #region Helper Functions
    protected void PutOnCooldown()
    {
        m_remainingCooldown = m_defaultCooldown;
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
    public float Cooldown
    {
        get
        {
            return m_defaultCooldown;
        }
    }
    public float RemainingCooldown
    {
        get
        {
            return m_remainingCooldown;
        }
    }
    #endregion

    #region Interface
    public bool IsReady()
    {
        return (m_remainingCooldown <= 0.0f);
    }

    public virtual bool IsUsable()
    {
        return false;
    }

    public void DecrementCooldown(float decrement)
    {
        if (m_remainingCooldown > 0.0f)
        {
            m_remainingCooldown -= decrement;
        }

    }

    public virtual void InitAttack(Action<bool> action = null)
    {

    }
    #endregion

}
