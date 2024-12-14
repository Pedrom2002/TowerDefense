using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; // Bullets Per Second
    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPreFab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        // Encontrar todos os inimigos dentro do alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        Transform closestTank = null;
        Transform closestEnemy = null;
        float closestTankDistance = Mathf.Infinity;
        float closestEnemyDistance = Mathf.Infinity;

        // Percorrer todos os inimigos encontrados
        foreach (var hit in hits)
        {
            // Acessar o GameObject através do collider do hit
            GameObject enemy = hit.collider.gameObject;

            // Verificar se o inimigo tem a tag "Tank"
            if (enemy.CompareTag("Tank"))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestTankDistance)
                {
                    closestTankDistance = distance;
                    closestTank = enemy.transform; // A torre foca no tanque mais próximo
                }
            }
            // Se não for tanque, verificar se é um inimigo normal
            else if (enemy.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestEnemyDistance)
                {
                    closestEnemyDistance = distance;
                    closestEnemy = enemy.transform; // A torre foca no inimigo mais próximo
                }
            }
        }

        // Se um tanque foi encontrado, a torre foca nele
        if (closestTank != null)
        {
            target = closestTank;
        }
        // Caso contrário, foca no inimigo normal
        else if (closestEnemy != null)
        {
            target = closestEnemy;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + -90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) // Certificar que só desenha no editor
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, targetingRange);
        }
    }
}
