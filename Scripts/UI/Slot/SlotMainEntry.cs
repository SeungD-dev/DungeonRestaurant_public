using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotMainEntry : EntrySlotBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject unRegisterButton;
    [SerializeField] private GameObject emptyImage;
    private DragSlotMainEntry dragSlot;

    [ShowInInspector] public int SubEntryIndex { get; set; }


    protected override void Awake()
    {
        base.Awake();
        dragSlot = UIStrategy.Instance.dragSlotMain;
        OnDataChange += SetUIOnRegister;
    }

    protected void SetUIOnRegister()
    {
        if (CharacterData != null)
        {
            unRegisterButton.SetActive(true);
            emptyImage.SetActive(false);
        }
        else
        {
            unRegisterButton.SetActive(false);
            emptyImage.SetActive(true);
        }
    }

    public void OnUnRegisterButton()
    {
        SoundManager.Instance.PlaySound("SFX_UI_Click");
        UIStrategy.Instance.UnRegisterEntry(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UIStrategy.Instance.mainEntry.selectedSlot != null)
            UIStrategy.Instance.mainEntry.selectedSlot.toggle.isOn = false;
        dragSlot.SetActive(true);
        dragSlot.Slot = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragSlot.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragSlot.Slot = null;
        dragSlot.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        SlotMainEntry droppedSlot;
        if (eventData.pointerDrag.TryGetComponent<SlotMainEntry>(out droppedSlot))
        {
            UIStrategy.Instance.SwapEntryPosition(this, droppedSlot);
            return;
        }

        SlotSubEntry droppedSubSlot;
        if (eventData.pointerDrag.TryGetComponent<SlotSubEntry>(out droppedSubSlot))
        {
            if (droppedSubSlot.IsRegistered) return;
            UIStrategy.Instance.Check(this, droppedSubSlot);
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toggle.isOn == false)
            backgroundImage.color = Color.white;
    }
}
