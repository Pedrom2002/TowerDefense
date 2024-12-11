using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class turretslowmo : MonoBehaviour
{ 
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject iceEffectPart; // Referência ao efeito de gelo

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f; // Distância de alcance do efeito de gelo
    [SerializeField] private float aps = 4f; // Quantidade de vezes que a torre dispara por segundo (tempo mínimo para reativação)
    [SerializeField] private float freezeTime = 1f; // Duração do efeito de desaceleração
    private float timeUntilFire; // Tempo até a próxima ativação
    private bool isFreezing; // Indica se o efeito está ativo
    private List<EnemyMovement> affectedEnemies = new List<EnemyMovement>(); // Lista de inimigos afetados pelo gelo

    private void Update() {
        timeUntilFire += Time.deltaTime;

        if (!isFreezing && timeUntilFire >= 1f / aps) {
            CheckForEnemiesAndActivate();
        }
    }

    private void CheckForEnemiesAndActivate() {
        // Verifica se há inimigos no alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0) {
            ActivateFreezeEffect(); // Ativa o efeito de desaceleração e visual
        }
    }

    private void ActivateFreezeEffect() {
        isFreezing = true; // Marca que o efeito está ativo
        timeUntilFire = 0f; // Reseta o tempo de recarga

        // Ativa o efeito visual
        if (iceEffectPart != null) {
            iceEffectPart.SetActive(true);
        }

        // Inicia a coroutine para manter o efeito ativo durante o freezeTime
        StartCoroutine(FreezeEnemiesDuringEffect());
    }

    private IEnumerator FreezeEnemiesDuringEffect() {
        float timer = 0f;

        while (timer < freezeTime) {
            timer += Time.deltaTime;

            // Verifica inimigos no alcance
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
            foreach (RaycastHit2D hit in hits) {
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();

                if (em != null && !affectedEnemies.Contains(em)) {
                    affectedEnemies.Add(em); // Adiciona novos inimigos à lista
                    em.UpdateSpeed(0.5f); // Aplica o efeito de desaceleração
                }
            }

            yield return null; // Espera até o próximo frame
        }

        // Após o término do freezeTime, restaura a velocidade dos inimigos e desativa o efeito visual
        foreach (EnemyMovement em in affectedEnemies) {
            if (em != null) {
                em.ResetSpeed();
            }
        }

        affectedEnemies.Clear(); // Limpa a lista de inimigos afetados
        isFreezing = false; // Marca que o efeito terminou

        if (iceEffectPart != null) {
            iceEffectPart.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected() {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
