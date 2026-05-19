using UnityEngine;

public class CajaMunicion : MonoBehaviour
{
    [Header("Ajustes de Munición")]
    public int ammoAmount = 10;

    [Header("Audio (NUEVO)")]
    public AudioClip pickupSound; // Sonido de recarga/recogida (ej. un "click" metálico)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProjectileGun gun = other.GetComponentInChildren<ProjectileGun>();

            if (gun != null && gun.currentAmmo < gun.maxAmmo)
            {
                gun.AddAmmo(ammoAmount);

                // --- REPRODUCIR AUDIO ANTES DE DESTRUIR ---
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                Debug.Log("Caja de munición recogida. Munición actual: " + gun.currentAmmo);

                Destroy(gameObject);
            }
        }
    }
}