using UnityEngine;

public class ControlJeringas : MonoBehaviour
{
    [Header("Ajustes de Victoria")]
    public int jeringasRecogidas = 0;
    public int jeringasNecesarias = 3;

    [Header("Interfaz")]
    public Victory controladorVictoria;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jeringa"))
        {
            jeringasRecogidas++;
            Debug.Log("¡Jeringa recogida! Llevas: " + jeringasRecogidas + " de " + jeringasNecesarias);
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