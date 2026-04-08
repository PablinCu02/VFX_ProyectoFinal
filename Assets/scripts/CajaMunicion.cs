using UnityEngine;

public class CajaMunicion : MonoBehaviour
{
    [Header("Ajustes de Munición")]
    public int ammoAmount = 10; // Cuántas balas recupera esta caja

    void OnTriggerEnter(Collider other)
    {
        // Verificamos si quien tocó la caja es el jugador
        if (other.CompareTag("Player"))
        {
            // Buscamos el script ProjectileGun en el jugador o en sus hijos (por si el arma está en la cámara)
            ProjectileGun gun = other.GetComponentInChildren<ProjectileGun>();

            // Si encontramos el arma y la munición no está al máximo, recargamos
            if (gun != null && gun.currentAmmo < gun.maxAmmo)
            {
                gun.AddAmmo(ammoAmount);

                // Destruimos la caja de munición de la escena
                Destroy(gameObject);
            }
            Debug.Log("Caja de munición recogida. Munición actual: " + gun.currentAmmo);
        }
    }

}