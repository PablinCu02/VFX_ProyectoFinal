using UnityEngine;

public class Botiquin : MonoBehaviour
{
    [Header("Ajustes del Botiquín")]
    public float healAmount = 25f;

    [Header("Audio")]
    public AudioClip healSound; // Sonido de curación 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.Heal(healAmount);
                if (healSound != null)
                {
                    AudioSource.PlayClipAtPoint(healSound, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}