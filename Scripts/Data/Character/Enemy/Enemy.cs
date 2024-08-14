using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy : MonoBehaviour
{
    public EnemyInfo Info { get; set; }

    [SerializeField] private EnemyCombatAI combatAI = null;

    private void Start()
    {
        combatAI = GetComponent<EnemyCombatAI>();
        combatAI.team = Team.Enemy;
        StartCoroutine(SetCharacterInfo());
    }

    IEnumerator SetCharacterInfo()
    {
        yield return new WaitUntil(() => Info != null);
        combatAI.LoadEnemyInfo(Info);
        combatAI.SetBaseInfo();
        combatAI.InitializedTarget();
    }
}
