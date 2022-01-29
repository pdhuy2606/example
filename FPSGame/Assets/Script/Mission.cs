using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Timer time;
    public bool timeIsRunning;
    public float timeRemain;
    public int isWin;
    public string stageName;

    public Scene stage;
    public TMP_Text missionTextTitle;
    public TMP_Text missionText;
    public GameObject endPanel;
    public Button btNextStage;
    public Button btReplay;
    public Button btStageMenu;
    public AudioSource onClickSFX;

    string[] stageList = { "Stage 1-1", "Stage 1-2", "Stage 1-3", "Stage 1-4", "Stage 1-5" };
    void Start()
    {
        isWin = 0;
        time = GetComponent<Timer>();
        timeIsRunning = time.timerIsRunning;
        timeRemain = time.timeRemaining;

        stage = SceneManager.GetActiveScene();
        stageName = stage.name;

        endPanel.gameObject.SetActive(false);
        btNextStage.gameObject.SetActive(false);
        btReplay.gameObject.SetActive(false);
        btStageMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWin == 0)
        {
            CheckMission(stage);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            time.timerIsRunning = false;
            endPanel.gameObject.SetActive(true);
            btNextStage.gameObject.SetActive(true);
            btReplay.gameObject.SetActive(true);
            btStageMenu.gameObject.SetActive(true);

            if (isWin == 1)
            {
                missionTextTitle.text = "MISSION COMPLETE!";
                /*StageManager stageManager = GetComponent<StageManager>();
                stageManager.stagePassedToSave(stage.name.ToString());*/
                PlayerPrefs.SetString("StagePassed", stageName);
            }
            if (isWin == 2)
            {
                btNextStage.interactable = false;
                missionTextTitle.text = "MISSION FAILED!";
            }
        }
    }
    public void CheckMission(Scene stage)
    {
        /*for(int i = 0; i < stageList.Length; i++)
        {
            if(stage.name == stageList[i])
            {

            }
        }*/
        switch (stage.name)
        {
            case "Stage 1-1":
                MissionDestroy();
                break;
            case "Stage 1-2":
                MissionCollect();
                break;
            case "Stage 1-3":
                MissionSpeedrun();
                break;
            case "Stage 1-4":
                MissionPuzzle();
                break;
             case "Stage 1-5":
                MissionBoss();
                break;
        }
    }

    public void MissionDestroy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        missionTextTitle.text = "ELIMINATE ALL THE ENEMIES";
        missionText.text = "Enemies remains: " + enemyList.Length.ToString();
        if (timeRemain > 0 && enemyList.Length == 0)
        {
            isWin = 1;
        }
        if (timeRemain == 0 && enemyList.Length > 0)
        {
            isWin = 2;
        }
    }
    public void MissionCollect()
    {
        GameObject[] itemList = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] itemActiveList = GameObject.FindGameObjectsWithTag("ActivedItem");
        //GameObject[] itemUnActiveList = GameObject.FindGameObjectsWithTag("UnActivedItem");
        missionTextTitle.text = "ACTIVATE ALL THE ITEMS";
        missionText.text = "Items remains: " + (itemList.Length - itemActiveList.Length).ToString();
        if (timeRemain > 0 && itemList.Length - itemActiveList.Length == 0)
        {
            isWin = 1;
        }
        if (timeRemain == 0 && itemList.Length - itemActiveList.Length > 0)
        {
            isWin = 2;
        }
       /* if (timeRemain > 0 && itemActiveList.Length == itemList.Length)
        {
            isWin = true;
        }
        if (timeRemain == 0 && itemActiveList.Length != itemList.Length)
        {
            isWin = false;
        }*/
    }
    public void MissionSpeedrun()
    {
        GameObject[] itemList = GameObject.FindGameObjectsWithTag("ActivedItem");
        missionTextTitle.text = "RUN TO THE DESTINATION";
        missionText.text = "Find the Unactived Item and activate it in time";
        if (timeRemain > 0 && itemList.Length != 0)
        {
            isWin = 1;
        }
        if (timeRemain == 0 && itemList.Length == 0)
        {
            isWin = 2;
        }
    }
    public void MissionPuzzle()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] grenadeList = GameObject.FindGameObjectsWithTag("Grenade");
        missionTextTitle.text = "DESTROY ALL THE GRENADES";
        missionText.text = "Avoid eliminating enemies. Grenades remains: " + grenadeList.Length;
        if (timeRemain > 0 && grenadeList.Length == 0  && enemyList.Length == 5)
        {
            isWin = 1;
        }
        if ((timeRemain == 0 && grenadeList.Length != 0) || (timeRemain > 0 && enemyList.Length < 5) )
        {
            isWin = 2;
        }
    }

    public void MissionBoss()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        missionTextTitle.text = "ELIMINATE THE BOSS";
        missionText.text = "Find and Destroy the Boss in time";
        if (timeRemain > 0 && boss == null)
        {
            isWin = 1;
        }
        if (timeRemain == 0 && boss != null)
        {
            isWin = 2;
        }
    }

    public void StageMenuOnClick()
    {
        onClickSFX.Play();
        if (isWin == 1)
        {
            GameObject pL = GameObject.FindGameObjectWithTag("Player");
            pL.GetComponent<PlayerCharacterController>().SaveTotalCrystal();
        }
        SceneManager.LoadScene(sceneName: "Choose Stage");
    }

    public void NextStageOnClick()
    {
        onClickSFX.Play();
        GameObject pL = GameObject.FindGameObjectWithTag("Player");
        pL.GetComponent<PlayerCharacterController>().SaveTotalCrystal();
        for (int i = 0; i < stageList.Length; i++)
        {
            if (SceneManager.GetActiveScene().name.ToString() == stageList[stageList.Length - 1])
            {
                SceneManager.LoadScene(sceneName: "Choose Stage");
            }
            else  if (SceneManager.GetActiveScene().name.ToString() == stageList[i])
            {
                SceneManager.LoadScene(stageList[i + 1]);
            }
        }
       
        //SceneManager.LoadScene(sceneName: "");
    }
    public void ReplayOnClick()
    {
        onClickSFX.Play();
        if (isWin == 1)
        {
            GameObject pL = GameObject.FindGameObjectWithTag("Player");
            pL.GetComponent<PlayerCharacterController>().SaveTotalCrystal();
        }
        //SceneManager.LoadScene(scene.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(sceneName: "Stage 01");
    }
}
