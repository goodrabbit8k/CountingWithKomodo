using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip click;

    AudioSource mainMenuAudio;

    void Start() 
    {
        mainMenuAudio = GetComponent<AudioSource>();    
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        mainMenuAudio.PlayOneShot(click, 1f);
        Time.timeScale = 1;
    }

    public void QuitGame() 
    {
        mainMenuAudio.PlayOneShot(click, 1f);
        Application.Quit();
    }
}
