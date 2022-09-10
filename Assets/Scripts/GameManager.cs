using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager sharedInstance;
    public bool gameStarted = false;
    public bool gamePaused = false;
    public AudioClip pausedAudio;
    public float invincibleTime = 0.0f;

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        StartCoroutine("StartGame");

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                GetComponent<AudioSource>().volume = 0.15f;
                PlayPauseMusic();
            }
            else
            {
                StopPauseMusic();
            }
        }
        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
    }
    
    void PlayPauseMusic()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = pausedAudio;
        source.loop = true;
        source.Play();
    }
    
    void StopPauseMusic()
    {
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(4.5f);
        gameStarted = true;
    }

    public void MakeInvincibleFor(float numberOfSeconds)
    {
        this.invincibleTime += numberOfSeconds;
    }
}
