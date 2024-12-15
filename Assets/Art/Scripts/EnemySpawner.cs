using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; // Necessário para manipular UI
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Image spawningIndicator; // Referência à imagem de indicador de spawning

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap= 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Difficulty Settings")]
    [SerializeField] public float speedScalingFactor = 1.0f; // Aumento percentual na velocidad e a cad

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesLeftToSpawn;
    public int enemiesAlive;
    private float eps;
    private bool isSpawning = false;
    public static EnemySpawner main;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
         main = this;
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

        // Garante que a imagem está oculta inicialmente
        if (spawningIndicator != null)
        {
            spawningIndicator.enabled = false;
        }

        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        // Torna a imagem visível enquanto os inimigos estão a spawnar
        if (spawningIndicator != null)
        {
            spawningIndicator.enabled = enemiesLeftToSpawn > 0;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0 && isSpawning)
        {
            EndWave();
        }
    }

    public void EnemyDestroyed()
    {
        enemiesAlive--;
    }

  private IEnumerator StartWave()
{
    yield return new WaitForSeconds(timeBetweenWaves);
    isSpawning = true;
    enemiesLeftToSpawn = EnemiesPerWave();
    eps = EnemiesPerSecond();

    // Aumentar a velocidade dos inimigos conforme a onda aumenta
    AdjustEnemySpeed();
    LevelManager.main.ResetWaveCurrency();

    // Mostra o indicador de spawning
    if (spawningIndicator != null)
    {
        spawningIndicator.enabled = true;
    }
    
}


    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        LevelManager.main.ResetWaveCurrency();

        // Esconde o indicador de spawning
        if (spawningIndicator != null)
        {
            spawningIndicator.enabled = false;
        }

        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);

        GameObject prefabToSpawn = enemyPrefabs[index];
        GameObject enemy = Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1f);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
 
    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, 
        difficultyScalingFactor),0, enemiesPerSecondCap);
    }
   private void AdjustEnemySpeed()
{
    // Aumenta a velocidade dos inimigos a partir da 3ª onda
    float additionalSpeedFactor = 1.0f;

    if (currentWave >= 3)
    {
        // Aumenta a velocidade com base na onda
        additionalSpeedFactor = 1.0f + (currentWave - 2) * 0.1f;  // Aumenta a cada onda após a 2ª
    }

    // Encontrar todos os inimigos já instanciados
    foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
    {
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            float newSpeed = enemyMovement.GetBaseSpeed() * Mathf.Pow(speedScalingFactor, currentWave - 1) * additionalSpeedFactor;
            enemyMovement.UpdateSpeed(newSpeed);
        }
    }

    foreach (var tank in GameObject.FindGameObjectsWithTag("Tank"))
    {
        EnemyMovement enemyMovement = tank.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            float newSpeed = enemyMovement.GetBaseSpeed() * Mathf.Pow(speedScalingFactor, currentWave - 1) * additionalSpeedFactor;
            enemyMovement.UpdateSpeed(newSpeed);
        }
    }
}

}