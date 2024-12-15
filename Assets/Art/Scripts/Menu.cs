using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour {
    [Header("References")]
   
   [SerializeField] private TextMeshProUGUI currencyUI;
   [SerializeField] Animator anim;


   private bool isMenuOpen= true;

   public void ToggleMenu() {
    isMenuOpen = !isMenuOpen;
    anim.SetBool("Menuopen", isMenuOpen);
   }

    private void OnGUI() {
        currencyUI.text = LevelManager.main.currency.ToString();
        
    }
    public void SetSelected(){
        
    }
}
