using UnityEngine;

public class Botiquin : MonoBehaviour
{
    [Header("Ajustes del Botiquín")]
    public float healAmount = 25f; // Cuánta vida recupera esta caja

    void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entró al área tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Buscamos el script de vida en el jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Si el jugador tiene el script y su vida no está al máximo, lo curamos
            if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.Heal(healAmount);

                // Destruimos el botiquín de la escena
                Destroy(gameObject);
            }
        }
    }
}