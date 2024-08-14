using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Atk,
    Def,
    Resist,
    Hp,
    AttackSpeed,
    CriticalPercent,
    CriticalDmg
}

[System.Serializable]
public class CharacterStat : StatBase
{
    protected CharacterInfo characterInfo;

    public CharacterStat(CharacterInfo info)
    {
        characterInfo = info;
        InitializeStat(info, 1);
    }

    public CharacterStat(CharacterInfo info, int level)
    {
        characterInfo = info;
        InitializeStat(info, level);
    }

    private void InitializeStat(CharacterInfo info, int level)
    {
        CharacterInitialStat stat = DataManager.Instance.InitStatDict.GetData(info.CharacterClass);

        hp = stat.HP;
        def = stat.DEF;
        atk = stat.ATK;
        resistance = stat.Resistance;
        criticalPercent = stat.CriticalPercent;
        mp = stat.MP;
        range = stat.Range;
        moveSpeed = stat.MoveSpeed;
        attackSpeed = stat.AttackSpeed;
        criticalDamege = stat.CriticalDamage;

        IncreaseStatOnLevelUp(level);
    }

    public void IncreaseStatOnLevelUp()
    {
        IncreaseStat(ref hp, Random.Range(characterInfo.MinHP, characterInfo.MaxHP + 1));
        IncreaseStat(ref atk, Random.Range(characterInfo.MinATK, characterInfo.MaxATK + 1));
        IncreaseStat(ref def, Random.Range(characterInfo.MinDEF, characterInfo.MaxDEF + 1));
        IncreaseStat(ref resistance, Random.Range(characterInfo.MinResistance, characterInfo.MaxResistance + 1));
        IncreaseStat(ref attackSpeed, Random.Range(characterInfo.MinAttackSpeed, characterInfo.MaxAttackSpeed));
    }

    private void IncreaseStatOnLevelUp(int level)
    {
        IncreaseStat(ref hp, Random.Range(characterInfo.MinHP * level, characterInfo.MaxHP * level + 1));
        IncreaseStat(ref atk, Random.Range(characterInfo.MinATK * level, characterInfo.MaxATK * level + 1));
        IncreaseStat(ref def, Random.Range(characterInfo.MinDEF * level, characterInfo.MaxDEF * level + 1));
        IncreaseStat(ref resistance, Random.Range(characterInfo.MinResistance * level, characterInfo.MaxResistance * level + 1));
        IncreaseStat(ref attackSpeed, Random.Range(characterInfo.MinAttackSpeed * level, characterInfo.MaxAttackSpeed * level));
    }

    public int GetLevel(UpgradeType upgradeType)
    {
        return upgradeType switch
        {
            UpgradeType.Atk => DataManager.Instance.UpgradeLevel.atkLevel,
            UpgradeType.Def => DataManager.Instance.UpgradeLevel.defLevel,
            UpgradeType.Resist => DataManager.Instance.UpgradeLevel.resistLevel,
            UpgradeType.Hp => DataManager.Instance.UpgradeLevel.hpLevel,
            UpgradeType.AttackSpeed => DataManager.Instance.UpgradeLevel.AttackSpeedLevel,
            UpgradeType.CriticalPercent => DataManager.Instance.UpgradeLevel.criticalPercentLevel,
            UpgradeType.CriticalDmg => DataManager.Instance.UpgradeLevel.criticalDmgLevel,
            _ => 0,
        };
    }

    public int GetIntStat(UpgradeType upgradeType)
    {
        return upgradeType switch
        {
            UpgradeType.Atk => atk,
            UpgradeType.Def => def,
            UpgradeType.Resist => resistance,
            UpgradeType.Hp => hp,
            UpgradeType.CriticalDmg => (int)criticalDamege,
            _ => 0,
        };
    }

    public float GetFloatStat(UpgradeType upgradeType)
    {
        return upgradeType switch
        {
            UpgradeType.AttackSpeed => attackSpeed,
            UpgradeType.CriticalPercent => criticalPercent,
            _ => 0,
        };
    }

    public void IncreaseStat(UpgradeType upgradeType, int amount)
    {
        switch (upgradeType)
        {
            case UpgradeType.Atk:
                atk += amount;
                break;
            case UpgradeType.Def:
                def += amount;
                break;
            case UpgradeType.Resist:
                resistance += amount;
                break;
            case UpgradeType.Hp:
                hp += amount;
                break;
            case UpgradeType.CriticalDmg:
                criticalDamege += amount;
                break;
        }
    }

    public void IncreaseStat(UpgradeType upgradeType, float amount)
    {
        switch (upgradeType)
        {
            case UpgradeType.AttackSpeed:
                attackSpeed += amount;
                break;
            case UpgradeType.CriticalPercent:
                criticalPercent += amount;
                break;
        }
    }

    public void IncreaseStat(ref int stat, int amount)
    {
        stat += amount;
    }
    public void IncreaseStat(ref float stat, float amount)
    {
        stat += amount;
    }
}