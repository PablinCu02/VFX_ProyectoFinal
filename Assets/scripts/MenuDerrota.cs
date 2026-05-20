using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{
    public void ReiniciarNivel()
    {
        // Cargamos la escena Main
        SceneManager.LoadScene("Main");
    }

    // Método para volver al menú principal
    public void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}