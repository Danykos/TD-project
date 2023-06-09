using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")] 
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;

    private void Start(){
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    private void Update(){
        if(Vector2.Distance(target.position, transform.position) <= 0.1f){
            pathIndex++;
             
            if (pathIndex == LevelManager.main.path.Length){
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                EnemySpawner enemySpawner = GameObject.Find("LevelManager").GetComponent<EnemySpawner>();
                enemySpawner.HP();
                return;
            }else{
                target = LevelManager.main.path[pathIndex];
            }
        }

    }

          private void FixedUpdate() {
    Vector2 direction = (target.position - transform.position).normalized;
    rb.velocity = direction * moveSpeed;
    float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    float rotationSpeed = 180f;
    Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
}




    public void UpdateSpeed(float newSpeed){
        moveSpeed = newSpeed;
    }
    public void ResetSpeed(){
        moveSpeed = baseSpeed;
    }
    
}
