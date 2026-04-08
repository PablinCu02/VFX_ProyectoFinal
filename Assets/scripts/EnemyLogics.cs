using UnityEngine;
using System.Collections;

public class EnemyLogics : MonoBehaviour
{
    [Header("Salud y Efectos del Enemigo")]
    public float health = 50f;
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    [Header("Ataque al Jugador")]
    public float damageToPlayer = 20f;
    // --- NUEVO: Variables para el "delay" (Cooldown) ---
    public float attackRate = 2.0f; // Segundos que tarda en volver a morder
    private float nextAttackTime = 0f; // El cronómetro interno del virus

    private Renderer myRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        myRenderer = GetComponentInChildren<Renderer>();
        if (myRenderer != null)
        {
            originalColor = myRenderer.material.color;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Vida restante del enemigo: " + health);

        if (!isFlashing && myRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (health <= 0f)
        {
            Die();
        }
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
        Debug.Log("Enemigo eliminado");
        Destroy(gameObject);
    }

    // --- ACTUALIZADO: Detección física si chocan los cuerpos sólidos ---
    void OnCollisionStay(Collision collision)
    {
        // Verificamos si es el jugador Y si ya pasó el tiempo de delay
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                // Reiniciamos el cronómetro para el siguiente ataque
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    // --- ACTUALIZADO: Detección por el Hitbox (La esfera invisible) ---
    void OnTriggerStay(Collider other)
    {
        // Verificamos si es el jugador Y si ya pasó el tiempo de delay
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                // Reiniciamos el cronómetro para el siguiente ataque
                nextAttackTime = Time.time + attackRate;
            }
        }
    }
}