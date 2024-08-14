using UnityEngine;

public class StatUpgradeBtn : MonoBehaviour
{
    public RestaurantUpgrade restaurantUpgrade;
    public UpgradeType upgradeType;
    public StatUpgradeInfo selectedStatInfo;

    public void SetSelectedStatInfo(StatUpgradeInfo statInfo)
    {
        selectedStatInfo = statInfo;
        SetUpgradeType(statInfo.upgradeType);
    }

    public void SetUpgradeType(UpgradeType upgradeType)
    {
        this.upgradeType = upgradeType;
    }

    public void StatLevelUp()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        int currentLevel = selectedStatInfo.GetCurrentLevel();
        int maxLevel = selectedStatInfo.statInfo.maxLevel;
        int requiredGold = selectedStatInfo.GetRequiredGold(currentLevel + 1);

        if (currentLevel >= maxLevel)
        {
            Debug.Log("최대 레벨");
            return;
        }

        if (UserInfo.userInfo.Gold < requiredGold)
        {
            Debug.Log("돈 부족.");
            return;
        }

        UpgradeGold(requiredGold);
        UpgradeManager.Instance.IncreaseStatLevel(upgradeType);

        foreach (var character in DataManager.Instance.characterList)
        {
            switch (upgradeType)
            {
                case UpgradeType.Atk:
                case UpgradeType.Def:
                case UpgradeType.Hp:
                case UpgradeType.Resist:
                case UpgradeType.AttackSpeed:
                case UpgradeType.CriticalDmg:
                case UpgradeType.CriticalPercent:
                    UpgradeManager.Instance.StatUpgradeAllCharacters(upgradeType);
                    break;
                default: break;
            }
        }

        selectedStatInfo?.UpdateUI(upgradeType);
        selectedStatInfo?.StatInfoSet(upgradeType);
    }

    public void UpgradeGold(int amount)
    {
        if (UserInfo.userInfo.Gold >= amount)
        {
            UserInfo.userInfo.Gold -= amount;
        }
    }
}
