using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
   [Header("References")]
   [SerializeField] private GameObject[] enemyPrefabs;
   [SerializeField] private TextMeshProUGUI hpText;
   [SerializeField] private TextMeshProUGUI WaveText;
   [SerializeField] private GameObject endMenu;
   [SerializeField] private GameObject WonMenu;

   [Header("Attributes")]
   [SerializeField] private int baseEnemies = 8;
   [SerializeField] private float enemiesPerSecond = 0.5f;
   [SerializeField] private float timeBetweenWaves = 5f;
   [SerializeField] private float dificultyScalingFactor = 0.75f;
   [SerializeField] private float enemiesPerSecondCap = 15f;
   [SerializeField] private int Health = 20;
   [SerializeField] private int MaxWave = 10;


   [Header("Events")]
   public static UnityEvent onEnemyDestroy = new UnityEvent();

   private int currentWave = 1;
   private float timeSinceLastSpawn;
   private int enemiesAlive;
   private int enemiesLeftToSpawn;
   private bool isSpawning = false;
   private float eps;

   public void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(currentSceneName);
    }

   private void Awake(){
    onEnemyDestroy.AddListener(EnemyDestroyed);
   }

   private void Start(){
        StartCoroutine(StartWave());
        hpText.text = Health.ToString();
   }
   public void HP(){
        Health-=1;
        Debug.Log(Health);
        hpText.text = Health.ToString();
        if(Health == 0){
            endMenu.SetActive(true);
            Time.timeScale = 0;
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                
                Collider2D collider = obj.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }

        }

   }
    
    private void Update(){
                WaveText.text = "Current wave: " + currentWave;
        if(!isSpawning)return;

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0){
           SpawnEnemy();
           enemiesLeftToSpawn--;
           enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        
        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0){
            EndWave();
        }
    
    }

    private void EnemyDestroyed(){
        enemiesAlive--;
    }


   private IEnumerator StartWave(){
    yield return new WaitForSeconds(timeBetweenWaves);
    isSpawning = true;
    enemiesLeftToSpawn = EnemiesPerWave();
    eps = EnemiesPerSecond();
   }

   private void EndWave(){
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        
        if(currentWave==MaxWave) {
            WonMenu.SetActive(true);
            Time.timeScale = 0;
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                
                Collider2D collider = obj.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
            return;
        }
        currentWave++;

        StartCoroutine(StartWave());

   }

  

   private int EnemiesPerWave(){
    return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave,dificultyScalingFactor));
   }

   private void SpawnEnemy(){
    int index = Random.Range(0, enemyPrefabs.Length);
    GameObject prefabToSpawn = enemyPrefabs[index];
    Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
   }
   private float EnemiesPerSecond(){
    return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave,dificultyScalingFactor), 0f, enemiesPerSecondCap);
   }
}
