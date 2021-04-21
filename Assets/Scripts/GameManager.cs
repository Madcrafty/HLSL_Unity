using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI FPS;
    public float m_refreshTime = 0.5f;
    [Tooltip("How many seconds you have to complete the level")]
    public int LevelTime;
    //public bool debuging;

    private string fpsBaseText;
    private int winScreenIndex = 2;
    private int gameOverScreenIndex = 3;
    private PostProcessor pp;
    private float curTime;

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    private bool timerActive;
    //Start is called before the first frame update
    void Start()
    {
        //scoreBoardBaseText = scoreBoard.text;
        //fpsBaseText = FPS.text;
        SceneManager.activeSceneChanged += ChangedActiveScene;
        DontDestroyOnLoad(this);
        //UpdateScoreBoard();
        Cursor.lockState = CursorLockMode.Locked;
        
        curTime = LevelTime;
        
        UpdateTimer();
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
        timerActive = false;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        pp = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessor>();
    }

    void ChangedActiveScene(Scene current, Scene next)
    {
        //scoreBoard = GameObject.Find("ScoreBoard").GetComponent<TextMeshProUGUI>();
        //scoreBoardBaseText = scoreBoard.text;
        //UpdateScoreBoard();
        SceneManager.activeSceneChanged -= ChangedActiveScene;
        Destroy(gameObject);
        //if (next.name != "00 Starter Scene")
        //{
        //    Destroy(gameObject);
        //    Destroy(this);
        //}
    }
    /// <summary>
    /// I wanted to turn on the blur then transition slowly back to normal
    /// I was unable to change the material the camera was using for the post-processing
    /// also the code would break at the yeild return in the loops
    /// also when the scene reloads it cant find the camera on start
    /// </summary>
    /// <param name="power"></param>
    /// <param name="time"></param>
    /// <returns></returns>

    public IEnumerator BlurScreenForTime(float power, float time)
    {
        // Current implementation
        pp.enabled = true;
        yield return new WaitForSeconds(time);
        pp.enabled = false;

        // ---While loop implementaion---
        // WaitForSeconds wfs = ;
        //float timer = 0;
        //while(timer < time)
        //{
        //    yield return new WaitForSeconds;
        //    pp.SetFloat("Radius", power * (1- (timer/time)));
        //    timer += Time.deltaTime;
        //}

        // ---For loop implementaion---
        //for (float i = 0; i < time; i+= Time.deltaTime)
        //{
        //    yield return wfs;
        //    pp.SetFloat("Radius", power * (1 - (i / time)));
        //}
        //pp.SetFloat("Radius",0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
            FPS.text = fpsBaseText + (int)m_lastFramerate;
        }
        if (timerActive)
        {
            curTime -= Time.deltaTime;
            UpdateTimer();
        } 
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    private void UpdateTimer()
    {
        // 3600 = 1 hour, 
        if (curTime >= 3600)
        {
            curTime = 3599f;
        }

        if (curTime < 0)
        {
            curTime = 0f;
        }

        int seconds = (int)curTime % 60;
        int minutes = (int)(curTime / 60) % 60;

        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

        Timer.text = timerString;

        if (curTime == 0 && timerActive)
        {
            GameOver();
        }
    }
    // Use to enable and disable the timer
    public void SetTimerState(bool a_active)
    {
        timerActive = a_active;
    }
    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }
    public void GameWon()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(winScreenIndex);
    }
}
