using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShadowBall : Skill
{
    //FREEZE EFFECTIVENESS to 50

    public float PullObjectSpeed;
    public float PullObjectRadius;

    #region Private Member
    private Mewtwo m_reference;
    private Animator m_animator;

    private IList<GameObject> m_pullingObjects;
    #endregion

    #region Constructor
    public ChargeShadowBall(Mewtwo reference)
    {
        m_reference = reference;
        m_animator = m_reference.GetComponent<Animator>();
        m_remainingCooldown = 0f;

        m_castTime = 0.9f;
        m_defaultCooldown = 15f;

        m_pullingObjects = new List<GameObject>();
        PullObjectSpeed = 3f;
        PullObjectRadius = 4f;
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

    private void PullNearbyObjects()
    {
        int layerMasks = (1 << LayerMask.NameToLayer("Platform"));
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(m_reference.gameObject.transform.position,
                                                                PullObjectRadius,
                                                                layerMasks);

        foreach(Collider2D collider in hitColliders)
        {
            if(collider.GetComponent<Rigidbody2D>() != null)
            {
                Vector3 direction = (m_reference.gameObject.transform.position - collider.gameObject.transform.position).normalized;
                collider.GetComponent<Rigidbody2D>().velocity += new Vector2(direction.x, direction.y) * PullObjectSpeed;
                m_pullingObjects.Add(collider.gameObject);
            }
        }
    }

    //Delegated to Mewtwo to perform
    private void StopPullingNearbyObjects(bool delegateOperation)
    {
        foreach(GameObject obj in m_pullingObjects)
        {
            if (obj != null)
            {
               if (obj.layer == LayerMask.NameToLayer("Player") ||
                   obj.layer == LayerMask.NameToLayer("Platform"))
               {
                    obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
               }
            }
            
        }
        m_pullingObjects.Clear();
    }
    #endregion

    #region Interface
    public override bool IsUsable()
    {
        return !(m_reference.ChargedShadowBall);
    }

    public override void InitAttack(Action<bool> action = null)
    {
        //Disable Floaty
        m_reference.gameObject.GetComponent<FloatyBehaviour>().enabled = false;

        //Face the Player when using Skill
        FacePlayer();

        //Use the attack
        PullNearbyObjects();
        foreach (Transform child in m_reference.gameObject.transform)
        {
            if(child.name == "ObjectMagnet")
            {
                child.gameObject.SetActive(true);
                break;
            }
        }

        //Set Animations
        m_animator.SetTrigger("UseChargeShadowBall");

        //Set Shadowball to be charged
        m_reference.ChargedShadowBall = true;
        m_reference.ChargeChanged = true;

        //Put move on cooldown
        PutOnCooldown();

        //Delegate Stop Pulling objects till after the move effect
        m_reference.DelegateOperation = StopPullingNearbyObjects;
    }
    #endregion
}
