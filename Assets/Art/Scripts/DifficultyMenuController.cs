using UnityEngine;

public class DifficultyMenuController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private GameObject mainMenuCanvas;  // Canvas do menu principal
    [SerializeField] private GameObject difficultyMenuCanvas;  // Canvas do menu de dificuldade

    public void OpenDifficultyMenu()
    {
        // Abrir o menu de dificuldade e fechar o menu principal
        if (difficultyMenuCanvas != null)
        {
            difficultyMenuCanvas.SetActive(true);
        }

        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        // Voltar ao menu principal e fechar o menu de dificuldade
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
        }

        if (difficultyMenuCanvas != null)
        {
            difficultyMenuCanvas.SetActive(false);
        }
    }

    public void SetDifficultyEasy()
    {
        LevelManager.main.SetDifficultyEasy();
       // Aplica as configurações
        BackToMainMenu(); // Volta ao menu principal
    }

    public void SetDifficultyMedium()
    {
    LevelManager.main.SetDifficultyMedium();

    // Volta ao menu principal
    BackToMainMenu();
    }

    public void SetDifficultyHard()
    {
         // Obtenha uma instância do LevelManager
    LevelManager.main.SetDifficultyHard();
    // Volta ao menu principal
    BackToMainMenu();
    }
}
