using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event System.Action BulletFiredEvent;
    public event System.Action LevelFailedEvent;
    public event System.Action LevelFinishedEvent;
    public List<GameObject> endgameWalls = new List<GameObject>();
    public int scoreTotal;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UIManager.instance.PlayButtonClickEvent += BulletFired;
        UIManager.instance.RestartButtonClickEvent += RestartButtonClickEvent;
    }

    public int CalculateHighScore()
    {
        for(int i = 0; i < endgameWalls.Count; i++) 
        {
            int subScore = 999 - endgameWalls[i].GetComponent<EndgameWall>().healthPoints;
            scoreTotal += subScore;
        }
        return scoreTotal;
    }

    private void RestartButtonClickEvent()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void BulletFired() 
    {
        BulletFiredEvent?.Invoke();
    }
    public void LevelFailed() 
    {
        LevelFailedEvent?.Invoke();
    }
    public void LevelFinished() 
    {
        LevelFinishedEvent?.Invoke();
    }
}
