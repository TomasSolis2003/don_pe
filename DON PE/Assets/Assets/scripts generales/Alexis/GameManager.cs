using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string nombreEscena;
    public void CambiarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
    
}
