using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Character : MonoBehaviour
{
    public CharacterData data;

    [SerializeField, ShowInInspector] public PlayerCombatAI CombatAI { get; private set; } = null;

    private void Start()
    {
        CombatAI = GetComponent<PlayerCombatAI>();
        CombatAI.team = Team.Ally;
        StartCoroutine(SetCharacterInfo());
    }

    IEnumerator SetCharacterInfo()
    {
        yield return new WaitUntil(() => data != null);
        CombatAI.SetBaseInfo();
        CombatAI.InitializedTarget();
    }
}
