using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        // Al iniciar, el jugador tiene la vida al máximo
        currentHealth = maxHealth;
    }

    // Esta función será llamada por los botiquines
    public void Heal(float amount)
    {
        currentHealth += amount;

        // Evitamos que la vida supere el máximo permitido
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("¡Botiquín recogido! Vida actual: " + currentHealth);
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("¡El virus te atacó! Vida restante: " + currentHealth);

        // Comprobamos si la vida se agotó
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("¡El jugador ha sido infectado/eliminado!");
            // PENDIENTE LOGICA DE GAME OVER
        }
    }
}