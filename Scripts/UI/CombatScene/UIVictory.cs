using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIVictory : UIBase
{
    [SerializeField] private TMP_Text clearGoldText;
    [SerializeField] private TMP_Text clearMedalText;

    // TODO : Å¬¸®¾î ½Ã ¾ÆÀÌÅÛ È¹µæ ¸ñ·Ï
    [SerializeField] private Transform itemPanel;
    [SerializeField] private GameObject itemSlotPrefab;

    private void OnEnable()
    {
        opened?.Invoke(null);
    }

    public override void OnOpened(object[] param)
    {
        SetResultUI();
    }


    public void SetResultUI()
    {
        int clearGold = GameManager.Instance.combatController.ClearGold;
        clearGoldText.text = string.Format("{0:#,##0}", clearGold);

        int clearMedal = GameManager.Instance.combatController.CurrentDungeon.ClearMedal;
        clearMedalText.text = clearMedal.ToString();

        // TODO : Å¬¸®¾î½Ã È¹µæ ¾ÆÀÌÅÛ ½½·Ô »ý¼º
        //foreach(Item item in ItemList)
        //{
        //    GameObject obj = Instantiate(itemSlotPrefab, itemPanel);
        //    ItemSlot itemSlot = obj.GetComponent<ItemSlot>;
        //    itemSlot.item = item;
        //}
    }
}
