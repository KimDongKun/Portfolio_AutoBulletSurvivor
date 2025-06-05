using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    /// <summary>
    /// 시간 마다 Enemy 생산주기 및 새 적대 유닛 생성
    /// 아래 예시
    /// 100초 : Runner
    /// 200초 : Heavy
    /// </summary>
    public GameStageManager GameStageManager;
    public Transform player; 

    public GameObject nomal_spawnEnemy_Prefab;
    public GameObject runner_spawnEnemy_Prefab;
    public GameObject heavy_spawnEnemy_Prefab;
    public GameObject shooter_spawnEnemy_Prefab;
    public GameObject bug_spawnEnemy_Prefab;

    public float nomal_respawnTime = 1f;
    private float nomal_timer;

    public float runner_respawnTime = 5f;
    private float runner_timer = 1f;

    public float heavy_respawnTime = 10f;
    private float heavy_timer = 1f;


    public float scale;
    public int stageTimeLevel;
    private int _changeLevel;

    private Vector3 spawnPos;
    private void Start()
    {
        nomal_timer = nomal_respawnTime;
        runner_timer = runner_respawnTime;
        heavy_timer = heavy_respawnTime;
        _changeLevel = -1;
        //EnemySpawn_Level(0);

        //EnemySpawn_Level(stageTimeLevel);
        //nomal_spawnEnemy_Prefab
        StartCoroutine(SpawnEnemySetting(6f, 4, nomal_spawnEnemy_Prefab, 200f, 4f, 50f));
        StartCoroutine(SpawnEnemySetting(5f, 5, runner_spawnEnemy_Prefab, 100f, 5f, 20f,5));
        StartCoroutine(SpawnEnemySetting(7f, 6, heavy_spawnEnemy_Prefab, 1000f, 4f, 50f, 15));
        StartCoroutine(SpawnEnemySetting(6f, 7, shooter_spawnEnemy_Prefab, 500f, 6f, 20f, 20));
        StartCoroutine(SpawnEnemySetting(7f, 8, bug_spawnEnemy_Prefab, 1500f, 5f, 50f, 30));
    }

    // Update is called once per frame
    void Update()
    {
      
        stageTimeLevel = ((int)GameStageManager.timer / 60 % 60);
        if (_changeLevel != stageTimeLevel)
        {
            _changeLevel = stageTimeLevel;
            //EnemySpawn_Level(stageTimeLevel);
        }
        
    }
    bool EnemySpawnValue()
    {
        GameObject[] _enemyValue = GameObject.FindGameObjectsWithTag("EnemyUnit");
        //Debug.LogError("적의 숫자 :" + _enemyValue.Length);
        if (_enemyValue.Length < 200)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    void EnemySpawn_Level(int _level)
    {
        //체력 200/100/1000/300/500
        //이속 5  / 8 /  5 / 7 / 7
        //Debug.Log("스폰 레벨: "+_level);
        if (EnemySpawnValue())
        {
            float stageLV = (float)_level;
            

            //float balanceValue = (Mathf.Pow(stageLV, 2) * 0.02f) + 1.0f;
            float balanceValue = (stageLV*(0.15f)) + 1.0f;

            Debug.Log("스테이지 레벨: " + stageLV + " / " + balanceValue);

            StartCoroutine(SpawnEnemySetting(12f, (_level < 2 ? 6 : 2), nomal_spawnEnemy_Prefab, 200f * balanceValue, 4f * balanceValue, 50f * balanceValue));
            /*StartCoroutine(SpawnEnemySetting(12f, (_level < 4 ? 4 : 6), runner_spawnEnemy_Prefab, 100f * balanceValue, 5f * balanceValue, 20f * balanceValue, _level > 2));
            StartCoroutine(SpawnEnemySetting(15f, (_level < 9 ? 6 : 8), heavy_spawnEnemy_Prefab, 1000f * balanceValue, 4f * balanceValue, 50f * balanceValue, _level > 4));
            StartCoroutine(SpawnEnemySetting(15f, (_level < 12 ? 6 : 8), shooter_spawnEnemy_Prefab, 300f * balanceValue, 5f * balanceValue, 10f * balanceValue, _level > 9));
            StartCoroutine(SpawnEnemySetting(15f, (_level < 15 ? 6 : 10), bug_spawnEnemy_Prefab, 400f * balanceValue, 5f * balanceValue, 50f * balanceValue, _level > 12));*/

           
        }

    }
    public Vector3 RandomSpawnPosition()
    {
        float radius = 35f;
        Vector3 playerPosition = player.position;

        float a = playerPosition.x;
        float b = playerPosition.y;

        float x = Random.Range(-radius + a, radius + a);
        float y_b = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x - a, 2));
        y_b *= Random.Range(0, 2) == 0 ? -1 : 1;
        float y = y_b + b;

        Vector3 randomPosition = new Vector3(x, y, 0);
        //Debug.LogError("결과 좌표: "+ randomPosition);
        return randomPosition;
    }

    IEnumerator SpawnEnemySetting(float spawnTimer, int spawnValue, GameObject enemyPrefab, float enemyHP, float enemySpeed, float enemyAtk,int stageRound = 0)
    {
        Debug.Log("생성 시작 : "+ enemyPrefab.name);
        bool isSpawnEnemy = true;

        while (isSpawnEnemy)
        {
            bool isSpawn = GameStageManager.round >= stageRound;
            int stageSpawnValue = spawnValue + (int)(GameStageManager.round / 3);
            if (isSpawn && EnemySpawnValue())
            {
                for (int i = 0; i < stageSpawnValue; i++)
                {
                    //float stageLV = (float)stageTimeLevel;
                    float stageLV = GameStageManager.round;
                    scale =
                        GameStageManager.round > 15 ? GameStageManager.round > 30 ? 0.15f : 0.1f : 0.05f;
                    float balanceValue = (stageLV * scale) + 1.0f;

                    float setHP = enemyHP * balanceValue;
                    float setSpeed = enemySpeed * balanceValue;
                    float setAtk = enemyAtk * balanceValue;

                    GameObject enemy = Instantiate(enemyPrefab, RandomSpawnPosition(), Quaternion.identity);

                    if (enemy.TryGetComponent(out EnemyCtrl enemyData))//EnemyBugCtrl
                    {
                        enemyData.HP = setHP;
                        enemyData.speed = setSpeed;
                        enemyData.AttackDamage = setAtk;
                    }
                    else if (enemy.TryGetComponent(out EnemyBugCtrl enemybugData))
                    {
                        enemybugData.HP = setHP;
                        enemybugData.speed = setSpeed;
                        enemybugData.AttackDamage = setAtk;
                    }
                }
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }

}
