using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Main");
    }

    // Botón de Salir
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); 
    }
}