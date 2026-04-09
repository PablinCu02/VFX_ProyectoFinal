using UnityEngine;
using UnityEngine.SceneManagement; // LIBRERÍA NECESARIA PARA REINICIAR

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

    void Start()
    {
        currentHealth = maxHealth;
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

        // COMPROBACIÓN DE MUERTE
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("¡El jugador ha muerto! Reiniciando nivel...");

        // Obtiene el nombre de la escena actual y la vuelve a cargar
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AcidEstomago"))
        {
            TakeDamage(acidDamage);
            if (currentHealth > 0) Respawn(spawnEstomago);
        }
        else if (other.CompareTag("AcidIntestino"))
        {
            TakeDamage(acidDamage);
            if (currentHealth > 0) Respawn(spawnIntestino);
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