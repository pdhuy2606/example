using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Button btBack, btReset, btShop; 
    public Button btStage11, btStage12, btStage13, btStage14, btStage15;
    public TMP_Text totalCrysText;
    public GameObject shopComing;
    public AudioSource onClickSFX;
    

    string stagePassed;
    int totalCrys;

    // Start is called before the first frame update
    void Start()
    {
        shopComing.SetActive(false);
        totalCrys = PlayerPrefs.GetInt("TotalCrystal");
        totalCrysText.text = totalCrys.ToString();
        stagePassed = PlayerPrefs.GetString("StagePassed");
        btStage12.interactable = false;
        btStage13.interactable = false;
        btStage14.interactable = false;
        btStage15.interactable = false;

        switch (stagePassed)
        {
            case "Stage 1-1":
                btStage12.interactable = true;
                break;
            case "Stage 1-2":
                btStage12.interactable = true;
                btStage13.interactable = true;
                break;
            case "Stage 1-3":
                btStage12.interactable = true;
                btStage13.interactable = true;
                btStage14.interactable = true;
                break;
            case "Stage 1-4":
                btStage12.interactable = true;
                btStage13.interactable = true;
                btStage14.interactable = true;
                btStage15.interactable = true;
                break;
        }
    }
    

    public void stageToLoad(string stage)
    {
        onClickSFX.Play();
        SceneManager.LoadScene(stage);
    }
    public void stagePassedToSave(string stageName)
    {
        PlayerPrefs.SetString("StagePassed", stageName);
    }
    public void resetPlayerPrefs()
    {
        onClickSFX.Play();
        btStage12.interactable = false;
        btStage13.interactable = false;
        btStage14.interactable = false;
        btStage15.interactable = false;
        PlayerPrefs.DeleteAll();
        totalCrys = PlayerPrefs.GetInt("TotalCrystal");
        totalCrysText.text = totalCrys.ToString();

    }

    public void BackOnClick()
    {
        onClickSFX.Play();
        SceneManager.LoadScene("MainMenu");
    }
    public void ShopOnClick()
    {
        onClickSFX.Play();
        StartCoroutine(Shop());
    }
    IEnumerator Shop()
    {
        shopComing.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        shopComing.SetActive(false);
    }
}
