using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    [SerializeField] AudioClip pause;

    AudioSource pauseAudio;
    
    [SerializeField] Player player;

    void Start() 
    {
        pauseAudio = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        // pauseAudio.PlayOneShot(pause, 1f);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        player.canAttack = true;
        //pauseAudio.PlayOneShot(pause, 1f);
        Time.timeScale = 1;
    }

    public void Home()
    {
        SceneManager.LoadScene(0);

        if (pauseAudio != null) 
        {
            pauseAudio.PlayOneShot(pause, 1f);
        }
    }
}
