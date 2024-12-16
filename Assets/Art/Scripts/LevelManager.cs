using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum DifficultyLevel { Easy, Medium, Hard } // Definição da enumeração

    [Header("Game Settings")]
    private DifficultyLevel difficulty; // Escolher pelo Inspector
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

        // Carrega a dificuldade salva no PlayerPrefs ao iniciar
        LoadDifficultyFromPrefs();
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
                currency = 100;
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

    // Função para obter o multiplicador de velocidade dos inimigos
    public float GetEnemySpeedMultiplier()
    {
        return speedScalingFactor;
    }

    // Funções para alterar a dificuldade e salvar no PlayerPrefs
    public void SetDifficultyEasy()
    {
       
        difficulty = DifficultyLevel.Easy;
        SaveDifficultyToPrefs(); // Salva no PlayerPrefs
        ApplyDifficultySettings();
    }

    public void SetDifficultyMedium()
    {

        difficulty = DifficultyLevel.Medium;
        SaveDifficultyToPrefs(); // Salva no PlayerPrefs
        ApplyDifficultySettings();
    }

    public void SetDifficultyHard()
    {
        difficulty = DifficultyLevel.Hard;
        SaveDifficultyToPrefs(); // Salva no PlayerPrefs
        ApplyDifficultySettings();
    }

    // Função para salvar a dificuldade no PlayerPrefs
    private void SaveDifficultyToPrefs()
    {
        PlayerPrefs.SetInt("Difficulty", (int)difficulty); // Salva a dificuldade como inteiro
        PlayerPrefs.Save(); // Garante que os dados sejam gravados no disco
    }

    // Função para carregar a dificuldade do PlayerPrefs
    private void LoadDifficultyFromPrefs()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            int savedDifficulty = PlayerPrefs.GetInt("Difficulty");
            difficulty = (DifficultyLevel)savedDifficulty; // Converte o inteiro para o enum
        }
        else
        {
            difficulty = DifficultyLevel.Easy; // Define o padrão como Easy se não houver valor salvo
        }

        Debug.Log($"Loaded Difficulty: {difficulty}");
    }
}
