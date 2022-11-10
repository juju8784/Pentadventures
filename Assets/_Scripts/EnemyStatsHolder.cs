using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsHolder : StatsHolder
{
    public EnemyRandomStatGenerator enemyRNG;

    public override void Start()
    {
        SetStats();
        movementLeft = movementRange;
        actionPoints = CalculateActionPoint();
        actionPointsLeft = actionPoints;
        fleeChance = CalculateActionFleeChance();
    }

    public override void SetStats()
    {
        // use this block to make different star levels spawn in different positions 
        // TODO
       // starLevel = enemyRNG.starLevel;


        int maxRange = starLevel * 1000;
        int minRange = (starLevel == 1) ? 500 : maxRange - 1000;
        int num = UnityEngine.Random.Range(minRange, maxRange);
        float t = (float)num / 100f;
        BaseStrength = (int)(t * (float)enemyRNG.strenghtPercentage);
        BaseDexterity = (int)(t * (float)enemyRNG.dexterityPercentage);
        BaseVitality = (int)(t * (float)enemyRNG.vitalityPercentage);
        BaseWisdom = (int)(t * (float)enemyRNG.wisdomPercentage);
        BaseIntelligence = (int)(t * (float)enemyRNG.intelligencePercentage);

    }
}
