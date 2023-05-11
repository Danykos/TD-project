using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private GameObject range;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject texte;
    

    [Header("Attribute")]
    [SerializeField] public float targetInRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private int maxLvl = 5;

    private float bpsBase;
    private float targetingRangeBase;

    private Transform target;
    private float timeUntilFire;
    private Bullet bulletScript;

    private int level = 1;

    private void Start(){
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = baseUpgradeCost.ToString();
        bpsBase = bps;
        targetingRangeBase = targetInRange;

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update(){
         upgradeImage.rectTransform.localScale = new Vector3(targetInRange * 2.23f, targetInRange * 2.23f, targetInRange * 2.23f);   
        if(target == null){
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        if(!CheckTargetIsInRange()){
            target = null;
        }else{
        timeUntilFire += Time.deltaTime;
        
        if(timeUntilFire >= 1f / bps){
            Shoot();
            timeUntilFire = 0f;
        }}
    }

    private void Shoot(){
        GameObject bulletObj = Instantiate(bulletPrefab,firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private bool CheckTargetIsInRange(){
        return Vector2.Distance(target.position, transform.position) <= targetInRange;
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetInRange, (Vector2)transform.position, 0f, enemyMask);

        if(hits.Length > 0){
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget(){
    
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

        bps = CalculateBPS();

        targetInRange = CalculateRange();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach(Renderer renderer in renderers){
        Color newColor = renderer.material.color;
        newColor.r -= 0.2f;
        newColor.g -= 0.2f;
        newColor.b -= 0.2f;
        renderer.material.color = newColor;
    }
        
        
        Debug.Log("New BPS: " + bps + "New Cost: " + CalculateCost() + "New range: " + targetInRange + "Current lvl: " + level);
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
     private void OnMouseDown(){
         if(UIManager.main.IsHoveringUI())return;
         OpenUpgradeUI();
        }
}
