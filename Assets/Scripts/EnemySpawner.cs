using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
   [Header("References")]
   [SerializeField] private GameObject[] enemyPrefabs;
   [SerializeField] private TextMeshProUGUI hpText;

   [Header("Attributes")]
   [SerializeField] private int baseEnemies = 8;
   [SerializeField] private float enemiesPerSecond = 0.5f;
   [SerializeField] private float timeBetweenWaves = 5f;
   [SerializeField] private float dificultyScalingFactor = 0.75f;
   [SerializeField] private float enemiesPerSecondCap = 15f;
   [SerializeField] private int Health = 20;


   [Header("Events")]
   public static UnityEvent onEnemyDestroy = new UnityEvent();

   private int currentWave = 1;
   private float timeSinceLastSpawn;
   private int enemiesAlive;
   private int enemiesLeftToSpawn;
   private bool isSpawning = false;
   private float eps;

   private void Awake(){
    onEnemyDestroy.AddListener(EnemyDestroyed);
   }

   private void Start(){
        StartCoroutine(StartWave());
        hpText.text = Health.ToString();
   }
    
    private void Update(){
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
        Health--;
        hpText.text = Health.ToString();
      
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
