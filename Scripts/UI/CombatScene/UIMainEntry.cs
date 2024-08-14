using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// TODO :
// MainEntry(가제) : 전투 편성 엔트리
public class UIMainEntry : UIEntryBase
{
    [SerializeField] public List<SlotMainEntry> slots;
    [SerializeField] private SlotMainEntry _selectedSlot;

    public SlotMainEntry selectedSlot
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

        for (int i = 0; i < GameManager.MAX_POSITION; i++)
        {
            GameObject obj;
            obj = Instantiate(slotPrefab, background);
            SlotMainEntry slot = obj.GetComponent<SlotMainEntry>();
            slot.CharacterData = null;
            slot.Index = i;
            slot.toggle.onValueChanged.AddListener(OnToggleValueChanged);
            slot.toggle.group = toggleGroup;
            slots.Add(slot);
        }
    }


    public override void OnToggleValueChanged(bool isOn)
    {
        base.OnToggleValueChanged(isOn);
        selectedSlot = currentActiveToggle?.gameObject.GetComponent<SlotMainEntry>();
    }

}
