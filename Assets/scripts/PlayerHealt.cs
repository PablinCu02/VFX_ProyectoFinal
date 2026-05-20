using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
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
    public AudioClip generalDamageSound;
    public AudioClip acidDamageSound;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
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
        Debug.Log("El jugador ha muerto");

        // Liberamos y hacemos visible el cursor para poder usar los botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cargamos la escena de GameOver
        SceneManager.LoadScene("GameOver");
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