using UnityEngine;

public class Botiquin : MonoBehaviour
{
    [Header("Ajustes del Botiquín")]
    public float healAmount = 25f;

    [Header("Audio (NUEVO)")]
    public AudioClip healSound; // Sonido de curación (ej. un "gasp" de aire o efecto de jeringa)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.Heal(healAmount);

                // --- REPRODUCIR AUDIO ANTES DE DESTRUIR ---
                if (healSound != null)
                {
                    AudioSource.PlayClipAtPoint(healSound, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}