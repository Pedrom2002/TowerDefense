using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 5f;



    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) < 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

        rb.linearVelocity = 2*(direction * moveSpeed);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    public void UpdateSpeed(float newSpeed){
        moveSpeed = newSpeed;
    }

    public void ResetSpeed() {
        moveSpeed = baseSpeed;
        
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }
    public bool IsAlive { get; private set; } = true; // Verifica se o inimigo está vivo

    public void Die()
    {
        IsAlive = false; // Marca o inimigo como morto
        // Adicione aqui qualquer lógica de morte (destruição, animação, etc.)
        Destroy(gameObject); // Destroi o inimigo
    }

}