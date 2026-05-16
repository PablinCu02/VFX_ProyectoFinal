using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class Victory : MonoBehaviour
{
    public GameObject pantallaVictoria;
    public void MostrarVictoria()
    {
        pantallaVictoria.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.Locked; 
        StartCoroutine(RegresoAutomatico(4f)); // Iniciamos la corrutina para el regreso automático después de 4 segundos
    }
    private IEnumerator RegresoAutomatico(float segundosDeEspera)
    {
        // Le decimos a Unity que espere los segundos en tiempo real
        yield return new WaitForSecondsRealtime(segundosDeEspera);
        Time.timeScale = 1f; 

        // Liberamos el mouse 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MenuPrincipal");
    }

}