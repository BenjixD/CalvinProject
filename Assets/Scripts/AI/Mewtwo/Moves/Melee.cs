using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Skill
{
    #region Private Member
    private Mewtwo m_reference;
    private Animator m_animator;

    private Vector3 m_colliderOffset = new Vector3(1.4f, 0.5f, 0f);
    #endregion

    #region Constructor
    public Melee(Mewtwo reference)
    {
        m_reference = reference;
        m_animator = m_reference.GetComponent<Animator>();
        m_remainingCooldown = 0f;

        m_castTime = 0.6f;
        m_defaultCooldown = 4f;
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
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(m_reference.gameObject.transform.position + m_colliderOffset,
                                                             m_reference.gameObject.transform.position - m_colliderOffset,
                                                             1 << LayerMask.NameToLayer("Player"));

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
            if(child.name == "MeleeHitBox")
            {
                child.gameObject.GetComponent<MeleeMoveHitBox>().Action = action;
                child.gameObject.SetActive(true);
                break;
            }
        }

        //Set Animations
        m_animator.SetTrigger("UseMelee");

        //Put move on cooldown
        PutOnCooldown();
    }
    #endregion
}
