using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResetter : MonoBehaviour
{
    private const string IsRestartPrefName = "IsRestart";
    private const string LevelPrefName = "CurrentLevel";

    private void OnDestroy()
    {
        DOTween.Clear(true);
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt(IsRestartPrefName, Convert.ToInt32(true));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackInMainMenu(int currentLevel)
    {
        PlayerPrefs.SetInt(LevelPrefName, currentLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
