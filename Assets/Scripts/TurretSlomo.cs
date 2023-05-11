using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class TurretSlomo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private GameObject range;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject texte;

    [Header("Attribute")]
    [SerializeField] private float targetInRange = 5f;
    [SerializeField] private float aps = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private int maxLvl = 5;

    private float timeUntilFire;
    private float bpsBase;
    private float targetingRangeBase;
    
    private int level = 1;

    private void Start(){
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = baseUpgradeCost.ToString();
        bpsBase = aps;
        targetingRangeBase = targetInRange;

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update(){
        upgradeImage.rectTransform.localScale = new Vector3(targetInRange * 2.23f, targetInRange * 2.23f, targetInRange * 2.23f); 
       
        
        timeUntilFire += Time.deltaTime;
        
        if(timeUntilFire >= 1f / aps){
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }
    private void FreezeEnemies(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetInRange, (Vector2)transform.position, 0f, enemyMask);
        if(hits.Length > 0){
            for(int i =0; i<hits.Length; i++){
                RaycastHit2D hit = hits[i];

                EnemyMovement em= hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f); 

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em){
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }

    
    private void OnMouseDown(){
         if(UIManager.main.IsHoveringUI())return;
         OpenUpgradeUI();
        }
    public void OpenUpgradeUI(){
        upgradeUI.SetActive(true);
        range.SetActive(true);
       

        
    }
    public void CloseUpgradeUI(){
        upgradeUI.SetActive(false);
        range.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade(){
        if(CalculateCost() > LevelManager.main.currency || level >= maxLvl) return;

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        aps = CalculateBPS();

        targetInRange = CalculateRange();

        Color newColor = GetComponent<Renderer>().material.color;
        newColor.r -= 0.2f;
        newColor.g -= 0.2f;
        newColor.b -= 0.2f;
        GetComponent<Renderer>().material.color = newColor;
        
        
        Debug.Log("New BPS: " + aps + "New Cost: " + CalculateCost() + "New range: " + targetInRange + "Current lvl: " + level);
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = CalculateCost().ToString();
        if (level == maxLvl) {
        upgradeButton.interactable = false;
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Max lvl";
        }
    CloseUpgradeUI();
    }

    private int CalculateCost(){
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level,1f));

    }

    private float CalculateBPS(){
        return bpsBase * Mathf.Pow(level,0.6f);
    }
     private float CalculateRange(){
        return targetingRangeBase * Mathf.Pow(level,0.4f);
    }
}
