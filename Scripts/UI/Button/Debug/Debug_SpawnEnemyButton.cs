using UnityEngine;

public class Debug_SpawnEnemyButton : MonoBehaviour
{
    public void OnSpawnEnemyButton()
    {
        GameManager.Instance.combatController.Debug_SpawnEnemy();
    }
}
