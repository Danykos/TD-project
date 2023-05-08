using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Attribute")]
    [SerializeField] private float targetInRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;

    private Transform target;
    private float timeUntilFire;
    private Bullet bulletScript;

    private void Update(){
        
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
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected(){

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetInRange);

    }
}
