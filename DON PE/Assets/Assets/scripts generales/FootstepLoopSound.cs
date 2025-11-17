using UnityEngine;

public class FootstepLoopSound : MonoBehaviour
{
    public AudioSource audioSrc;
    public KeyCode[] moveKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    void Update()
    {
        bool isPressing = false;

        foreach (KeyCode key in moveKeys)
        {
            if (Input.GetKey(key))
            {
                isPressing = true;
                break;
            }
        }

        if (isPressing && !audioSrc.isPlaying)
        {
            audioSrc.Play();
        }

        if (!isPressing && audioSrc.isPlaying)
        {
            audioSrc.Stop();
        }
    }
}
