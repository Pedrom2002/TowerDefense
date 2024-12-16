using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI; // Necessário para manipular UI
using System.Collections;
using TMPro;


public class EnemySpawner : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private int playerLife = 100; // Vida inicial do jogador
    [SerializeField] private int damagePerEnemy = 10; // Dano causado por cada inimigo que chega ao final

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Image spawningIndicator;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;
    [SerializeField] private int maxLife = 100;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Difficulty Settings")]
    [SerializeField] public float speedScalingFactor = 1.0f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI healthText;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    public int enemiesLeftToSpawn;
    public int enemiesAlive;
    private float eps;
    private bool isSpawning = false;
    public static EnemySpawner main;
    private int currentLife;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        main = this;
    }

    private void Start()
    {
        currentLife = maxLife;
        UpdateLifeUI();
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

        if (spawningIndicator != null)
        {
            spawningIndicator.enabled = false;
        }

        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

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

    public void EnemyReachedEnd()
    {
        playerLife -= damagePerEnemy; // Reduz a vida do jogador
        Debug.Log($"Player Life: {playerLife}");
        UpdateLifeUI();

        if (playerLife <= 0)
        {
            GameOver(); // Chama o método GameOver quando a vida do jogador atinge 0
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over. Returning to Main Menu...");
        ResetGame(); // Reseta os valores relevantes
        GameOverPopup.TriggerGameOverMessage();
        SceneManager.LoadScene("MainMenu"); // Troca para a cena do menu principal
    }

    private void ResetGame()
    {
        currentWave = 1;
        enemiesAlive = 0;
        playerLife = 100; // Restaura a vida inicial
        timeSinceLastSpawn = 0f;
    }
private IEnumerator StartWave()
{
    yield return new WaitForSeconds(timeBetweenWaves);
    isSpawning = true;
    enemiesLeftToSpawn = EnemiesPerWave();
    eps = EnemiesPerSecond();
    AdjustEnemySpeed();

    // Obtendo uma referência ao LevelManager na cena
    LevelManager levelManager = Object.FindObjectOfType<LevelManager>();
    if (levelManager != null)
    {
        levelManager.ResetWaveCurrency(); // Chama o método da instância
    }
    else
    {
        Debug.LogError("LevelManager não encontrado na cena.");
    }

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

        if (spawningIndicator != null)
        {
            spawningIndicator.enabled = false;
        }

        StartCoroutine(StartWave());
    }

    
    private void SpawnEnemy()
    {
        if(enemyPrefabs.Length > 0){
        int index = Random.Range(0, enemyPrefabs.Length);
        
        GameObject prefabToSpawn = enemyPrefabs[index];
        GameObject enemy = Instantiate(prefabToSpawn, startPoint.position, Quaternion.identity);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 1f);
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0, enemiesPerSecondCap);
    }

    private void AdjustEnemySpeed()
    {
        float additionalSpeedFactor = 1.0f;

        if (currentWave >= 3)
        {
            additionalSpeedFactor = 1.0f + (currentWave - 2) * 0.1f;
        }

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                float newSpeed = enemyMovement.GetBaseSpeed() * Mathf.Pow(speedScalingFactor, currentWave - 1) * additionalSpeedFactor;
                enemyMovement.UpdateSpeed(newSpeed);
            }
        }
    }
   private void UpdateLifeUI()
    {
        if ( healthText!= null)
        {
            healthText.text = $"Life: {playerLife}";
        }
    }
}
