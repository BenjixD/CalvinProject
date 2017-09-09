using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    #region Public Members
    public int Effectiveness;
    public int Normal;
    public ISkill Skill;
    #endregion

    #region Private Members
    private int m_adjustmentValue = 5; 
    #endregion

    public Move(ISkill skill, int effectiveness = 50)
    {
        if(effectiveness < 0 || effectiveness > 100)
        {
            effectiveness = 0;
        }

        Effectiveness = effectiveness;
        Normal = effectiveness;
        Skill = skill;
    }

    #region Forwarded Calls
    public bool IsReady()
    {
        return Skill.IsReady();
    }
    #endregion

    #region Move Statistics Handlers
    public void NormalizeEffectiveness(int normal)
    {
        int change = (normal - Effectiveness) / 10;
        Effectiveness += change;
    }

    public void AdjustEffectiveness(bool success)
    {
        if(success)
        {
            Effectiveness += m_adjustmentValue;
        }
        else
        {
            Effectiveness -= m_adjustmentValue;
        }

        //DEBUG
        //Debug.Log("Effectiveness: " + Effectiveness + " - " + Skill.GetType().Name);
        //
    }
    #endregion
}

public class IntentIndex
{
    #region Public Members
    public AIBehaviour.Intent Intent;
    public float Index;

    //List of move classes that inherit from ISkill 
    public IList<Move> Moveset;
    #endregion

    #region Private Members
    private float m_baseIndex;
    #endregion

    #region Constructors
    public IntentIndex(AIBehaviour.Intent intent, float index)
    {
        Intent = intent;
        Index = index;
        m_baseIndex = index;
        Moveset = new List<Move>();
    }
    #endregion

    #region Helper Functions
    public void ResetIndex()
    {
        Index = m_baseIndex;
    }
    #endregion
}

public class AIBehaviour : MonoBehaviour {

    #region Public Members
    public IDictionary<State, IntentIndex[]> Intents;
    public float VisionRadius;
    public IBossConfig Config;
    public Vector3 HorizontalOffset;
    #endregion

    #region Protected Members
    protected IPlayer m_target;
    protected Rigidbody2D m_rigidbody;
    protected bool m_facingRight;
    protected State m_currentState;

    //Lambda of Intent Index Modifiers
    public IList<Action<IntentIndex[]>> m_intentModifiers;
    #endregion
    
    #region Properties
    public bool IsFacingRight
    {
        get
        {
            return m_facingRight;
        }
    }

    public State GetCurrentState
    {
        get
        {
            return m_currentState;
        }
    }

    public IPlayer GetTarget
    {
        get
        {
            return m_target;
        }
    }
    #endregion

    #region Enums
    public enum State
    {
        NonAggro = 0,
        Aggro = 1
    };

    public enum Intent
    {
        Attack = 0,
        Avoid = 1,
        Chase = 2,
        Discovery = 3
    }
    #endregion

    #region Initialization
    protected void Initialize()
    {
        Intents = new Dictionary<State, IntentIndex[]> ();
        m_intentModifiers = new List<Action<IntentIndex[]>>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_currentState = State.NonAggro;
    }
    #endregion

    #region Helper Functions
    public void Flip()
    {
        //Flip the character Model
        m_facingRight = !m_facingRight;

        //Set the new localScale to be inverted
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (m_facingRight)
        {
            transform.position += HorizontalOffset;
        }
        else
        {
            transform.position -= HorizontalOffset;
        }
    }
    protected void HandleFlip()
    {
        //Facing Directions
        if (m_rigidbody.velocity.x < 0 && m_facingRight) //Moving Left, facing right (flip)
        {
            Flip();
        }
        else if (m_rigidbody.velocity.x > 0 && !m_facingRight)
        {
            Flip();
        }
    }

