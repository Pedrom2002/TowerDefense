using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject difficultyMenuCanvas;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button difficultyButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        // Inicia o menu principal visível
        ShowMainMenu();

        // Configura os listeners dos botões
        startButton.onClick.AddListener(OnStartButtonClicked);
        difficultyButton.onClick.AddListener(OnDifficultyButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);  // Exibe o menu principal
        difficultyMenuCanvas.SetActive(false);  // Esconde o menu de dificuldade
    }

    private void ShowDifficultyMenu()
    {
        mainMenuCanvas.SetActive(false);  // Esconde o menu principal
        difficultyMenuCanvas.SetActive(true);  // Exibe o menu de dificuldade
    }

    private void OnStartButtonClicked()
    {
        // Iniciar o jogo
        Debug.Log("Game Started!");
        SceneManager.LoadScene("SampleScene");
    }

    private void OnDifficultyButtonClicked()
    {
        // Mostrar o menu de dificuldade
        ShowDifficultyMenu();
    }

    private void OnExitButtonClicked()
    {
        // Fechar o jogo
        Debug.Log("Game Exited!");
        Application.Quit();
    }
}
