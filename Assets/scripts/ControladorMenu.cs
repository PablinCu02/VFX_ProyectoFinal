using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class ControladorMenu : MonoBehaviour
{
    // Esta función la conectaremos al botón de Inicio
    public void Jugar()
    {
        // OJO: Cambia "NombreDeTuEscenaDeJuego" por el nombre exacto de tu escena del laberinto
        SceneManager.LoadScene("Main");
    }

    // Esta función la conectaremos al botón de Salir
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); // Esto cerrará el juego cuando ya esté exportado (.exe)
    }
}