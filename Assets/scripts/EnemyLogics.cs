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

    [Header("Efectos de Muerte (Gelatina)")]
    public GameObject deathParticlesPrefab; // El sistema de partículas
    public GameObject smallEnemyPrefab;    // El enemigo pequeño que nacerá
    public int amountOfSmallEnemies = 2;    // Cuántos mini-slimes salen

    [Header("Ataque")]
    public float damageToPlayer = 20f;
    public float attackRate = 2.0f;
    private float nextAttackTime = 0f;

    private Renderer myRenderer;
    private Color originalColor;
    private bool isFlashing = false;
    private bool isDead = false; // Nueva bandera para evitar bugs en la muerte

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

        // 1. Instanciar partículas de gelatina
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        // 2. Aparecer enemigos pequeños
        SpawnSmallEnemies();

        // 3. Desaparecer el grande
        // Nota: Si la animación de muerte es importante, destruimos en 2s. 
        // Si quieres que desaparezca de golpe, usa 0.1s.
        Destroy(gameObject, 0.5f);
    }

    void SpawnSmallEnemies()
    {
        if (smallEnemyPrefab == null) return;

        for (int i = 0; i < amountOfSmallEnemies; i++)
        {
            // Creamos un pequeño desplazamiento para que no nazcan uno dentro de otro
            Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));

            GameObject miniSlime = Instantiate(smallEnemyPrefab, transform.position + spawnOffset, Quaternion.identity);

            // Si el mini-slime usa este mismo script, asegúrate de asignarle el player
            EnemyLogics miniLogic = miniSlime.GetComponent<EnemyLogics>();
            if (miniLogic != null) miniLogic.player = this.player;
        }
    }
}