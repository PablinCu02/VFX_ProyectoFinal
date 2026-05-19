using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))] // Asegura que el jugador tenga un AudioSource
public class PlayerHealth : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Puntos de Aparición (Spawns)")]
    public Transform spawnEstomago;
    public Transform spawnIntestino;

    [Header("Configuración de Daño")]
    public float acidDamage = 20f;

    [Header("Efectos de Audio (NUEVO)")]
    public AudioClip generalDamageSound; // Sonido genérico al recibir daño (ej. quejas, quejido del personaje)
    public AudioClip acidDamageSound;    // Sonido específico de ácido (ej. quemadura, corrosión o líquido hirviendo)
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        // Configuración del AudioSource del jugador
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 0f significa sonido 2D (se escucha directo en los audífonos del jugador)
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log("Vida actual: " + currentHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("¡Daño! Vida restante: " + currentHealth);

        // --- REPRODUCIR SONIDO DE DAÑO GENÉRICO ---
        // Solo suena si no caímos en ácido, para que no se encimen los audios
        if (generalDamageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(generalDamageSound);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("¡El jugador ha muerto! Reiniciando nivel...");
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AcidEstomago"))
        {
            PlayAcidSound();
            TakeDamage(acidDamage);
            if (currentHealth > 0) Respawn(spawnEstomago);
        }
        else if (other.CompareTag("AcidIntestino"))
        {
            PlayAcidSound();
            TakeDamage(acidDamage);
            if (currentHealth > 0) Respawn(spawnIntestino);
        }
    }

    // Función auxiliar para gestionar el sonido del ácido
    void PlayAcidSound()
    {
        if (acidDamageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(acidDamageSound);
        }
    }

    void Respawn(Transform puntoSeguro)
    {
        if (puntoSeguro != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            transform.position = puntoSeguro.position;

            if (cc != null) cc.enabled = true;
        }
    }
}