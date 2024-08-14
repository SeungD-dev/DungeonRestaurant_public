using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    private Dictionary<CharacterData, Dictionary<UpgradeType, int>> lastIncreaseAmount = new Dictionary<CharacterData, Dictionary<UpgradeType, int>>();
    private Dictionary<CharacterData, Dictionary<UpgradeType, float>> floatLastIncreaseAmount = new Dictionary<CharacterData, Dictionary<UpgradeType, float>>();

    public void StatUpgradeAllCharacters(UpgradeType upgradeType)
    {
        foreach (var character in DataManager.Instance.characterList)
        {
            if (upgradeType == UpgradeType.Atk || upgradeType == UpgradeType.Def || upgradeType == UpgradeType.Hp || upgradeType == UpgradeType.Resist || upgradeType == UpgradeType.CriticalDmg)
                RemoveLastIncrease(character, upgradeType);
            else
                RemoveLastIncreaseFloat(character, upgradeType);

            UpgradeStatType(character, upgradeType);
        }
    }

    public void UpgradeStatType(CharacterData character, UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.Atk || upgradeType == UpgradeType.Def || upgradeType == UpgradeType.Hp) // 공격력, 방어력, 체력
        {
            float increasePercentage = character.Stat.GetLevel(upgradeType) * 0.05f;
            int increaseAmount = Mathf.RoundToInt(character.Stat.GetIntStat(upgradeType) * increasePercentage);
            character.Stat.IncreaseStat(upgradeType, increaseAmount);

            SaveValue(character, upgradeType, increaseAmount);
        }
        else if (upgradeType == UpgradeType.Resist) // 저항
        {
            int increaseAmount = character.Stat.GetLevel(upgradeType) * 10;
            character.Stat.IncreaseStat(upgradeType, increaseAmount);

            SaveValue(character, upgradeType, increaseAmount);
        }
        else if (upgradeType == UpgradeType.CriticalDmg) // 크리 데미지
        {
            int increaseAmount = character.Stat.GetLevel(upgradeType) * 5;
            character.Stat.IncreaseStat(upgradeType, increaseAmount);

            SaveValue(character, upgradeType, increaseAmount);
        }
        else if (upgradeType == UpgradeType.CriticalPercent) // 크리 확률
        {
            float increaseAmount = character.Stat.GetLevel(upgradeType) * 0.05f;
            character.Stat.IncreaseStat(upgradeType, increaseAmount);

            SaveValue(character, upgradeType, increaseAmount);
        }
        else
        {
            float increasePercentage = character.Stat.GetLevel(upgradeType) * 0.05f;
            float increaseAmount = Mathf.Round(character.Stat.GetFloatStat(upgradeType) * increasePercentage * 100f) / 100f; // 공격 속도 소수점 두자리로 증가
            character.Stat.IncreaseStat(upgradeType, increaseAmount);

            SaveValue(character, upgradeType, increaseAmount);
        }
    }

    public void SaveValue(CharacterData character, UpgradeType upgradeType, int increaseAmount) // 증가한 값을 저장 (int)
    {
        if (!lastIncreaseAmount.ContainsKey(character))
            lastIncreaseAmount[character] = new Dictionary<UpgradeType, int>();

        lastIncreaseAmount[character][upgradeType] = increaseAmount;
    }

    public void SaveValue(CharacterData character, UpgradeType upgradeType, float increaseAmount) // 증가한 값을 저장 (float)
    {
        if (!floatLastIncreaseAmount.ContainsKey(character))
            floatLastIncreaseAmount[character] = new Dictionary<UpgradeType, float>();

        floatLastIncreaseAmount[character][upgradeType] = increaseAmount;
    }

    public void RemoveLastIncrease(CharacterData character, UpgradeType upgradeType) // 최근 증가한 값을 빼주는 것 (int)
    {
        if (lastIncreaseAmount.ContainsKey(character) && lastIncreaseAmount[character].ContainsKey(upgradeType))
        {
            int lastAmount = lastIncreaseAmount[character][upgradeType];
            character.Stat.IncreaseStat(upgradeType, -lastAmount);
        }
    }

    public void RemoveLastIncreaseFloat(CharacterData character, UpgradeType upgradeType) // 최근 증가한 값을 빼주는 것 (float)
    {
        if (floatLastIncreaseAmount.ContainsKey(character) && floatLastIncreaseAmount[character].ContainsKey(upgradeType))
        {
            float lastAmount = floatLastIncreaseAmount[character][upgradeType];
            character.Stat.IncreaseStat(upgradeType, -lastAmount);
        }
    }

    public void RemoveLastIncreaseAll(CharacterData character)
    {
        RemoveLastIncrease(character, UpgradeType.Atk);
        RemoveLastIncrease(character, UpgradeType.Def);
        RemoveLastIncrease(character, UpgradeType.Resist);
        RemoveLastIncrease(character, UpgradeType.Hp);
        RemoveLastIncreaseFloat(character, UpgradeType.AttackSpeed);
        RemoveLastIncrease(character, UpgradeType.CriticalDmg);
        RemoveLastIncreaseFloat(character, UpgradeType.CriticalPercent);
    }

    public void StatUpgradeAll(CharacterData character)
    {
        UpgradeStatType(character, UpgradeType.Atk);
        UpgradeStatType(character, UpgradeType.Def);
        UpgradeStatType(character, UpgradeType.Resist);
        UpgradeStatType(character, UpgradeType.Hp);
        UpgradeStatType(character, UpgradeType.AttackSpeed);
        UpgradeStatType(character, UpgradeType.CriticalPercent);
        UpgradeStatType(character, UpgradeType.CriticalDmg);
    }

    public int GetLastIncreaseAmount(CharacterData character, UpgradeType upgradeType) // int 증가치 저장
    {
        if (lastIncreaseAmount.ContainsKey(character) && lastIncreaseAmount[character].ContainsKey(upgradeType))
        {
            return lastIncreaseAmount[character][upgradeType];
        }
        return 0;
    }

    public float GetLastIncreaseAmountFloat(CharacterData character, UpgradeType upgradeType) // float 증가치 저장
    {
        if (floatLastIncreaseAmount.ContainsKey(character) && floatLastIncreaseAmount[character].ContainsKey(upgradeType))
        {
            return floatLastIncreaseAmount[character][upgradeType];
        }
        return 0f;
    }

    public void IncreaseStatLevel(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Atk:
                DataManager.Instance.UpgradeLevel.atkLevel++;
                break;
            case UpgradeType.Def:
                DataManager.Instance.UpgradeLevel.defLevel++;
                break;
            case UpgradeType.Resist:
                DataManager.Instance.UpgradeLevel.resistLevel++;
                break;
            case UpgradeType.Hp:
                DataManager.Instance.UpgradeLevel.hpLevel++;
                break;
            case UpgradeType.AttackSpeed:
                DataManager.Instance.UpgradeLevel.AttackSpeedLevel++;
                break;
            case UpgradeType.CriticalPercent:
                DataManager.Instance.UpgradeLevel.criticalPercentLevel++;
                break;
            case UpgradeType.CriticalDmg:
                DataManager.Instance.UpgradeLevel.criticalDmgLevel++;
                break;
            default: break;
        }
    }

    public void IncreaseMedalLevel(MedalUpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case MedalUpgradeType.entry:
                DataManager.Instance.UpgradeLevel.entryLevel++;
                DataManager.Instance.userInfo.MaxPartyNumber++;
                break;
            case MedalUpgradeType.employPrice:
                DataManager.Instance.UpgradeLevel.employPriceLevel++; // CharacterEmployment.cs에서 값 변환
                break; 
            case MedalUpgradeType.characterLvUpPrice:
                DataManager.Instance.UpgradeLevel.characterLvUpPriceLevel++; // LevelUp.cs에서 값 변환
                break;
            case MedalUpgradeType.rarePercent:
                DataManager.Instance.UpgradeLevel.rarePercentLevel++;
                DataManager.Instance.UpgradeLevel.UpdateRarePercent();
                break;
            case MedalUpgradeType.heroPercent:
                DataManager.Instance.UpgradeLevel.heroPercentLevel++;
                DataManager.Instance.UpgradeLevel.UpdateHeroPercent();
                break;
            case MedalUpgradeType.maxCharacterListCount:
                DataManager.Instance.UpgradeLevel.maxCharacterListCountLevel++;
                IncreaseMaxCharacterListCount();
                break;
            default: break;
        }
    }

    public float GetEmployPriceDiscount()
    {
        return 0.05f * DataManager.Instance.UpgradeLevel.employPriceLevel;
    }

    public float GetCharacterLevelUpPriceDiscount()
    {
        return 0.05f * DataManager.Instance.UpgradeLevel.characterLvUpPriceLevel;
    }

    public void IncreaseMaxCharacterListCount()
    {
        UserInfo.userInfo.maxCharacterListCount
            = 10 + 5 * DataManager.Instance.UpgradeLevel.maxCharacterListCountLevel;
    }
}
