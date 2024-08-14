using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_KillAllEnemies : MonoBehaviour
{
    public void OnAllKillEnemiesButton()
    {
        GameManager.Instance.combatController.Debug_KillAllEnemies();
    }
}
