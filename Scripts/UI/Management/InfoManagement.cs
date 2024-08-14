using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManagement : MonoBehaviour
{
    public CharacterData characterData;

    public TextMeshProUGUI stat;
    public TextMeshProUGUI level;
    public TextMeshProUGUI job;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI needGold;
    public TextMeshProUGUI characterExplanation;
    public GameObject infoThumnail;

    public int index;

    [SerializeField] private Currency currencyGold;

    [Header("스킬")]
    public Image skillImg;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;

    [Header("오브젝트")]
    public GameObject LVUpPanel;
    public GameObject firePanel;
    public GameObject warningPanel;

    private Camera _camera;
    private Canvas canvas;

    private int requireGold;

    private readonly Dictionary<int, int> levelUpCosts = new Dictionary<int, int>
    {
        {1, 1250}, {2, 3750}, {3, 5000}, {4, 6250}, {5, 7500},
        {6, 8750}, {7, 10000}, {8, 11250}, {9, 12500}
    };

    private void Awake()
    {
        _camera = Camera.main;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = _camera;
    }

    private void Start()
    {
        UserInfo.userInfo.SetGoldEvent(currencyGold.SetText);
    }

    private void OnDestroy()
    {
        UserInfo.userInfo.RemoveGoldEvent(currencyGold.SetText);
        characterData.RemoveLevelUpEvent(UpdateUI);
    }

    public void SetInfo(CharacterData characterData, int index) // 캐릭터 관리에서 캐릭트 확인할 때 데이터 값 업데이트
    {
        this.index = index;
        this.characterData = characterData;
        InitializeUI();
        characterData.SetLevelUpEvent(UpdateUI);
    }

    private void InitializeUI()
    {
        UpdateCharacterInfo();
        UpdateCharacterStats();
        UpdateSkill();
    }

    public void UpdateUI()
    {
        UpdateCharacterStats();
        LevelUpUpdateUI();
    }


    private void UpdateCharacterStats() // 스탯 관련 UI 업데이트
    {
        var upgradeManager = UpgradeManager.Instance;

        int hpIncrease = upgradeManager.GetLastIncreaseAmount(characterData, UpgradeType.Hp);
        int atkIncrease = upgradeManager.GetLastIncreaseAmount(characterData, UpgradeType.Atk);
        int defIncrease = upgradeManager.GetLastIncreaseAmount(characterData, UpgradeType.Def);
        int resistIncrease = upgradeManager.GetLastIncreaseAmount(characterData, UpgradeType.Resist);
        float attackSpeedIncrease = upgradeManager.GetLastIncreaseAmountFloat(characterData, UpgradeType.AttackSpeed);
        float criticalPercentIncrease = upgradeManager.GetLastIncreaseAmountFloat(characterData, UpgradeType.CriticalPercent);
        int criticalDmgIncrease = upgradeManager.GetLastIncreaseAmount(characterData, UpgradeType.CriticalDmg);


        stat.text = $"{characterData.Stat.HP} <color=green>(+{hpIncrease})</color>\n" +
                    $"{characterData.Stat.ATK} <color=green>(+{atkIncrease})</color>\n" +
                    $"{characterData.Stat.DEF} <color=green>(+{defIncrease})</color>\n" +
                    $"{characterData.Stat.Resistance} <color=green>(+{resistIncrease})</color>\n" +
                    $"{characterData.Stat.AttackSpeed:F2} <color=green>(+{attackSpeedIncrease:F2})</color>\n" +
                    $"{characterData.Stat.CriticalPercent * 100}% <color=green>(+{criticalPercentIncrease * 100}%)</color>\n" +
                    $"{characterData.Stat.CriticalDamage}% <color=green>(+{criticalDmgIncrease}%)</color>";
    }

    private void UpdateCharacterInfo() // 캐릭터 정보 업데이트
    {
        level.text = $"LV. {characterData.Level}";
        job.text = characterData.Info.ClassString;
        characterName.text = characterData.characterName;
        characterExplanation.text = characterData.Info.Description;

        levelUpCosts.TryGetValue(characterData.Level, out requireGold);
        float discount = UpgradeManager.Instance.GetCharacterLevelUpPriceDiscount();
        int discountedPrice = Mathf.RoundToInt(requireGold * (1 - discount));
        needGold.text = discountedPrice.ToString();

        GameObject instantiatedThumbnail = Instantiate(characterData.skin.characterIdle, infoThumnail.transform);
        instantiatedThumbnail.transform.localScale = new Vector3(250f, 250f, 1f);
        instantiatedThumbnail.transform.localPosition = new Vector2(0f, -30f);
    }

    private void UpdateSkill()
    {
        skillImg.sprite = characterData.skill.skillImg;
        skillName.text = characterData.skill.skillName;
        skillDescription.text = characterData.skill.skillDiscription;
    }

    public void YesLVUp() // 10렙이 되면 버튼 클릭 x
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (characterData.Level <= 9 && characterData.Level < UserInfo.userInfo.UserLevel)
        {
            if (levelUpCosts.TryGetValue(characterData.Level, out requireGold))
            {
                float discount = UpgradeManager.Instance.GetCharacterLevelUpPriceDiscount();
                int discountedPrice = Mathf.RoundToInt(requireGold * (1 - discount));
                //Debug.Log($"원래 가격: {requireGold}, 감소량: {discount * 100}%, 감소된 가격: {discountedPrice}");

                if (UserInfo.userInfo.Gold >= discountedPrice)
                {
                    characterData.LevelUp();
                    UpdateUI();
                    UserInfo.userInfo.Gold -= discountedPrice;
                }
                else
                {
                    Debug.Log("돈이 부족합니다..");
                }
            }
            else
            {
                Debug.Log("Max레벨 입니다.");
            }
        }
        else
        {
            Instantiate(warningPanel);
        }
    }

    //public void OnClickLVUpBtn()
    //{
    //    SoundManager.Instance.PlaySound("SFX_UI_Click");
    //    Instantiate(LVUpPanel);
    //}

    private void LevelUpUpdateUI()
    {
        level.text = $"LV. {characterData.Level}";

        levelUpCosts.TryGetValue(characterData.Level, out requireGold);
        float discount = UpgradeManager.Instance.GetCharacterLevelUpPriceDiscount();
        int discountedPrice = Mathf.RoundToInt(requireGold * (1 - discount));
        needGold.text = discountedPrice.ToString();
    }

    public void FireBtn()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        Instantiate(firePanel);
    }

    public void Fire()
    {
        Destroy(gameObject);
    }

    public void BackBtn()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        FindObjectOfType<ManageSlotInstance>().Refresh();
        Destroy(gameObject);
    }

}
