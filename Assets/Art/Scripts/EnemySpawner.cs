using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform startPoint;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesLeftToSpawn;
    private int enemiesAlive;
    private bool isSpawning = false;
    private void Awake() {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        if (startPoint == null)
        {
            LevelManager levelManager = Object.FindFirstObjectByType<LevelManager>();
            if (levelManager != null)
            {
                startPoint = levelManager.startPoint;
            }
            else
            {
                Debug.LogError("LevelManager não encontrado na cena.");
            }
        }
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0 && isSpawning){
            EndWave();
        }
    }

    private void EnemyDestroyed(){
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave(){
    isSpawning = false;
    timeSinceLastSpawn=0f;
    currentWave++;
    StartCoroutine(StartWave());    
}
    private void SpawnEnemy()
    {
        Debug.Log("Spawned Enemy");


        GameObject prefabToSpawn = enemyPrefabs[0];
        GameObject enemy = Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1f);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}