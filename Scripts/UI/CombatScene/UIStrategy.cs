using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStrategy : UIBase
{
    #region Fields
    public static UIStrategy Instance;

    [SerializeField] public UIMainEntry mainEntry;
    [SerializeField] public UISubEntry subEntry;
    [SerializeField] UIBase warningMessgeUI;
    public DragSlotMainEntry dragSlotMain;
    public DragSlotSubEntry dragSlotSub;

    [SerializeField] TMP_Text entryCountText;
    [SerializeField] TMP_Text maxEntryNumberText;

    [SerializeField] Button startButton;

    private string slashString = "/ ";
    StringBuilder stringBuilder = new StringBuilder();

    private int entryCount = 0;
    public int EntryCount
    {
        get { return entryCount; }
        set
        {
            entryCount = value;
            onEntryCountChange?.Invoke();
        }
    }

    delegate void OnEntryCountChange();
    event OnEntryCountChange onEntryCountChange;
    #endregion

    #region Unity Lifecycle Functions

    private void Awake()
    {
        Instance = this;
        onEntryCountChange += SetEntryCountText;
        onEntryCountChange += SetTextColor;
        onEntryCountChange += SetButtonInteratable;
        SetButtonInteratable();

        maxEntryNumberText.text = stringBuilder.Append(slashString).Append(UserInfo.userInfo.MaxPartyNumber).ToString();
        stringBuilder.Clear();
    }

    private void Start()
    {
        UserInfo.userInfo.SetMaxPartyNumberEvent(SetMaxPartyNumberText);
        UserInfo.userInfo.SetMaxPartyNumberEvent(SetTextColor);
    }

    private void OnDestroy()
    {
        UserInfo.userInfo.RemoveMaxPartyNumberEvent(SetMaxPartyNumberText);
        UserInfo.userInfo.RemoveMaxPartyNumberEvent(SetTextColor);
    }
    #endregion

    #region Entry Logic Functions
    public void Check()
    {
        CheckProcess(mainEntry.selectedSlot, subEntry.selectedSlot);
    }

    public void Check(SlotMainEntry mainEntrySlot, SlotSubEntry subEntrySlot)
    {
        CheckProcess(mainEntrySlot, subEntrySlot);
    }

    private void CheckProcess(SlotMainEntry mainEntrySlot, SlotSubEntry subEntrySlot)
    {
        if (mainEntrySlot != null && subEntrySlot != null)
        {
            if (subEntrySlot.IsRegistered) return;

            if (mainEntrySlot.IsSlotEmpty() == false)
            {
                ChangeEntry(mainEntrySlot, subEntrySlot);
            }
            else
            {
                if (entryCount >= UserInfo.userInfo.MaxPartyNumber)
                {
                    mainEntrySlot.toggle.isOn = false;
                    subEntrySlot.toggle.isOn = false;
                    EmptySelectedSlot();
                    return;
                }
                RegisterEntry(mainEntrySlot, subEntrySlot);
            }
        }
    }

    public void RegisterEntry(SlotMainEntry mainEntrySlot, SlotSubEntry subEntrySlot)
    {
        mainEntrySlot.CharacterData = subEntrySlot.CharacterData;
        mainEntrySlot.SubEntryIndex = subEntrySlot.Index;
        subEntrySlot.OnEntryRegister();
        EntryCount++;
        mainEntrySlot.toggle.isOn = false;
        subEntrySlot.toggle.isOn = false;
        EmptySelectedSlot();
    }

    public void ChangeEntry(SlotMainEntry mainEntrySlot, SlotSubEntry subEntrySlot)
    {
        UnRegisterEntry(mainEntrySlot);
        RegisterEntry(mainEntrySlot, subEntrySlot);
    }

    public void UnRegisterEntry(SlotMainEntry mainEntrySlot)
    {
        if (mainEntrySlot.CharacterData == null) return;
        SlotSubEntry subEntrySlot = subEntry.slots[mainEntrySlot.SubEntryIndex];
        subEntrySlot.OnEntryUnRegister();
        mainEntrySlot.CharacterData = null;
        mainEntrySlot = null;
        EntryCount--;
    }

    public void EmptyEntry()
    {
        foreach (SlotMainEntry slot in mainEntry.slots)
        {
            UnRegisterEntry(slot);
        }
        SlotSelectCancel(mainEntry.selectedSlot);
    }

    public void SlotSelectCancel(EntrySlotBase slot)
    {
        if (slot == null) return;
        slot.toggle.isOn = false;
    }

    public void EmptySelectedSlot()
    {
        if (mainEntry.selectedSlot != null)
        {
            mainEntry.selectedSlot.toggle.isOn = false;
            mainEntry.selectedSlot = null;
        }
        if (subEntry.selectedSlot != null)
        {
            subEntry.selectedSlot.toggle.isOn = false;
            subEntry.selectedSlot = null;
        }
    }

    public void SwapEntryPosition(SlotMainEntry slot1, SlotMainEntry slot2)
    {
        CharacterData tempData = slot1.CharacterData;
        int tempIndex = slot1.SubEntryIndex;

        slot1.CharacterData = slot2.CharacterData;
        slot1.SubEntryIndex = slot2.SubEntryIndex;

        slot2.CharacterData = tempData;
        slot2.SubEntryIndex = tempIndex;
    }
    #endregion

    #region Set UI Functions
    void SetEntryCountText()
    {
        entryCountText.text = entryCount.ToString();
    }

    void SetTextColor()
    {
        if (entryCount < UserInfo.userInfo.MaxPartyNumber)
        {
            entryCountText.color = Color.white;
            maxEntryNumberText.color = Color.white;
        }
        else
        {
            entryCountText.color = Color.red;
            maxEntryNumberText.color = Color.red;
        }
    }

    void SetTextColor(int value)
    {
        if (entryCount < UserInfo.userInfo.MaxPartyNumber)
        {
            entryCountText.color = Color.white;
            maxEntryNumberText.color = Color.white;
        }
        else
        {
            entryCountText.color = Color.red;
            maxEntryNumberText.color = Color.red;
        }
    }

    public void SetMaxPartyNumberText(int value)
    {
        maxEntryNumberText.text = stringBuilder.Append(slashString).Append(value).ToString();
        stringBuilder.Clear();
    }

    public void SetButtonInteratable()
    {
        if(entryCount <= 0) startButton.interactable = false;
        else startButton.interactable = true;
    }
    #endregion

    #region Button Functions
    public void OnCombatStartButton()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        if (entryCount > 0)
        {
            if (!DataManager.Instance.userInfo.isUserTutorials && ToturialsManager.Instance.phase == 13)
            {
                ToturialsManager.Instance.isClear[5] = true;
            }
            GameManager.Instance.combatController.SetEntry(mainEntry.slots);
            GameManager.Instance.CombatStart();
        }
        else
        {
            warningMessgeUI.SetActive(true);
        }
    }

    public void OnEntryEmptyButton()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        EmptyEntry();
    }
    #endregion
}
