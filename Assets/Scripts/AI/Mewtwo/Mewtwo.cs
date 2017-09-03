using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mewtwo : AIBehaviour {
    #region Public Members
    public float TeleportRadius;
    public GameObject LeftWall;
    public GameObject RightWall;

    public Action<bool> DelegateOperation;
    #endregion

    #region Balancer Values
    public float PlatformInfluence = 0.1f;
    public float TeleportInfluence = 1.3f;
    public float CorneringInfluence = 1f;
    #endregion

    #region Charged Items
    public bool ChargedShadowBall;
    #endregion

    #region Private Members
    private Health m_life;

    private float m_averageDamageTaken;
    private long m_movesMade;
    private float m_damageTaken;
    #endregion

    // Use this for initialization
    void Start ()
    {
        base.Initialize();

        Config = new MewtwoConfig(this);
        LoadIntents(Config);
        AddIntentBalancers();
        m_facingRight = true;

        m_life = GetComponent<Health>();
        m_averageDamageTaken = 0;
        m_movesMade = 0;
        m_damageTaken = 0;

        ChargedShadowBall = false;
        

        StartCoroutine("PerformMove");
        StartCoroutine("DecrementCooldowns");
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleFlip();
	}

    #region Helper Functions
    protected IPlayer CheckPlayerInTPRange()
    {
        Collider2D[] hitObjects = CheckRadiusForObject(TeleportRadius, LayerMask.NameToLayer("Player"));

        if (hitObjects.Length > 0)
        {
            return hitObjects[0].gameObject.GetComponent<IPlayer>();
        }

        return null;
    }

    protected override void ChangeState()
    {
        if (m_target == null)
        {
            if(m_currentState != State.NonAggro)
            {
                m_currentState = State.NonAggro;
                m_movesMade = 0;
                m_averageDamageTaken = 0;
            }
        }
        else
        {
            if(m_currentState != State.Aggro)
            {
                m_currentState = State.Aggro;
                m_movesMade = 0;
                m_averageDamageTaken = 0;
            }
        }
    }
    #endregion

    #region Coroutines
    public override IEnumerator PerformMove()
    {
        for(;;)
        {
            //Scan for Player and Change State
            m_target = CheckPlayerInSight();
            ChangeState();

            //Save current health and check against health after move
            float healthBeforeMove = m_life.CurrentHealth;

            Move selectedMove = ChooseMove();
            if(selectedMove != null)
            {
                selectedMove.Skill.InitAttack(selectedMove.AdjustEffectiveness);
                yield return new WaitForSeconds(selectedMove.Skill.CastTime);

                //Get health difference
                float damageTaken = healthBeforeMove - m_life.CurrentHealth;

                //Delegate Success or Fail to Mewtwo if applicable
                if(DelegateOperation != null)
                {
                    //Report Success if if damage taken is less than average
                    DelegateOperation(damageTaken < m_averageDamageTaken);
                    DelegateOperation = null;
                }

                //Set new Average Damage
                m_averageDamageTaken = (m_averageDamageTaken * m_movesMade + damageTaken) / (m_movesMade + 1);
                m_movesMade += 1;

                //DEBUG
                //Debug.Log("Average Damage: " + m_averageDamageTaken);
                //Debug.Log("Current HP: " + m_life.CurrentHealth);
                //

                //Normalize Moves
                NormalizeMoves();

                //Start Floaty if Disabled
                if(!GetComponent<FloatyBehaviour>().enabled)
                {
                    GetComponent<FloatyBehaviour>().enabled = true;
                }

                //Stop Moving
                gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector3(0, gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity.y, 0);
            }

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Associative Intent Balancer Functions
    //Balance Avoid *= 1 + #nearby-platforms * 0.05
    private void NearbyPlatforms_IntentBalancer(IntentIndex[] intents)
    {
        GameObject[] nearbyPlatforms = CheckPlatformsInSight();

        //Modify Avoid
        intents[1].Index *= (1 + nearbyPlatforms.Length * PlatformInfluence);
    }

    //Balance Chase *= 1.3
    private void TPRange_IntentBalancer(IntentIndex[] intents)
    {
        //Modify Chase
        if(CheckPlayerInTPRange() != null)
        {
            intents[2].Index *= TeleportInfluence;
        }
        
    }

    //Balance Attack/Avoid
    private void Cornering_IntentBalancer(IntentIndex[] intents)
    {
        if(m_target != null)
        {
            //Player is left of Mewtwo
            if(m_target.GetObject.transform.position.x < transform.position.x)
            {
                //Player is forcing Mewtwo into a wall
                //Increase Attack and Chase
                if(Mathf.Abs(RightWall.transform.position.x - transform.position.x) < CorneringInfluence)
                {
                    intents[0].Index *= 2;
                    intents[2].Index *= 2;
                }

                //Mewtwo is forcing player into a wall
                //Increase Avoid
                if(Mathf.Abs(LeftWall.transform.position.x - transform.position.x) < CorneringInfluence)
                {
                    intents[1].Index *= 2;
                }
            }

            //Player is right of Mewtwo
            else if(m_target.GetObject.transform.position.x > transform.position.x)
            {
                //Player is forcing Mewtwo into a wall
                //Increase Attack and Chase
                if (Mathf.Abs(LeftWall.transform.position.x - transform.position.x) < CorneringInfluence)
                {
                    intents[0].Index *= 2;
                    intents[2].Index *= 2;
                }

                //Mewtwo is forcing player into a wall
                //Increase Avoid
                if (Mathf.Abs(RightWall.transform.position.x - transform.position.x) < CorneringInfluence)
                {
                    intents[1].Index *= 2;
                }
            }
        }
    }

    //Add more modifiers here!
    #endregion

    private void AddIntentBalancers()
    {
        m_intentModifiers.Add(NearbyPlatforms_IntentBalancer);
        m_intentModifiers.Add(TPRange_IntentBalancer);
        m_intentModifiers.Add(Cornering_IntentBalancer);
    }
}
