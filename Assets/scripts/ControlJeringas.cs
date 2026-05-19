using UnityEngine;

public class ControlJeringas : MonoBehaviour
{
    [Header("Ajustes de Victoria")]
    public int jeringasRecogidas = 0;
    public int jeringasNecesarias = 3;

    [Header("Audio (NUEVO)")]
    public AudioClip jeringaPickupSound; // Asigna aquí tu sonido de recolección

    [Header("Interfaz")]
    public Victory controladorVictoria;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jeringa"))
        {
            jeringasRecogidas++;
            Debug.Log("¡Jeringa recogida! Llevas: " + jeringasRecogidas + " de " + jeringasNecesarias);

            // --- REPRODUCIR SONIDO ANTES DE DESTRUIR LA JERINGA ---
            if (jeringaPickupSound != null)
            {
                // Suena exactamente en la posición de la jeringa antes de borrarla
                AudioSource.PlayClipAtPoint(jeringaPickupSound, other.transform.position);
            }

            Destroy(other.gameObject);

            if (jeringasRecogidas >= jeringasNecesarias)
            {
                Debug.Log("¡VACUNA COMPLETADA! Has ganado.");

                if (controladorVictoria != null)
                {
                    controladorVictoria.MostrarVictoria();
                }
            }
        }
    }
}