using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> coins;

    public GameObject LevelEndPanel;

    public void CheckLevelEnd()
    {
        if(enemies.Count == 0 && coins.Count == 0)
        {
            LevelEndPanel.SetActive(true);
        }
    }

    public void LoadSameScene()
    {
        SceneManager.LoadScene(0);
    }
}
