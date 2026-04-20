using UnityEngine;
using UnityEngine.AI; // IMPORTANTE: Para usar NavMeshAgent

public class EnemyLogics : MonoBehaviour
{
    [Header("Configuración de AI")]
    public Transform player; // Arrastra el objeto Player aquí
    public float attackRange = 1.5f;

    private NavMeshAgent agent;
    private Animator anim;

    [Header("Salud y Efectos")]
    public float health = 50f;
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    [Header("Ataque")]
    public float damageToPlayer = 20f;
    public float attackRate = 2.0f;
    private float nextAttackTime = 0f;

    private Renderer myRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // Obtenemos el Animator
        myRenderer = GetComponentInChildren<Renderer>();

        if (myRenderer != null) originalColor = myRenderer.material.color;

        // Buscar al jugador automáticamente si no está asignado
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // LÓGICA DE ESTADOS
        if (distance <= attackRange)
        {
            // ESTADO: ATACAR
            agent.isStopped = true; // Detener movimiento
            anim.SetFloat("Speed", 0f); // Animación Idle
            anim.SetBool("isAttacking", true); // Activar ataque

            // Intentar infligir daño
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
            // ESTADO: CAMINAR / SEGUIR
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Enviamos la velocidad al Animator (usamos .magnitude para obtener un valor positivo)
            anim.SetFloat("Speed", agent.velocity.magnitude);
            anim.SetBool("isAttacking", false);
        }
    }

    // Mantenemos tus funciones originales de daño
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (!isFlashing && myRenderer != null) StartCoroutine(FlashRed());
        if (health <= 0f) Die();
    }

    System.Collections.IEnumerator FlashRed()
    {
        isFlashing = true;
        myRenderer.material.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        myRenderer.material.color = originalColor;
        isFlashing = false;
    }

    void Die()
    {
        anim.SetTrigger("Die"); // Asumiendo que tienes una animación de muerte
        agent.enabled = false;
        Destroy(gameObject, 2f);
    }
}