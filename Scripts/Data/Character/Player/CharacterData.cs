using System;
using UnityEngine;


[Serializable]
public class CharacterData
{
    #region Fields
    public string characterName;

    [SerializeField] private int level = 1;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            OnLevelUp?.Invoke();
        }
    }
    public delegate void LevelUpEventHandler();
    protected event LevelUpEventHandler OnLevelUp;

    [SerializeField] private CharacterInfo info;
    public CharacterInfo Info { get { return info; } set { info = value; } }

    [SerializeField] private CharacterStat stat = null;
    public CharacterStat Stat { get { return stat; } set { stat = value; } }

    public BaseSkill skill;

    public CharacterPrefab skin;
    #endregion

    public CharacterData(CharacterInfo info)
    {
        this.info = info;
        stat = new CharacterStat(info);
        OnLevelUp += stat.IncreaseStatOnLevelUp;
    }

    public CharacterData(CharacterInfo info, int level)
    {
        this.info = info;
        stat = new CharacterStat(info, level);
        OnLevelUp += stat.IncreaseStatOnLevelUp;
    }

    public CharacterData(CharacterDataSerializable data)
    {
        Deserialization(data);
    }

    public void LevelUp()
    {
        // 레벨업 전에 강화로 인한 스탯 초기화
        UpgradeManager.Instance.RemoveLastIncreaseAll(this);
        if (!DataManager.Instance.userInfo.isUserTutorials && ToturialsManager.Instance.phase == 35)
        {
            ToturialsManager.Instance.isClear[15] = true;
        }
        Level++;

        // 레벨업 후 다시 강화로 인한 스탯 적용
        UpgradeManager.Instance.StatUpgradeAll(this);
    }

    public void SetLevelUpEvent(LevelUpEventHandler action)
    {
        OnLevelUp += action;
    }

    public void RemoveLevelUpEvent(LevelUpEventHandler action)
    {
        OnLevelUp -= action;
    }

    public CharacterDataSerializable Serialization()
    {
        return new CharacterDataSerializable
        {
            characterName = this.characterName,
            level = this.level,
            characterInfoRcode = this.info != null ? this.info.Rcode : null,
            stat = this.stat,
            skillRcode = this.skill != null ? this.skill.rcode : null,
            skinRcode = this.skin != null ? this.skin.rcode : null
        };
    }

    public void Deserialization(CharacterDataSerializable serializable)
    {
        this.characterName = serializable.characterName;
        this.level = serializable.level;
        this.Info = DataManager.Instance.InfoDict.GetData(serializable.characterInfoRcode);
        this.stat = serializable.stat;
        this.skill = SkillManager.Instance.GetSkill(Info.CharacterClass, serializable.skillRcode);
        this.skin = DataManager.Instance.GetPrefab(Info.CharacterClass, serializable.skinRcode);
    }

}

[Serializable]
public class CharacterDataSerializable
{
    public string characterName;
    public int level;
    public string characterInfoRcode;
    public CharacterStat stat;
    public string skillRcode;
    public string skinRcode;
}