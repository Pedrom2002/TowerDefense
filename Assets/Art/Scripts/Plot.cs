using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    private GameObject towerObj;
    public Turret turret;
    private Color startColor;

    private void Start() {
        startColor = sr.color;
    }

    private void OnMouseEnter() {
        sr.color = hoverColor;
        
    }
    private void OnMouseExit() {
        sr.color = startColor;
    }
    private void OnMouseDown() {
        if(towerObj != null){
            return;
            
        }
        
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if(towerToBuild.cost > LevelManager.main.currency){
            return;
        }
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
    }
}

