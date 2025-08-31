using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnenmiesController : MonoBehaviour
{
    [Header("Konfiguracja przeciwników")]
    [SerializeField] private List<Sprite> AllEnemies;
    [SerializeField] private List<SpawnPoint> SpawnPoints;
    [SerializeField] private GameObject EnemyPrefab;


    private int _maxEnemies = 3;
    private int _currentEnemies = 0;

    private void Awake()
    {
        ConfigureEnemiesController();
    }

    private void Start()
    {
       

        SpawnEnemies();
    }

    private void OnEnable()
    {
        GameEvents.EnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.EnemyKilled -= EnemyKilled;

      
    }

    private void EnemyKilled(IEnemy enemy)
    {
        int spawnIndex = FreeSpawnPoint(enemy.GetEnemyPosition());


       

        DestroyKilledEnemy(enemy.GetEnemyObject());
        StartCoroutine(SpawnEnemyViaCor());
    }

    private void SpawnEnemies()
    {
        while (_currentEnemies < _maxEnemies)
        {
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnEnemyViaCor()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (_currentEnemies >= _maxEnemies)
        {
            Debug.LogWarning("Max Enemies reached! Kill some to spawn new");
            return;
        }

        int freeSpawnPointIndex = -1;
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (!SpawnPoints[i].IsOccupied)
            {
                freeSpawnPointIndex = i;
                break;
            }
        }

        if (freeSpawnPointIndex == -1) return;

        SpawnPoints[freeSpawnPointIndex].IsOccupied = true;

        SoulEnemy enemy = Instantiate(
            EnemyPrefab,
            SpawnPoints[freeSpawnPointIndex].Position.position,
            Quaternion.identity,
            transform
        ).GetComponent<SoulEnemy>();

        if (enemy == null)
        {
            Debug.LogError("EnemyPrefab does not have SoulEnemy component!");
            return;
        }

        int spriteIndex = Random.Range(0, AllEnemies.Count);
        enemy.SetupEnemy(AllEnemies[spriteIndex], SpawnPoints[freeSpawnPointIndex]);
        _currentEnemies++;

     
    }

    private void DestroyKilledEnemy(GameObject enemy)
    {
        Destroy(enemy);
        GameEvents.RescanYourGrid?.Invoke();
    }

    private int FreeSpawnPoint(SpawnPoint spawnPoint)
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (spawnPoint != SpawnPoints[i]) continue;

            SpawnPoints[i].IsOccupied = false;
            _currentEnemies--;
            return i;
        }
        return -1;
    }

    private void ConfigureEnemiesController()
    {
        _maxEnemies = SpawnPoints != null ? SpawnPoints.Count : 3;
    }
}

[System.Serializable]
public class SpawnPoint
{
    public Transform Position;
    public bool IsOccupied;
}
