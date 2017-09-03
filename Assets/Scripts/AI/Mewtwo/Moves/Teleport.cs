using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Skill
{
    #region Private Member
    private Mewtwo m_reference;
    private Animator m_animator;
    private float m_tpRadius;
    private Mewtwo.Intent m_moveReason;
    #endregion

    #region Constructor
    public Teleport(Mewtwo reference, Mewtwo.Intent mt = Mewtwo.Intent.Discovery)
    {
        m_reference = reference;
        m_animator = m_reference.GetComponent<Animator>();
        m_remainingCooldown = 0f;

        m_castTime = 0.7f;
        m_defaultCooldown = 5f;

        m_tpRadius = m_reference.TeleportRadius;
        m_moveReason = mt;
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

    private Collider2D[] ReferenceObjects()
    {
        int layerMasks = (1 << LayerMask.NameToLayer("Player")) |
                         (1 << LayerMask.NameToLayer("Ground")) |
                         (1 << LayerMask.NameToLayer("Platform"));

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(m_reference.gameObject.transform.position,
                                                               m_tpRadius,
                                                               layerMasks);

        return hitColliders;
    }

    private Vector3 TeleportToObjectPosition()
    {
        IPlayer target = m_reference.GetTarget;
        Collider2D[] hits = ReferenceObjects();

        IList<Collider2D> players = new List<Collider2D>();
        IList<Collider2D> platforms = new List<Collider2D>();
        IList<Collider2D> ground = new List<Collider2D>();

        Vector3 targetLocation = m_reference.gameObject.transform.position;

        if(hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; ++i)
            {
                if(hits[i].gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    players.Add(hits[i]);
                }
                else if(hits[i].gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    platforms.Add(hits[i]);
                }
                else
                {
                    ground.Add(hits[i]);
                }
            }
        }

        if(target == null)
        {
            m_moveReason = Mewtwo.Intent.Discovery;
        }

        //Make the teleport decision
        if(m_moveReason == Mewtwo.Intent.Avoid)
        {
            //Find the platform furthest away from player
            Collider2D furthestAway = null;
            foreach(Collider2D platform in platforms)
            {
                if(furthestAway == null || 
                   (platform.gameObject.transform.position - target.GetObject.transform.position).magnitude > 
                   (furthestAway.gameObject.transform.position - target.GetObject.transform.position).magnitude)
                {
                    furthestAway = platform;
                }
            }

            //Teleporting to ground may be better
            if(ground.Count > 0)
            {
                if(furthestAway == null)
                {
                    targetLocation = TeleportOnGround(ground[0], target, true);
                }
                else
                {
                    Vector3 colSize = furthestAway.gameObject.GetComponent<BoxCollider2D>().size;

                    //If player is below us, pick platform
                    if (target.GetObject.transform.position.y < m_reference.transform.position.y)
                    {
                        float yOffset = furthestAway.gameObject.GetComponent<BoxCollider2D>().offset.y;
                        targetLocation = new Vector3(furthestAway.gameObject.transform.position.x + UnityEngine.Random.Range(-1*colSize.x/2 + 0.1f, colSize.x/2 - 0.1f),
                                                     furthestAway.gameObject.transform.position.y + yOffset + 0.1f,
                                                     0);
                    }
                    //If player is above us, pick ground
                    else
                    {
                        float yOffset = ground[0].gameObject.GetComponent<BoxCollider2D>().offset.y;
                        targetLocation = new Vector3(furthestAway.gameObject.transform.position.x + UnityEngine.Random.Range(-1 * colSize.x / 2 + 0.1f, colSize.x / 2 - 0.1f),
                                                     ground[0].gameObject.transform.position.y + yOffset + 0.1f,
                                                     0);
                    }
                }
            }
        }
        else if(m_moveReason == Mewtwo.Intent.Chase)
        {
            //Find the platform closest from player
            Collider2D closest = null;
            foreach (Collider2D platform in platforms)
            {
                if (closest == null ||
                   (platform.gameObject.transform.position - target.GetObject.transform.position).magnitude <
                   (closest.gameObject.transform.position - target.GetObject.transform.position).magnitude)
                {
                    closest = platform;
                }
            }

            if(players.Count > 0)
            {
                int ran = UnityEngine.Random.Range(0,1);
                Vector3 offset = m_reference.GetComponent<BoxCollider2D>().size/2;
                //Left side
                if(ran == 0)
                {
                    targetLocation = target.GetObject.transform.position - new Vector3(offset.x, 0, 0);
                }
                //Right side
                else
                {
                    targetLocation = target.GetObject.transform.position + new Vector3(offset.x, 0, 0);
                }
            }
            else if (closest == null)
            {
                targetLocation = TeleportOnGround(ground[0], target, false);
            }
            else
            {
                Vector3 colSize = closest.gameObject.GetComponent<BoxCollider2D>().size;

                //If player is below us, pick ground
                if (target.GetObject.transform.position.y < m_reference.transform.position.y)
                {
                    float yOffset = ground[0].gameObject.GetComponent<BoxCollider2D>().offset.y;
                    targetLocation = new Vector3(closest.gameObject.transform.position.x + UnityEngine.Random.Range(-1 * colSize.x / 2 + 0.1f, colSize.x / 2 - 0.1f),
                                                 ground[0].gameObject.transform.position.y + yOffset + 0.1f,
                                                 0);
                }
                //If player is above us, pick platform
                else
                {
                    float yOffset = closest.gameObject.GetComponent<BoxCollider2D>().offset.y;
                    targetLocation = new Vector3(closest.gameObject.transform.position.x + UnityEngine.Random.Range(-1 * colSize.x / 2 + 0.1f, colSize.x / 2 - 0.1f),
                                                 closest.gameObject.transform.position.y + yOffset + 0.1f,
                                                 0);
                }
            }
        }
        else //discovery
        {
            int ran = UnityEngine.Random.Range(0, hits.Length);
            int platformOrGround = UnityEngine.Random.Range(0, 1);
            Vector3 colSize = hits[ran].gameObject.GetComponent<BoxCollider2D>().size;

            //Do Platform
            if (platformOrGround == 0)
            {
                float yOffset = hits[ran].gameObject.GetComponent<BoxCollider2D>().offset.y;
                targetLocation = new Vector3(hits[ran].gameObject.transform.position.x + UnityEngine.Random.Range(-1 * colSize.x / 2 + 0.1f, colSize.x / 2 - 0.1f),
                                             hits[ran].gameObject.transform.position.y + yOffset + 0.1f,
                                             0);
            }
            //DoGround
            else
            {
                float yOffset = ground[0].gameObject.GetComponent<BoxCollider2D>().offset.y;
                targetLocation = new Vector3(hits[0].gameObject.transform.position.x + UnityEngine.Random.Range(-1 * colSize.x / 2 + 0.1f, colSize.x / 2 - 0.1f),
                                             ground[0].gameObject.transform.position.y + yOffset + 0.1f,
                                             0);
            }
        }

        return targetLocation;
    }

    private Vector3 TeleportOnGround(Collider2D ground, IPlayer target, bool awayFromTarget)
    {
        float xOffset = 0.3f;
        float yOffset = 0.1f;
        float leftBoundary = ground.gameObject.transform.position.x - ground.gameObject.GetComponent<BoxCollider2D>().size.x / 2 + xOffset;
        float rightBoundary = ground.gameObject.transform.position.x + ground.gameObject.GetComponent<BoxCollider2D>().size.x / 2 - yOffset;

        float x;
        float y = ground.gameObject.transform.position.y
                + ground.gameObject.GetComponent<BoxCollider2D>().offset.y
                + ground.gameObject.GetComponent<BoxCollider2D>().size.y / 2
                + yOffset;

        if(awayFromTarget)
        {
            //To the right of Player
            if (m_reference.gameObject.transform.position.x >= target.GetObject.transform.position.x)
            {
                x = Mathf.Min(m_reference.gameObject.transform.position.x + m_tpRadius, rightBoundary);
            }
            //To the left of player
            else
            {
                x = Mathf.Max(m_reference.gameObject.transform.position.x - m_tpRadius, leftBoundary);
            }
        }
        else
        {
            //To the right of Player
            if (m_reference.gameObject.transform.position.x >= target.GetObject.transform.position.x)
            {
                x = Mathf.Max(m_reference.gameObject.transform.position.x - m_tpRadius, leftBoundary);
            }
            //To the left of player
            else
            {
                x = Mathf.Min(m_reference.gameObject.transform.position.x + m_tpRadius, rightBoundary);
            }
        }
        
        //Set the target
        return new Vector3(x, y, 0);
    }
    #endregion

    #region Delegate Functions
    private float DelegateTeleport(Vector3 location)
    {
        m_reference.transform.parent.gameObject.transform.position = location;
        return m_castTime;
    }
    #endregion

    #region Interface
    public override bool IsUsable()
    {
        Collider2D[] hitColliders = ReferenceObjects();

        return (hitColliders.Length > 0);
    }

    public override void InitAttack(Action<bool> action = null)
    {
        //Disable Floaty
        //m_reference.gameObject.GetComponent<FloatyBehaviour>().enabled = false;

        //Face the Player when using Skill
        FacePlayer();

        //Use the Move
        Vector3 targetLocation = TeleportToObjectPosition();
        m_reference.DeferredTeleport = () => DelegateTeleport(targetLocation);

        //Set Animations
        m_animator.SetTrigger("UseTeleport");
        m_reference.GetComponent<BoxCollider2D>().enabled = false;

        //Put move on cooldown
        PutOnCooldown();
    }
    #endregion
}
