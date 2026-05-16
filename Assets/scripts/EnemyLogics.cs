using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyLogics : MonoBehaviour
{
    [Header("Configuración de AI")]
    public Transform player;
    public float attackRange = 1.5f;
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Salud y Efectos")]
    public float health = 50f;
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    [Header("Efectos de Daño (NUEVO)")]
    public GameObject damageParticlesPrefab; // Partículas que saltan al recibir un golpe

    [Header("Efectos de Muerte (Gelatina)")]
    public bool canDivide = true;          // ACTÍVALO en el grande, DESACTÍVALO en el pequeño
    public GameObject deathParticlesPrefab; // El sistema de partículas de explosión
    public GameObject smallEnemyPrefab;
    public int amountOfSmallEnemies = 2;

    [Header("Ataque")]
    public float damageToPlayer = 20f;
    public float attackRate = 2.0f;
    private float nextAttackTime = 0f;

    private Renderer myRenderer;
    private Color originalColor;
    private bool isFlashing = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        myRenderer = GetComponentInChildren<Renderer>();

        if (myRenderer != null) originalColor = myRenderer.material.color;
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || agent == null || !agent.enabled || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            anim.SetFloat("Speed", 0f);
            anim.SetBool("isAttacking", true);

            if (Time.time >= nextAttackTime)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetFloat("Speed", agent.velocity.magnitude);
            anim.SetBool("isAttacking", false);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        health -= amount;

        // --- NUEVO: Instanciar partículas de daño en cada golpe ---
        if (damageParticlesPrefab != null)
        {
            Instantiate(damageParticlesPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        if (!isFlashing && myRenderer != null) StartCoroutine(FlashRed());
        if (health <= 0f) Die();
    }

    IEnumerator FlashRed()
    {
        isFlashing = true;
        myRenderer.material.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        myRenderer.material.color = originalColor;
        isFlashing = false;
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        agent.enabled = false;

        // Instanciar partículas de muerte
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        // --- MODIFICADO: Solo se divide si la casilla está marcada ---
        if (canDivide)
        {
            SpawnSmallEnemies();
        }

        Destroy(gameObject, 0.5f);
    }

    void SpawnSmallEnemies()
    {
        if (smallEnemyPrefab == null) return;

        for (int i = 0; i < amountOfSmallEnemies; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            GameObject miniSlime = Instantiate(smallEnemyPrefab, transform.position + spawnOffset, Quaternion.identity);

            EnemyLogics miniLogic = miniSlime.GetComponent<EnemyLogics>();
            if (miniLogic != null)
            {
                miniLogic.player = this.player;
                miniLogic.canDivide = false; // Por seguridad, le decimos por código que no se divida
            }
        }
    }
}