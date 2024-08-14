using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class UIDefeat : UIBase
{
    [SerializeField] private TMP_Text clearGoldText;


    private void OnEnable()
    {
        opened?.Invoke(null);
    }

    public override void OnOpened(object[] param) 
    {
        SetResultUI();
    }

    public virtual void SetResultUI()
    {
        int clearGold = GameManager.Instance.combatController.ClearGold;
        clearGoldText.text = string.Format("{0:#,##0}", clearGold);
    }
}
