using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HerbalistSkill : MonoBehaviour
{
    private CircleCollider2D skillArea;
    [SerializeField] private float healInterval = 0.5f;
    [SerializeField] private float healMultiplier = 0.5f;
    [SerializeField] private float attackSpeedIncreasePercent = 0.1f;
    private List<PlayerCombatAI> affectedPlayers = new List<PlayerCombatAI>();
    private Coroutine healRoutine;
    private Herbalist herbalist;

    private void Awake()
    {
        skillArea = GetComponent<CircleCollider2D>();
        if (skillArea == null)
        {
            skillArea = gameObject.AddComponent<CircleCollider2D>();
        }
        skillArea.isTrigger = true;
    }

    public void Initialize(Herbalist herbalist)
    {
        this.herbalist = herbalist;
        StartSkillEffect();
    }

    private void StartSkillEffect()
    {
        healRoutine = StartCoroutine(HealAndBuffRoutine());
    }

    private void OnDisable()
    {
        StopSkillEffect();
    }

    public void StopSkillEffect()
    {
        if (healRoutine != null)
        {
            StopCoroutine(healRoutine);
        }
        ResetBuffs();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCombatAI player = other.GetComponent<PlayerCombatAI>();
        if (player != null && !affectedPlayers.Contains(player))
        {
            affectedPlayers.Add(player);
            ApplyAttackSpeedBuff(player);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerCombatAI player = other.GetComponent<PlayerCombatAI>();
        if (player != null && affectedPlayers.Contains(player))
        {
            affectedPlayers.Remove(player);
            RemoveAttackSpeedBuff(player);
        }
    }

    private IEnumerator HealAndBuffRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);
            float healAmount = herbalist.AttackDamage * healMultiplier;
            foreach (var player in affectedPlayers)
            {
                player.Heal(healAmount);
            }
        }
    }

    private void ApplyAttackSpeedBuff(PlayerCombatAI player)
    {
        player.IncreaseAttackSpeed(attackSpeedIncreasePercent);
    }

    private void RemoveAttackSpeedBuff(PlayerCombatAI player)
    {
        player.DecreaseAttackSpeed(attackSpeedIncreasePercent);
    }

    private void ResetBuffs()
    {
        foreach (var player in affectedPlayers)
        {
            RemoveAttackSpeedBuff(player);
        }
        affectedPlayers.Clear();
    }
}