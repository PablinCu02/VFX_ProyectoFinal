//using UnityEngine;

//public class ControlJeringas : MonoBehaviour
//{
//    [Header("Ajustes de Victoria")]
//    public int jeringasRecogidas = 0;
//    public int jeringasNecesarias = 3;

//    [Header("Audio (NUEVO)")]
//    public AudioClip jeringaPickupSound; 

//    [Header("Interfaz")]
//    public Victory controladorVictoria;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Jeringa"))
//        {
//            jeringasRecogidas++;
//            Debug.Log("¡Jeringa recogida! Llevas: " + jeringasRecogidas + " de " + jeringasNecesarias);
//            if (jeringaPickupSound != null)
//            {
//                AudioSource.PlayClipAtPoint(jeringaPickupSound, other.transform.position);
//            }

//            Destroy(other.gameObject);

//            if (jeringasRecogidas >= jeringasNecesarias)
//            {
//                Debug.Log("¡VACUNA COMPLETADA! Has ganado.");

//                if (controladorVictoria != null)
//                {
//                    controladorVictoria.MostrarVictoria();
//                }
//            }
//        }
//    }
//}

using UnityEngine;
using TMPro; // NUEVO: Librería para manipular TextMeshPro

public class ControlJeringas : MonoBehaviour
{
    [Header("Ajustes de Victoria")]
    public int jeringasRecogidas = 0;
    public int jeringasNecesarias = 3;

    [Header("Audio (NUEVO)")]
    public AudioClip jeringaPickupSound;

    [Header("Interfaz")]
    public Victory controladorVictoria;
    public TextMeshProUGUI textoJeringas;

    void Start()
    {
        // Se mostrará "0/3" al iniciar el nivel
        ActualizarTexto();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jeringa"))
        {
            jeringasRecogidas++;
            ActualizarTexto(); // Refrescar el número en pantalla al recoger

            Debug.Log("¡Jeringa recogida! Llevas: " + jeringasRecogidas + " de " + jeringasNecesarias);

            // Código de tu compañero intacto
            if (jeringaPickupSound != null)
            {
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

    // Función para actualizar el texto 
    void ActualizarTexto()
    {
        if (textoJeringas != null)
        {
            textoJeringas.text = jeringasRecogidas + "/" + jeringasNecesarias;
        }
    }
}