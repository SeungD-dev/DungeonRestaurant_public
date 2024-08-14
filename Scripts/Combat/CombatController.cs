using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : SerializedMonoBehaviour
{
    #region Fields
    public static CombatController instance;

    [Title("Player")]
    [SerializeField] private CharacterData[] entry;
    [SerializeField] public Character[] playerCharacters;
    [SerializeField] public List<PlayerCombatAI> playerCombatAI;
    private int[] playerSpawnIndex;
    private int entryCount = 0;
    private int playerDataLoadCount = 0;
    private int PlayerDataLoadCount
    {
        get { return playerDataLoadCount; }
        set
        {
            playerDataLoadCount = value;
            CheckLoading();
        }
    }
    [SerializeField] private int playerAliveCount = 0;
    public int PlayerAliveCount
    {
        get { return playerAliveCount; }
        private set
        {
            playerAliveCount = value;
            CheckCombatOver();
        }
    }

    [Title("Enemy")]
    [SerializeField] public List<Enemy> enemyCharacters;
    [SerializeField] public List<EnemyCombatAI> enemyCombatAI;
    [SerializeField] private int enemyAliveCount = 0;
    public int EnemyAliveCount
    {
        get { return enemyAliveCount; }
        set
        {
            enemyAliveCount = value;
            CheckCombatOver();
        }
    }

    [Title("Dungeon")]
    [ShowInInspector, ReadOnly] public DungeonInfo CurrentDungeon { get; private set; }
    private int currentWave = 0;

    private int maxWave;
    private float time;
    private float waveTime;
    public int ClearGold { get; set; }
    [SerializeField] private int killCount = 0;
    [SerializeField] private int killGold = 100;

    [Title("SpawnPosition")]
    [SerializeField] private Transform[] playerSpawnPosition = new Transform[GameManager.MAX_POSITION];
    [SerializeField] private Dictionary<SpawnArea, EnemySpawnArea> enemySpawnArea = new Dictionary<SpawnArea, EnemySpawnArea>();

    [Title("UI")]
    public TMP_Text timeText;
    public TMP_Text waveText;
    private string waveString;
    public Slider progressBar;
    public TMP_Text dungeonNameText;
    public TMP_Text stageText;
    private string stageString = "Stage ";

    [Title("Camera Settings")]
    [SerializeField] private float cameraMoveDistance = 10f;

    public bool isPlayerWin { get; private set; }
    #endregion


    #region Unity Lifecycle Functions

    private void Awake()
    {
        instance = this;
        GameManager.Instance.combatController = this;
        entry = new CharacterData[GameManager.MAX_PARTY_NUMBER];
        playerCharacters = new Character[GameManager.MAX_PARTY_NUMBER];
        playerSpawnIndex = new int[GameManager.MAX_PARTY_NUMBER];

        SetDungeonData();

        StartCoroutine(SetCombatUI());
    }

    private void Update()
    {
        CheckTime();
    }
    private void OnDestroy()
    {
        GameManager.Instance.combatController = null;
    }

    #endregion


    #region Set Data & UI Functions

    IEnumerator SetCombatUI()
    {
        yield return new WaitUntil(() => waveText != null);

        StringBuilder sb = new StringBuilder();
        sb.Append(" / ").Append(maxWave);
        waveString = sb.ToString();
        progressBar.maxValue = maxWave;
        SetWaveProgressUI();

        sb.Clear();
        sb.Append(stageString).Append(CurrentDungeon.Stage);
        stageText.text = sb.ToString();
        dungeonNameText.text = CurrentDungeon.DungeonName;
    }

    private void SetDungeonData()
    {
        CurrentDungeon = DataManager.Instance.dungeonList.CurrentDungeon;
        waveTime = CurrentDungeon.WaveTime;
        time = waveTime;
        maxWave = CurrentDungeon.WaveList.Count;
        currentWave = 0;
    }

    public void SetEntry(List<SlotMainEntry> mainEntry)
    {
        int idx = 0;

        for (int i = 0; i < GameManager.MAX_POSITION; i++)
        {
            if (mainEntry[i].IsSlotEmpty() == false)
            {
                entry[idx] = mainEntry[i].CharacterData;
                playerSpawnIndex[idx] = i;
                idx++;
                entryCount++;
            }
        }
    }

    void SetWaveProgressUI()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(currentWave + 1);
        sb.Append(waveString);
        waveText.text = sb.ToString();
        progressBar.value = currentWave;
    }

    #endregion


    #region Data Await Functions
    void CheckLoading()
    {
        if (PlayerDataLoadCount >= entryCount)
        {
            GameManager.Instance.SetCombatFlag(true);
        }
        return;
    }
    public void PlayerDataLoadComplete()
    {
        PlayerDataLoadCount++;
    }

    #endregion


    #region Combat Logic Functions

    public void CheckTime()
    {
        if (GameManager.Instance.isCombatStart)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                NextWave();
            }
            timeText.text = time.ToString("00");
        }
    }

    public void SpawnPlayerCharacter()
    {
        for (int i = 0; i < entry.Length; i++)
        {
            if (entry[i] != null)
            {
                GameObject obj;
                Character ch;
                int spawnIndex = playerSpawnIndex[i];
                obj = Instantiate(entry[i].skin.prefab, playerSpawnPosition[spawnIndex].position, Quaternion.identity);
                ch = obj.GetComponent<Character>();
                ch.data = entry[i];
                playerCharacters[i] = ch;
                playerAliveCount++;
            }
        }
    }

    public void SpawnEnemyCharacter(int wave)
    {
        WaveData waveData = CurrentDungeon.GetWaveData(wave);

        foreach(SpawnArea eSpawnArea in Enum.GetValues(typeof(SpawnArea)))
        {
            List<EnemyInfo> enemyList = waveData.GetEnemieListBySpawnArea(eSpawnArea);

            if (enemyList != null)
            {
                EnemySpawnArea spawnArea = enemySpawnArea[eSpawnArea];
                spawnArea.ClearOccupiedPositions();

                foreach (EnemyInfo enemyInfo in enemyList)
                {
                    Enemy enemy;
                    GameObject obj;

                    Vector2 spawnPosition = spawnArea.GetRandomPosition();

                    obj = Instantiate(enemyInfo.Prefab.prefab, spawnPosition, Quaternion.identity);
                    enemy = obj.GetComponent<Enemy>();
                    enemy.Info = enemyInfo;
                    enemyCharacters.Add(enemy);
                    enemyAliveCount++;
                }
            }
        }
    }



    void NextWave()
    {
        if (currentWave < (maxWave - 1))
        {
            time = waveTime;
            currentWave++;
            SpawnEnemyCharacter(currentWave);
            SetWaveProgressUI();
        }
        else
        {
            Defeat();
        }
    }
    public void CharacterDie(Team team)
    {
        if (team == Team.Ally)
        {
            PlayerAliveCount--;
        }
        else if (team == Team.Enemy)
        {
            EnemyAliveCount--;
            killCount++;
        }
    }

    public void CharacterResurrect(Team team)
    {
        if (team == Team.Ally)
        {
            PlayerAliveCount++;
        }
        else if (team == Team.Enemy)
        {
            EnemyAliveCount++;
        }
    }

    public void CheckCombatOver()
    {
        if (GameManager.Instance.isCombatStart)
        {
            if (playerAliveCount == 0)
            {
                Defeat();
            }
            else if (enemyAliveCount <= 0)
            {
                if (currentWave < (maxWave - 1))
                    NextWave();
                else
                    Victory();

            }
            else return;
        }
    }

    public void Defeat()
    {
        isPlayerWin = false;
        StartCoroutine(CombatOver());
    }

    public void Victory()
    {
        isPlayerWin = true;
        progressBar.value = maxWave;
        EmploymentManager.Instance.ReRoll();
        StartCoroutine(CombatOver());
    }

    private IEnumerator CombatOver()
    {
        yield return new WaitForSeconds(1f);
        if (isPlayerWin)
        {
            SoundManager.Instance.PlaySound("BGM_Victory");
        }
        else
        {
            SoundManager.Instance.PlaySound("BGM_Defeat");
        }
        GetClearReward();
        GameManager.Instance.CombatOver();
        CameraController.Instance.ResetCamera();
    }

    public void GetClearReward()
    {
        ClearGold = CurrentDungeon.ClearGold;
        if (isPlayerWin)
        {
            UserInfo.userInfo.Medal += CurrentDungeon.ClearMedal;
        }
        else
        {
            ClearGold = (int)(ClearGold * CurrentDungeon.DefeatMultiplier);
        }
        ClearGold += killCount * killGold;
        UserInfo.userInfo.Gold += ClearGold;
    }
    #endregion


    #region Debug

    public void Debug_BattleWin()
    {
        Victory();
    }

    public void Debug_NextWave()
    {
        time = 0;
    }

    public void Debug_KillAllEnemies()
    {
        foreach (Enemy enemy in enemyCharacters)
        {
            if (enemy.gameObject.activeSelf == true)
            {
                BaseCombatAI ai = enemy.gameObject.GetComponent<BaseCombatAI>();
                ai.Die();
            }
        }
    }

    public void Debug_SpawnEnemy()
    {
        SpawnEnemyCharacter(0);
    }

    public void Debug_ResurrectPlayerCharacters()
    {
        foreach (Character character in playerCharacters)
        {
            if (character == null) return;
            if (character.CombatAI.CheckAlive() == false)
                character.CombatAI.Resurrection(100);
        }
    }

    #endregion
}