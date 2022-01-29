using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject firstMenu;
    public GameObject startGame;
    public GameObject leaderboard;

    public AudioSource onClickSFX;

    public GameObject clearPanel;
    //public Button btNewGame;
    public Button btLoadGame;

    void Start()
    {
        firstMenu.SetActive(true);
        startGame.SetActive(false);
        leaderboard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackOnCLick()
    {
        onClickSFX.Play();
        firstMenu.SetActive(true);
        startGame.SetActive(false);
        leaderboard.SetActive(false);
    }
    public void StartGameOnClick()
    {
        onClickSFX.Play();
        //SceneManager.LoadScene("Stage01");
        firstMenu.SetActive(false);
        startGame.SetActive(true);
        leaderboard.SetActive(false);
        clearPanel.SetActive(false);

        string stagePass = PlayerPrefs.GetString("StagePassed");
        if (stagePass == "Stage 1 - 1")
        {
            btLoadGame.interactable = false;
        }
        else
        {
            btLoadGame.interactable = true;
        }
    }
    public void LeaderboardOnclick()
    {
        onClickSFX.Play();
        //SceneManager.LoadScene("Leaderboard");
        firstMenu.SetActive(false);
        startGame.SetActive(false);
        leaderboard.SetActive(true);
    }
    public void TutorialOnClick()
    {
        onClickSFX.Play();
        SceneManager.LoadScene(sceneName: "SampleScene");
    }
    public void QuitOnClick()
    {
        onClickSFX.Play();
        Application.Quit();
    }

    public void NewGameOnClick()
    {
        onClickSFX.Play();
        clearPanel.SetActive(true);
    }
    public void YesOnClick()
    {
        onClickSFX.Play();
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(sceneName: "Choose Stage");
    }
    public void NoOnClick()
    {
        onClickSFX.Play();
        clearPanel.SetActive(false);
    }
    public void LoadGameOnClick()
    {
        onClickSFX.Play();
        //StageManager stageManeger = gameObject.GetComponent<StageManager>();
        SceneManager.LoadScene(sceneName: "Choose Stage");
    }

}
