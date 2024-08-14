using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EmploymentManager : Singleton<EmploymentManager>
{
    [Title("CharacterEmployment")]
    public List<CharacterData> employmentList = new List<CharacterData>();

    [Title("CheckEmployment")]
    public List<bool> isEmploymentAvailability = new List<bool>();

    [ShowInInspector] private List<CharacterInfo> commonCharacterList = new List<CharacterInfo>();
    [ShowInInspector] private List<CharacterInfo> rareCharacterList = new List<CharacterInfo>();
    [ShowInInspector] private List<CharacterInfo> heroCharacterList = new List<CharacterInfo>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitCharacterInfoList();
        GenerateEmploymentList();
    }

    private void InitCharacterInfoList()
    {
        commonCharacterList = DataManager.Instance.InfoDict.GetListByRank(eCharacterRank.Common);
        rareCharacterList = DataManager.Instance.InfoDict.GetListByRank(eCharacterRank.Rare);
        heroCharacterList = DataManager.Instance.InfoDict.GetListByRank(eCharacterRank.Hero);
    }

    public void GenerateEmploymentList()
    {
        CharacterNameList nameList = DataManager.Instance.NameList;
        employmentList.Clear();
        isEmploymentAvailability.Clear();
        for (int i = 0; i < GameManager.MAX_PARTY_NUMBER; i++)
        {
            int random = Random.RandomRange(0,DataManager.Instance.userInfo.UserLevel);
            CharacterInfo randomCharacter = CharacterRank();
            CharacterData character = new CharacterData(randomCharacter);
            character.characterName = nameList.GetRandomName(character.Info.Rank);
            if (random == 0) 
            {
                character.Level = 1;
            }
            while (character.Level <= random)
            {
                if (character.Level > 9)
                {
                    break;
                }
                character.LevelUp();
            }
            employmentList.Add(character);
            isEmploymentAvailability.Add(true);
            AssignRandomSkill(character);
            SetSkin(character);
        }
    }

    public void ReRoll()
    {
        GenerateEmploymentList();
    }


    private void AssignRandomSkill(CharacterData character)
    {
        BaseSkill randomSkill = SkillManager.Instance.GetRandomSkill(character.Info.CharacterClass);
        character.skill = randomSkill;
    }

    private void SetSkin(CharacterData character)
    {
        CharacterPrefabList[] skinList = DataManager.Instance.PrefabList;
        for (int i = 0; i < skinList.Length; i++)
        {
            if (skinList[i].characterClass == character.Info.CharacterClass)
            {
                int randomIdx = Random.Range(0, skinList[i].prefabList.Count);
                character.skin = skinList[i].prefabList[randomIdx];
                return;
            }
        }
    }

    private CharacterInfo CharacterRank()
    {
        float randomIdx = Random.Range(0, 1f);
        if (randomIdx < DataManager.Instance.UpgradeLevel.commonPercent)
        {
            int idx = Random.Range(0, commonCharacterList.Count);
            return commonCharacterList[idx];
        }
        else if (randomIdx < DataManager.Instance.UpgradeLevel.commonPercent + DataManager.Instance.UpgradeLevel.rarePercent)
        {
            int idx = Random.Range(0, rareCharacterList.Count);
            return rareCharacterList[idx];
        }
        else
        {
            int idx = Random.Range(0, heroCharacterList.Count);
            return heroCharacterList[idx];
        }
    }
}