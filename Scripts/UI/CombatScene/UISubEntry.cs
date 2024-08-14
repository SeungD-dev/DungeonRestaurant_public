using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


// TODO :
// SubEntry(가제) : 플레이어가 보유하고 있는 캐릭터 리스트
public class UISubEntry : UIEntryBase
{
    public List<SlotSubEntry> slots;

    [SerializeField] private SlotSubEntry _selectedSlot;
    public SlotSubEntry selectedSlot
    {
        get => _selectedSlot;
        set
        {
            _selectedSlot = value;
            UIStrategy.Instance.Check();
        }
    }


    protected override void Awake()
    {
        base.Awake();

        int slotIndex = 0;
        foreach (CharacterData data in DataManager.Instance.characterList)
        {
            GameObject obj;
            obj = Instantiate(slotPrefab, background);
            SlotSubEntry slot = obj.GetComponent<SlotSubEntry>();
            slot.CharacterData = data;
            slot.Index = slotIndex;
            slotIndex++;
            slot.toggle.onValueChanged.AddListener(OnToggleValueChanged);
            slot.toggle.group = toggleGroup;
            slots.Add(slot);
        }
    }

    public override void OnToggleValueChanged(bool isOn)
    {
        base.OnToggleValueChanged(isOn);
        selectedSlot = currentActiveToggle?.gameObject.GetComponent<SlotSubEntry>();
    }

}
