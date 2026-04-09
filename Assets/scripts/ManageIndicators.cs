using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    [Header("Elementos de la Pantalla (Canvas)")]
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoMunicion;

    [Header("Referencias del Jugador")]
    public PlayerHealth playerHealth;
    public ProjectileGun playerGun;

    void Update()
    {
        // Se valida el script de vida y actualizamos el texto
        if (playerHealth != null && textoVida != null)
        {
            textoVida.text = playerHealth.currentHealth.ToString();
        }

        // Revisamos que el script del arma esté conectado y actualizamos el texto
        if (playerGun != null && textoMunicion != null)
        {
            textoMunicion.text = playerGun.currentAmmo.ToString();
        }
    }
}