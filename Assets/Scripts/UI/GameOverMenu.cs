using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] AudioClip overSound;

    AudioSource gameOverAudio;

    private void Start() 
    {
        gameOverAudio = GetComponent<AudioSource>();
    }

    public void BackToHomeScreen()
    {
        gameOverAudio.PlayOneShot(overSound, 1f);
        SceneManager.LoadScene(0);
    }
}
