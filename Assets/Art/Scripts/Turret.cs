using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;


    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private Transform target;
    private void Update() {
         if(target==null){
            FindTarget();
            return;
         } 

         RotateTowardsTarget();
         if (!CheckTargetIsInRange()){
            target = null;
         }

         bool CheckTargetIsInRange() {
            return Vector2.Distance(target.position, transform.position) <= targetingRange;
         }
    
    }

    private void FindTarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 
        0f,enemyMask);

        if(hits.Length > 0){
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget(){
         float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x -
          transform.position.x) * Mathf.Rad2Deg + -90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation,
        targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected() {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
} 

