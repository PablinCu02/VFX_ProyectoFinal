using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(AudioSource))] // Esto obliga a que el objeto tenga un AudioSource
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

    [Header("Efectos de Audio (NUEVO)")]
    public AudioClip damageSound;  // Sonido al recibir un balazo/golpe
    public AudioClip deathSound;   // Sonido asqueroso de explosión al morir
    private AudioSource audioSource;

    [Header("Efectos de Daño")]
    public GameObject damageParticlesPrefab;

    [Header("Efectos de Muerte (Gelatina)")]
    public bool canDivide = true;
    public GameObject deathParticlesPrefab;
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

        // Obtener el componente de audio
        audioSource = GetComponent<AudioSource>();
        // Configuración inicial de audio para que sea 3D (se escuche según la distancia)
        audioSource.spatialBlend = 1f;

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

        // --- REPRODUCIR SONIDO DE DAÑO ---
        if (damageSound != null && audioSource != null)
        {
            // PlayOneShot permite que si le disparas rápido, los sonidos se encimen de forma natural
            audioSource.PlayOneShot(damageSound);
        }

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

        // --- REPRODUCIR SONIDO DE MUERTE ---
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        if (canDivide)
        {
            SpawnSmallEnemies();
        }

        // Le damos 0.6 segundos para que se alcance a escuchar el sonido de muerte antes de borrar el objeto
        Destroy(gameObject, 0f);
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
                miniLogic.canDivide = false;
            }
        }
    }
}