using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public event System.Action BuyButtonClickEvent;
    public event System.Action PlayButtonClickEvent;
    public event System.Action RestartButtonClickEvent;

    public GameObject playButton, buyButton, levelFailedUI, continueUI;
    public TextMeshProUGUI scoreText;
    void Awake()
    {
        instance = this;
        PlayButtonClickEvent += CloseUI;
    }
    private void Start()
    {
        GameManager.instance.LevelFailedEvent += LevelFailed;
        GameManager.instance.LevelFinishedEvent += LevelFinished;
    }

    private void LevelFailed()
    {
        levelFailedUI.SetActive(true);   
    }
    private void LevelFinished() 
    {
        scoreText.text = "Highscore: " + GameManager.instance.CalculateHighScore().ToString();
        continueUI.SetActive(true);
    }
    private void CloseUI()
    {
        playButton.SetActive(false);
        buyButton.SetActive(false);
    }

    public void OnBuyButtonClick()
    {
        BuyButtonClickEvent?.Invoke();
    }
    public void OnPlayButtonClick() 
    {
        PlayButtonClickEvent?.Invoke();
    }
    public void OnRestartButtonClick() 
    {
        RestartButtonClickEvent?.Invoke();
    }
}
