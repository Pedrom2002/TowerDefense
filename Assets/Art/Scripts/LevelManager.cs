using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum DifficultyLevel { Easy, Medium, Hard } // Definição da enumeração

    [Header("Game Settings")]
    public DifficultyLevel difficulty = DifficultyLevel.Medium; // Escolher pelo Inspector
    public static LevelManager main;
    public Transform startPoint;
    public Transform[] path;

    public int currency;
    private int maxCurrency = 300; 
    private int currentWaveCurrency = 0; // Moeda acumulada durante a onda atual
    private int maxWaveCurrency = 300; // Limite máximo de moeda por onda
    private float speedScalingFactor; // Escala da velocidade dos inimigos

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        ApplyDifficultySettings(); // Define as configurações de dificuldade
    }

    public void IncreaseCurrency(int amount)
    {
        // Só aumenta a moeda se não exceder o limite da onda
        if (currentWaveCurrency + amount > maxWaveCurrency)
        {
            amount = maxWaveCurrency - currentWaveCurrency; // Ajusta o valor para o que falta até o limite
        }

        currentWaveCurrency += amount; // Atualiza os ganhos da onda
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Don't have money to buy it");
            return false;
        }
    }

    public void ResetWaveCurrency()
    {
        currentWaveCurrency = 0;
    }

    public void ApplyDifficultySettings()
{
    switch (difficulty)
    {
        case DifficultyLevel.Easy:
            maxCurrency = 200;
            maxWaveCurrency = 200;
            currency = 150;
            speedScalingFactor = 1.1f;  // Definindo o valor para Easy
            break;

        case DifficultyLevel.Medium:
            maxCurrency = 200;
            maxWaveCurrency = 150;
            currency = 150;
            speedScalingFactor = 1.2f;  // Definindo o valor para Medium
            break;

        case DifficultyLevel.Hard:
            maxCurrency = 200;
            maxWaveCurrency = 150;
            currency = 100;
            speedScalingFactor = 1.3f;  // Definindo o valor para Hard
            break;
    }

    // Verifique se o EnemySpawner.main não é nulo antes de tentar acessá-lo
    if (EnemySpawner.main != null)
    {
        EnemySpawner.main.speedScalingFactor = speedScalingFactor;
    }
    else
    {
        Debug.LogWarning("EnemySpawner.main não está inicializado.");
    }

    Debug.Log($"Difficulty set to {difficulty}. MaxCurrency: {maxCurrency}, MaxWaveCurrency: {maxWaveCurrency}, SpeedScalingFactor: {speedScalingFactor}");
}


    // Função para obter o multipicador de velocidade dos inimigos
    public float GetEnemySpeedMultiplier()
    {
        return speedScalingFactor;
    }

    
}