using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeRemaining = 180;
    public bool timerIsRunning;
    public bool isPause;

    public TMP_Text timeText;
    public GameObject pausePanel;
    public Button btStageMenu;
    public Button btReplay;
    public Button btResume;

    public AudioSource onClickSFX;
    Scene scene;

    /*public GameObject endPanel;
    public Button btNextStage;
    public Button btStageMenu;
    public Button btReplay;*/

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        StartCoroutine(Wait());

        timerIsRunning = true;
        pausePanel.SetActive(false);
        btStageMenu.gameObject.SetActive(false);
        btReplay.gameObject.SetActive(false);
        btResume.gameObject.SetActive(false);
        isPause = false;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                Cursor.visible = false;
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    CheckPause();
                }
                //CheckPause();
            }
            else
            {
                Debug.Log("Time has run out!");
                Cursor.visible = true;
                timeText.text = "TIME OUT";

                UnActiveable();
                timeRemaining = 0;
                timerIsRunning = false;
                btStageMenu.gameObject.SetActive(true);
            }
        }
        else
        {
            UnActiveable();
        }
    }

    public void CheckPause()
    {
        if (isPause == false)
        {
            pausePanel.SetActive(true);
            btStageMenu.gameObject.SetActive(true);
            btReplay.gameObject.SetActive(true);
            btResume.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            timerIsRunning = false;
            UnActiveable();
            /* btMainMenu.gameObject.SetActive(true);
                pnPause.gameObject.SetActive(true);*/

            isPause = true;
        }
        else
        {
            pausePanel.SetActive(false);
            btStageMenu.gameObject.SetActive(false);
            btReplay.gameObject.SetActive(false);
            btResume.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            timerIsRunning = true;
            Activeable();
            /* btMainMenu.gameObject.SetActive(false);
                pnPause.gameObject.SetActive(false);*/
            isPause = false;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StageMenuOnClick()
    {
        onClickSFX.Play();
        SceneManager.LoadScene(sceneName:"Choose Stage");
    }
    
    public void MainMenuOnClick()
    {
        onClickSFX.Play();
        SceneManager.LoadScene(sceneName:"MainMenu");
    }

    public void ResumeOnClick()
    {
        onClickSFX.Play();
        CheckPause();
    }
    public void ReplayOnClick()
    {
        onClickSFX.Play();
        //SceneManager.LoadScene(scene.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(sceneName: "Stage 01");
    }

    private void Activeable()
    {
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        pl.GetComponent<PlayerCharacterController>().isPlaying = true;

        GameObject bs = GameObject.FindGameObjectWithTag("Boss");
        if (bs != null)
        {
            bs.GetComponent<BossScript>().isPlaying = true;
        }

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].GetComponent<EnemyController>().isPlaying = true;
        }
        GameObject[] grenadeList = GameObject.FindGameObjectsWithTag("Grenade02");
        if (grenadeList.Length != 0)
        {
            for (int i = 0; i < grenadeList.Length; i++)
            {
                grenadeList[i].GetComponent<BombStatus>().isAvtiveable = true;
            }
        }
    }
    private void UnActiveable()
    {
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        pl.GetComponent<PlayerCharacterController>().isPlaying = false;

        GameObject bs = GameObject.FindGameObjectWithTag("Boss");
        if (bs != null)
        {
            bs.GetComponent<BossScript>().isPlaying = false;
        }
        

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].GetComponent<EnemyController>().isPlaying = false;
        }
        GameObject[] grenadeList = GameObject.FindGameObjectsWithTag("Grenade02");
        if (grenadeList.Length != 0)
        {
            for (int i = 0; i < grenadeList.Length; i++)
            {
                grenadeList[i].GetComponent<BombStatus>().isAvtiveable = false;
            }
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3f);
    }
}
