using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_CombatWinButton : MonoBehaviour
{
    public void DebugButton_BattleWin()
    {
        GameManager.Instance.combatController.Debug_BattleWin();
    }
}
