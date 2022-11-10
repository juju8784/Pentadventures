using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHolder : MonoBehaviour
{
    //Base Stats, Final Stat
    [SerializeField] private int[] dexterity = new int[2];
    [SerializeField] private int[] strength = new int[2];
    [SerializeField] private int[] vitality = new int[2];
    [SerializeField] private int[] wisdom = new int[2];
    [SerializeField] private int[] intelligence = new int[2];

    public bool poisoned = false;
    public int poisonEffect;
    public int poisonDamage;
    public int poisonLast;
    public bool slowed = false;
    public int slowEffect;
    public bool bleeding = false;
    public int bleedEffect;
    public int bleedDamage;
    public int bleedLast;
    public bool stunned = false;
    public int stunEffect;
    public int stunLast;
    public bool burning = false;
    public int burnEffect;
    public int burnDamage;
    public int burnLast;

    [SerializeField] protected int movementRange;
    [SerializeField] protected int movementRangeCurrent;
    [SerializeField] protected int movementLeft;
    [SerializeField] protected int starLevel;
    protected int spirit;
    protected int actionPoints;
    protected int actionPointsLeft;
    protected int actionPointsRecovery;
    public float fleeChance;

    public CharaStatBlock block;

    #region Properties
    public int BaseDexterity
    { 
        get
        {
            return dexterity[0];
        }
        set
        {
            dexterity[0] = value;
        }
    }
    public int BaseStrength
    {
        get
        {
            return strength[0];
        }
        set
        {
            strength[0] = value;
        }
    }
    public int BaseVitality
    {
        get
        {
            return vitality[0];
        }
        set
        {
            vitality[0] = value;
        }
    }
    public int BaseWisdom
    {
        get
        {
            return wisdom[0];
        }
        set
        {
            wisdom[0] = value;
        }
    }
    public int BaseIntelligence
    {
        get
        {
            return intelligence[0];
        }
        set
        {
            intelligence[0] = value;
        }
    }
    public int BaseSpirit
    {
        get
        {
            return spirit;
        }
        set
        {
            spirit = value;
        }
    }

    public int APointsLeft
    {
        get
        {
            return actionPointsLeft;
        }
        set
        {
            actionPointsLeft = value;
        }
    }

    public int APointsRecoveryRate
    {
        get
        {
            return actionPointsRecovery;
        }
        set
        {
            actionPointsRecovery = value;
        }
    }
    public int TotalStrength
    {
        get
        {
            return strength[1];
        }
        set
        {
            strength[1] = value;
        }
    }
    public int TotalDexterity
    {
        get
        {
            return dexterity[1];
        }
        set
        {
            dexterity[1] = value;
        }
    }
    public int TotalVitality
    {
        get
        {
            return vitality[1];
        }
        set
        {
            vitality[1] = value;
        }
    }
    public int TotalWisdom
    {
        get
        {
            return wisdom[1];
        }
        set
        {
            wisdom[1] = value;
        }
    }
    public int TotalIntelligence
    {
        get
        {
            return intelligence[1];
        }
        set
        {
            intelligence[1] = value;
        }
    }

    public int StarLevel
    {
        get
        {
            return starLevel;
        }
        set
        {
            starLevel = value;
        }
    }

    public int HitRate
    {
        get
        {
            return starLevel;
        }
    }

    #endregion

    public virtual void Start()
    {
        if (GetComponent<TestCharacterController>())
        {
            for (int i = 0; i < GameManager.instance.partyInfoBlock.members.Count; i++)
            {
                if (block.characterName == GameManager.instance.partyInfoBlock.members[i].characterName)
                {
                    block = GameManager.instance.partyInfoBlock.members[i];
                    break;
                }
            }
        }
        movementLeft = movementRange;
        if (block)
        {
            block.SetStatsHolder(this);
        }
        actionPoints = CalculateActionPoint();
        actionPointsLeft = actionPoints;
    }

    public virtual void SetStats()
    {
        if (GetComponent<TestCharacterController>())
        {
            for (int i = 0; i < GameManager.instance.partyInfoBlock.members.Count; i++)
            {
                if (block.characterName == GameManager.instance.partyInfoBlock.members[i].characterName)
                {
                    block = GameManager.instance.partyInfoBlock.members[i];
                    break;
                }
            }
        }
        if (block)
        {
            block.SetStatsHolder(this);
        }
        actionPoints = CalculateActionPoint();
        fleeChance = CalculateActionFleeChance();
        //actionPointsLeft = actionPoints;
    }

    public void ResetActionPointsNewCombat()
    {
        actionPoints = CalculateActionPoint();
        actionPointsLeft = actionPoints;
    }

    public int GetTotalMovementRange()
    {
        if (slowed)
        {
            return SlowReduceMovement(slowEffect);
        }
        else
        {
            return movementRange;
            //return CalculateMovement();
        }
    }

    public int GetMovementLeft()
    {
        return movementLeft;
    }

    public bool Move(int numTiles)
    {
        if (movementLeft >= numTiles)
        {
            movementLeft -= numTiles;
            return true;
        }
        return false;
    }

    public bool CanMove()
    {
        return (movementLeft > 0);
    }

    public void ResetMovement()
    {
        if (slowed)
        {
            movementLeft = SlowReduceMovement(slowEffect);
        }
        else
        {
            movementLeft = movementRange;
            //movementLeft = CalculateMovement();
        }
    }

    public void TurnReset()
    {
        ResetMovement();
        APointsLeft += actionPointsRecovery;
        if(APointsLeft > actionPoints)
        {
            APointsLeft = actionPoints;
        }
    }

    public bool CheckForPoints(int pointsToBeTaken)
    {
        if (pointsToBeTaken > actionPointsLeft)
        {
            return false;
        }
        return true;
    }

    public bool SubtractActionPoints(int pointsToBeTaken)
    {
        if(pointsToBeTaken > actionPointsLeft)
        {
            return false;
        }
        actionPointsLeft -= pointsToBeTaken;
        return true;
    }

    public void ReduceMovementLeft(int removeLeft)
    {
        movementLeft -= removeLeft;
        if(movementLeft < 0)
        {
            movementLeft = 0;
        }
    }
    public void AddMovementLeft(int addLeft)
    {
        movementLeft += addLeft;
    }
    public int SlowReduceMovement(int reduceValue)
    {
        float reduce = ((float)(reduceValue * movementRange) / (float)100);
        reduce = (reduce < 1) ? 1 : reduce;
        int move = movementRange - (int)reduce;
        if (move < 0)
        {
            move = 0;
        }
        if (movementLeft > move)
        {
            movementLeft = move;
        }
        return move;
    }
    public void AddSlowEffect(int percentage)
    {
        slowed = true;
        slowEffect = percentage;
    }
    public void RemoveSlowEffect()
    {
        slowed = false;
        slowEffect = 0;
    }

    public void AddPoisonEffect(int percentage)
    {
        poisoned = true;
        poisonEffect = percentage;
    }
    public void RemovePoisonEffect()
    {
        poisoned = false;
        poisonEffect = 0;
    }

    #region Calculations

    //This is going to get more complex but for now it's fine

    private float GetModifier(int value)
    {
        if (value >= 1000)
        {
            return 2;
        }
        if (value >= 900)
        {
            return 1.9f;
        }
        if (value >= 800)
        {
            return 1.8f;
        }
        if (value >= 700)
        {
            return 1.7f;
        }
        if (value >= 600)
        {
            return 1.6f;
        }
        if (value >= 500)
        {
            return 1.5f;
        }
        return 1;
    }


    //hp
    #endregion
    public int CalculateHealth()
    {
        int Health = (int)((float)BaseVitality * GetModifier(BaseVitality) * 2);
        //add item calculations here
        return Health;
    }
   
    //physical
    public int CalculatePhyDamage()
    {
        int attackDamage = (int)((float)BaseStrength * GetModifier(BaseStrength));
        //add item calculations here
        return attackDamage;
    }
 

    //int
    public int CalculateIntDamage()
    {
        int MagicDmg = (int)((float)BaseIntelligence * GetModifier(BaseIntelligence));
        //add item calculations here
        return MagicDmg;
    }

    //Dex
    public int CalculateEvasion()
    {
        int DodgeChance = (int)((float)BaseDexterity * 0.050f);
        if (BaseDexterity > 1000)
        {
            DodgeChance = (int)((float)1000 * 0.03);
        }
        //add item calculations here
        return DodgeChance;
    }
    public int CalculateMovement()
    {
        //if (BaseDexterity >= 500)
        //{
        //    movementRange = (int)(BaseDexterity * GetModifier(BaseDexterity) * 0.01) + 3;
        //}
        //else
        //{
        //    movementRange = 3;
        //}
        //int Movement = (int)(BaseDexterity * 0.01f);
        //add item calculations here
        return movementRange;
    }


    //wisdom
    public int CalculateCritChance()
    {
        int CritChance = (int)((float)BaseWisdom * 0.075f);
        //add item calculations here
        return CritChance;
    }
    public int CalculatePhyCritDamage()
    {
        int CritDamage = (int)((float)CalculatePhyDamage() * 2.75f);
        //add item calculations here
        return CritDamage;
    }

    public int CalculateMagCritDamage()
    {
        int CritDamage = (int)((float)CalculateIntDamage() * 2.75f);
        //add item calculations here
        return CritDamage;
    }
    public int CalculateRegenAction()
    {
        int RegenAct = (int)((float)BaseWisdom * GetModifier(BaseWisdom) * 0.005);
        //add item calculations here
        return RegenAct;
    }

    public int CalculateMR()
    {
        int MR = (int)((float)BaseIntelligence * 0.7f + ((float)BaseVitality * 0.3f));
        //add item calculations here
        return MR;
    }
    public int CalculateArmor()
    {
        int AR = (int)((float)BaseVitality * 0.7f + ((float)BaseStrength * 0.3f));
        //add item calculations here
        return AR;
    }

    //spirit
    public int CalculateActionPoint()
    {
        int Action = (int)(BaseSpirit * 0.01f);
        //add item calculations here
        if (BaseSpirit>899)
        {
            Action += 5;
        }
        else if (BaseSpirit > 799)
        {
            Action += 4;
        }
        else if (BaseSpirit > 699)
        {
            Action += 3;
        }
        else if (BaseSpirit > 599)
        {
            Action += 2;
        }
        else if (BaseSpirit > 499)
        {
            Action += 1;
        }
        return Action;
    }
    public int CalculateActionFleeChance()
    {
        int FleeChance = (int)((float)BaseDexterity * 0.001f);
        return FleeChance;
    }

    public void IncreaseFleeChance()
    {
        fleeChance += 0.1f;
    }
}
