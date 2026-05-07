using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausaUI;
    public GameObject panelControles; 
    private bool juegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si los controles están abiertos, ESC primero los cierra
            if (panelControles.activeSelf)
            {
                CerrarControles();
            }
            else if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        panelControles.SetActive(false); 
        Time.timeScale = 1f;
        juegoPausado = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void AbrirControles()
    {
        panelControles.SetActive(true);
    }

    public void CerrarControles()
    {
        panelControles.SetActive(false);
    }

    public void SalirAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}