using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel; // Referência ao painel do pop-up
    [SerializeField] private Button okButton; // Referência ao botão "Ok"

    private static bool showGameOverMessage = false; // Variável para rastrear se o pop-up deve ser mostrado

    private void Start()
    {
        // Define o estado inicial do pop-up
        popupPanel.SetActive(showGameOverMessage);

        if (okButton != null)
        {
            okButton.onClick.AddListener(HidePopup);
        }
    }

    public static void TriggerGameOverMessage()
    {
        showGameOverMessage = true; // Ativa a exibição do pop-up na próxima cena
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false); // Esconde o pop-up
        showGameOverMessage = false; // Reseta o estado para evitar exibições futuras
    }
}
