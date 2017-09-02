using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MewtwoConfig : IBossConfig
{
    //Reference to AI
    public Mewtwo Instance;

    public MewtwoConfig(Mewtwo instance)
    {
        Instance = instance;
    }

    public IDictionary<AIBehaviour.State, IntentIndex[]> GetIntentConfig()
    {
        //Dictionary of all intents per state
        IDictionary<AIBehaviour.State, IntentIndex[]> intentDictionary = new Dictionary<AIBehaviour.State, IntentIndex[]>();

        //Non Aggro Intent
        AIBehaviour.State nonAggro = AIBehaviour.State.NonAggro;
        IntentIndex[] nonAggroIntentList = SetIntentBehaviour(0, 0, 0, 100);
        intentDictionary.Add(nonAggro, nonAggroIntentList);

        //Aggro Intent
        AIBehaviour.State Aggro = AIBehaviour.State.Aggro;
        IntentIndex[] AggroIntentList = SetIntentBehaviour(40, 30, 30, 0);
        intentDictionary.Add(Aggro, AggroIntentList);

        return intentDictionary;
    }

    private IntentIndex[] SetIntentBehaviour(int attack, int avoid, int chase, int discovery)
    {
        IntentIndex[] intentList = new IntentIndex[4];

        //Attack Intent
        IntentIndex attackIntent = new IntentIndex(AIBehaviour.Intent.Attack, attack);
        SetAttackMoves(attackIntent);
        intentList[0] = attackIntent;

        //Avoid Intent
        IntentIndex avoidIntent = new IntentIndex(AIBehaviour.Intent.Avoid, avoid);
        SetAvoidMoves(avoidIntent);
        intentList[1] = avoidIntent;

        //Chase Intent
        IntentIndex chaseIntent = new IntentIndex(AIBehaviour.Intent.Chase, chase);
        SetChaseMoves(chaseIntent);
        intentList[2] = chaseIntent;

        //Discovery Intent
        IntentIndex discoveryIntent = new IntentIndex(AIBehaviour.Intent.Discovery, discovery);

        intentList[3] = discoveryIntent;

        return intentList;
    }

    #region Intent Move Setters
    private void SetAttackMoves(IntentIndex attackInt)
    {
        Move disable = new Move(new Disable(Instance));
        attackInt.Moveset.Add(disable);

        Move melee = new Move(new Melee(Instance));
        attackInt.Moveset.Add(melee);

        Move kiBlast = new Move(new KiBlast(Instance));
        attackInt.Moveset.Add(kiBlast);
    }

    private void SetAvoidMoves(IntentIndex avoidInt)
    {
        Move walk = new Move(new Walk(Instance, Walk.MoveType.Avoid));
        avoidInt.Moveset.Add(walk);
    }

    private void SetChaseMoves(IntentIndex chaseInt)
    {
        Move walk = new Move(new Walk(Instance, Walk.MoveType.Chase));
        chaseInt.Moveset.Add(walk);
    }

    private void SetDiscoveryMoves(IntentIndex discoveryInt)
    {
        Move walk = new Move(new Walk(Instance, Walk.MoveType.Discover));
        discoveryInt.Moveset.Add(walk);
    }
    #endregion
}