    //Basic AI Cognitive Data
    protected Collider2D[] CheckRadiusForObject(float radius, LayerMask layerMasks)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMasks);
        return hitColliders;
    }
    protected IPlayer CheckPlayerInSight()
    {
        LayerMask player = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("NonPlatformInteractor"));
        Collider2D[] hitObjects = CheckRadiusForObject(VisionRadius, player);
     
        if(hitObjects.Length > 0)
        {
            return hitObjects[0].gameObject.GetComponent<IPlayer>();
        }

        return null;
    }
    protected GameObject[] CheckPlatformsInSight()
    {
        LayerMask platform = 1 << LayerMask.NameToLayer("Platform");
        Collider2D[] hitObjects = CheckRadiusForObject(VisionRadius, platform);
        GameObject[] platforms = new GameObject[hitObjects.Length];

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            platforms[i] = hitObjects[i].gameObject;
        }

        return platforms;
    }
    protected bool IsTargetFacingRight()
    {
        bool isFacingRight = false;
        if(m_target != null)
        {
            isFacingRight = m_target.IsFacingRight;
        }
        return isFacingRight;
    }

    //Intent Manager Functions
    protected virtual void ChangeState()
    {
        if(m_target == null)
        {
            m_currentState = State.NonAggro;
        }
        else
        {
            m_currentState = State.Aggro;
        }
    }
    protected void RunIntentModifiers()
    {
        //Choose which intent set to modify
        IntentIndex[] intents = m_currentState == State.Aggro ? Intents[State.Aggro] : Intents[State.NonAggro];

        foreach (Action<IntentIndex[]> mod in m_intentModifiers)
        {
            mod(intents);
        }

        //Get total sum of intent index
        float indexTotal = 0.0f;
        foreach(IntentIndex intent in intents)
        {
            indexTotal += intent.Index;
        }

        //Set the total sum back to 100
        foreach(IntentIndex intent in intents)
        {
            intent.Index = intent.Index / indexTotal * 100;
        }

        //DEBUG//
        //PrintIntentValues();
        //
    }
    protected void ResetIntentIndex()
    {
        foreach(KeyValuePair<State, IntentIndex[]> intents in Intents )
        {
            foreach (IntentIndex intent in intents.Value)
            {
                intent.ResetIndex();
            }
        }
    }

    protected void NormalizeMoves()
    {
        foreach (KeyValuePair<State, IntentIndex[]> intents in Intents)
        {
            foreach (IntentIndex intent in intents.Value)
            {
                foreach (Move move in intent.Moveset)
                {
                    move.NormalizeEffectiveness(move.Normal);
                }
            }
        }
    }
    #endregion

    #region Load AI Configurations
    public void LoadIntents(IBossConfig bossConfig)
    {
        Intents = bossConfig.GetIntentConfig();
    }
    #endregion

    #region Move Chooser
    protected virtual Move ChooseMove()
    {
        IList<KeyValuePair<float, Move>> bestMoves = new List<KeyValuePair<float, Move>>();
        Move bestMove = null;

        //Reset Intent and then Run Intent Modifiers
        ResetIntentIndex();
        RunIntentModifiers();

        //Get the best moves from each intent
        foreach(IntentIndex intent in Intents[m_currentState])
        {
            Move bestMoveInIntent = GetBestMoveInIntent(intent);
            if(bestMoveInIntent != null)
            {
                bestMoves.Add(new KeyValuePair<float, Move>(CalculateScore(intent.Index, bestMoveInIntent.Effectiveness), bestMoveInIntent));
            }
        }

        //Find the best move
        float score = -1;
        foreach (KeyValuePair<float, Move> move in bestMoves)
        {
            if(move.Key > score)
            {
                bestMove = move.Value;
                score = move.Key;
            }
        }
        return bestMove;
    }

    //Get the current best move in the intent that is Usable & Ready
    protected virtual Move GetBestMoveInIntent(IntentIndex intent)
    {
        Move bestMove = null;
        foreach(Move move in intent.Moveset)
        {
            if(move.Skill.IsReady() && move.Skill.IsUsable())
            {
                if (bestMove == null || bestMove.Effectiveness < move.Effectiveness)
                {
                    bestMove = move;
                }
            }
        }
        return bestMove;
    }
    protected virtual float CalculateScore(float intentScore, float moveScore)
    {
        return intentScore * moveScore;
    }

    #endregion

    #region Coroutines
    public virtual IEnumerator PerformMove()
    {
        yield return null;
    }

    public virtual IEnumerator DecrementCooldowns()
    {
        for(;;)
        {
            foreach (KeyValuePair<State, IntentIndex[]> intents in Intents)
            {
                foreach (IntentIndex intent in intents.Value)
                {
                    foreach (Move move in intent.Moveset)
                    {
                        move.Skill.DecrementCooldown(0.1f);
                    }
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region Debug
    protected void PrintIntentValues()
    {
        Debug.Log("Attack: " + Intents[m_currentState][0].Index);
        Debug.Log("Avoid: " + Intents[m_currentState][1].Index);
        Debug.Log("Chase: " + Intents[m_currentState][2].Index);
        Debug.Log("Discovery: " + Intents[m_currentState][3].Index);
    }
    #endregion
}
