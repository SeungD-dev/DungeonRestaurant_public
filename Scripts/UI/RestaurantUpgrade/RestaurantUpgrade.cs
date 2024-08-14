using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantUpgrade : MonoBehaviour
{
    public GameObject StatUpgradeInfo;
    public GameObject MedalUpgradeInfo;

    [Title("UI")]
    public GameObject RestaurantUpgradeUI;
    public GameObject ReputeUpgradeUI;

    [Title("¹öÆ°")]
    public GameObject RestaurantUpgradeBtnActive;
    public GameObject RestaurantUpgradeBtnInActive;
    public GameObject ReputeUpgradeBtnActive;
    public GameObject ReputeUpgradeBtnInActive;

    public List<GameObject> Lock;

    private void Awake()
    {
        if (DataManager.Instance.UpgradeLevel.entryLevel >= 3)
        {
            foreach (var obj in Lock)
            {
                obj.SetActive(false);
            }
        }
    }

    public void Btn(bool active)
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        RestaurantUpgradeUI.SetActive(!active);
        RestaurantUpgradeBtnActive.SetActive(!active);
        RestaurantUpgradeBtnInActive.SetActive(active);

        ReputeUpgradeUI.SetActive(active);
        ReputeUpgradeBtnActive.SetActive(active);
        ReputeUpgradeBtnInActive.SetActive(!active);
    }

    public void RestaurantUpgradeBtn()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        Btn(false);
    }

    public void ReputeUpgradeBtn()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (!DataManager.Instance.userInfo.isUserTutorials && ToturialsManager.Instance.phase == 27)
        {
            ToturialsManager.Instance.isClear[11] = true;
        }
        Btn(true);
    }

    public void BackBtn()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (StatUpgradeInfo.activeSelf == true || MedalUpgradeInfo.activeSelf == true)
        {
            StatUpgradeInfo.SetActive(false);
            MedalUpgradeInfo.SetActive(false);
        }
        else gameObject.SetActive(false);
    }

    public void CheckEntryLevel()
    {
        if (DataManager.Instance.UpgradeLevel.entryLevel >= 3)
        {
            foreach (var obj in Lock)
            {
                obj.SetActive(false);
            }
        }
    }
}
