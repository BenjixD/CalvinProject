﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : Skill
{
    #region Private Member
    private Mewtwo m_reference;
    private Animator m_animator;

    private Vector3 m_colliderOffset = new Vector3(0.65f, 0.5f, 0f);
    #endregion

    #region Constructor#
    public Disable(Mewtwo reference)
    {
        m_reference = reference;
        m_animator = m_reference.GetComponent<Animator>();
        m_remainingCooldown = 0f;

        m_castTime = 0.5f;
        m_defaultCooldown = 2.5f;
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
    #endregion

    #region Interface
    public override bool IsUsable()
    {
        LayerMask player = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("NonPlatformInteractor"));
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(m_reference.gameObject.transform.position + m_colliderOffset,
                                                             m_reference.gameObject.transform.position - m_colliderOffset,
                                                             player);

        return (hitColliders.Length > 0);
    }

    public override void InitAttack(Action<bool> action = null)
    {
        //Disable Floaty
        m_reference.gameObject.GetComponent<FloatyBehaviour>().enabled = false;

        //Face the Player when using Skill
        FacePlayer();

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
}
