using UnityEngine;

public class LoopSoundOnMove : MonoBehaviour
{
    public AudioSource audioSrc; // Debe tener activado "Loop"
    public KeyCode[] moveKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    void Update()
    {
        bool isPressing = false;

        // ¿Alguna tecla está siendo presionada?
        foreach (KeyCode key in moveKeys)
        {
            if (Input.GetKey(key))
            {
                isPressing = true;
                break;
            }
        }

        // Encender sonido si se presiona
        if (isPressing && !audioSrc.isPlaying)
        {
            audioSrc.Play();
        }

        // Apagar sonido si no se presiona nada
        if (!isPressing && audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
    }
}
