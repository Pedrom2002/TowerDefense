using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header ("Atributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;
    public void SetTarget(Transform _target){
        target = _target;
    }
    private void FixedUpdate() {
        if(!target) return;
        Vector2 direction =(target.position - transform.position).normalized;
        rb.linearVelocity= direction * bulletSpeed;
        
    }
    private bool hasHit = false;

private void OnCollisionEnter2D(Collision2D other) {
    if (hasHit) return; // Verifica se a bala já causou dano
    Health health = other.gameObject.GetComponent<Health>();
    if (health != null) {
        health.TakeDamage(bulletDamage); // Aplica dano
        hasHit = true; // Marca que a bala já acertou um inimigo
        Destroy(gameObject); // Destrói a bala após acertar o inimigo
    }
}
}

